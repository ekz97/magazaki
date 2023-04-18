global using Hangfire;
global using Hangfire.Dashboard;
global using Hangfire.MemoryStorage;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using PeasieLib.Filters;
global using PeasieLib.Handlers;
global using PeasieLib.Models.DB;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Cryptography;
global using System.Security.Cryptography.X509Certificates;
global using System.Text;
global using System.Text.Json;
using Microsoft.IdentityModel.Logging;
using Serilog;
using System.Threading.RateLimiting;
using PeasieLib.Middleware;
using PeasieLib.Extensions;
using Hangfire.Common;
using PeasieAPI.Interfaces;
using PeasieAPI.Models.DB;

// TODO
// Header protection
// https://flerka.github.io/personal-blog/2022-06-21-ahead-of-time-compilation/#lets-try-native-aot-in-console AOT!!


namespace PeasieAPI
{
    public class Program
    {
        private static ApplicationContextService? _applicationContextService;

        public static void Main(string[] args)
        {
            _applicationContextService = new ApplicationContextService();

            // Create the app builder:
            var builder = WebApplication.CreateBuilder(args);

            // Add configurations to the container:
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            // Add loggers to the container:
            builder.Logging.AddJsonConsole();
            builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Debug()
               .Enrich.WithThreadId()
               .Enrich.WithThreadName()
               .Enrich.FromLogContext()
               .Enrich.WithProperty("Version", "1.0.0")
               .ReadFrom.Configuration(ctx.Configuration));

            // Add rate limiter tot the container.
            // ----------------------------------
            builder.Services.AddRateLimiter(options =>
            {
                options.OnRejected = async (context, token) =>
                {      
                    context.HttpContext.Response.StatusCode = 429; // 418: Teapot
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        await context.HttpContext.Response.WriteAsync(
                            $"Too many requests. Please try again after {retryAfter.TotalMinutes} minute(s)." /*+ $"Read more about our rate limits at https://example.org/docs/ratelimiting."*/, cancellationToken: token);
                    }
                    else
                    {
                        await context.HttpContext.Response.WriteAsync(
                            "Too many requests. Please try again later." /* + "Read more about our rate limits at https://example.org/docs/ratelimiting."*/, cancellationToken: token);
                    }
                };

                options.GlobalLimiter = PartitionedRateLimiter.CreateChained(
                        PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                        RateLimitPartition.GetFixedWindowLimiter(httpContext.ResolveClientIpAddress(), partition =>
                        new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 600,
                            Window = TimeSpan.FromMinutes(1)
                        })),
                        PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                        RateLimitPartition.GetFixedWindowLimiter(httpContext.ResolveClientIpAddress(), partition =>
                        new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 6000,
                            Window = TimeSpan.FromHours(1)
                        })),
                        PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                        RateLimitPartition.GetFixedWindowLimiter(httpContext.ResolveClientIpAddress(), partition =>
                        new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 10,
                            QueueLimit = 6,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            Window = TimeSpan.FromSeconds(1)
                        }))
                    );
            });

            builder.Services.Configure<IPWhitelistOptions>(builder.Configuration.GetSection("IPWhitelistOptions"));

            // Parameters:
            var connectionString = builder.Configuration.GetConnectionString("PeasieAPIDB") ?? "";
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!));
            _applicationContextService.Audience = builder.Configuration["Jwt:Audience"]!;
            _applicationContextService.Issuer = builder.Configuration["Jwt:Issuer"]!;
            var signingCertificate = new CertificateRequest("cn=foobar", RSA.Create(), HashAlgorithmName.SHA512, RSASignaturePadding.Pss).CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1));
            var encryptingCertificate = new CertificateRequest("cn=foobar", RSA.Create(), HashAlgorithmName.SHA512, RSASignaturePadding.Pss).CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1));
            var signingCertificateKey = new X509SecurityKey(signingCertificate);
            var encryptingCertificateKey = new X509SecurityKey(encryptingCertificate);
            var signingKeys = new List<SecurityKey> { symmetricKey, signingCertificateKey };

            IdentityModelEventSource.ShowPII = true; // NOT GDPR safe!

            // Add API services to the container.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SupportNonNullableReferenceTypes();
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JSON Web Token based security",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });
            builder.Services.AddHealthChecks();

            // Add AuthZ and AuthN services.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireSignedTokens = true,
                    ValidIssuer = _applicationContextService.Issuer,
                    ValidAudience = _applicationContextService.Audience,
                    IssuerSigningKeys = signingKeys,
                    TokenDecryptionKeys = new List<SecurityKey>
                    {
                        new EncryptingCredentials(symmetricKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512).Key,
                        new EncryptingCredentials(encryptingCertificateKey, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512).Key
                    },
                    IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) => signingKeys
                };
            });
            builder.Services.AddAuthorization(options => options.AddPolicy("IsAuthorized", policy => policy.Requirements.Add(new PeasieAuthorizationRequirement())));

            // Add DB services.
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
            // Add DB services.
            builder.Services.AddDbContext<PeasieAPIDbContext>(options =>
                    options.UseMySql(connectionString, serverVersion)
                    .UseLoggerFactory(LoggerFactory.Create(b => b.AddFilter(level => level >= LogLevel.Information)))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                );

            var dataProtectionConnectionString = builder.Configuration.GetConnectionString("DataProtectionConnection") ?? throw new InvalidOperationException("Connection string 'DataProtectionConnection' not found.");
            builder.Services.AddCustomDataProtection(dataProtectionConnectionString);

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Add performance booster services.
            builder.Services.AddResponseCompression();
            builder.Services.AddRequestDecompression();

            // Add custom services to the container.
            builder.Services.AddSingleton<IApplicationContextService>(_applicationContextService);
            builder.Services.AddScoped<IAuthorizationHandler, PeasieAuthorizationHandler>();
            builder.Services.AddScoped<PeasieEndpointHandler>();

            // Add third-parties services to the container.
            builder.Services.AddHangfire(configuration => configuration.UseMemoryStorage()).AddHangfireServer();
            JobStorage.Current = new MemoryStorage();

            if (builder.Environment.IsDevelopment())
            {
                // Move services you want to use in development only here.
            }

            // Add the app to the container.
            var app = builder.Build();

            _applicationContextService.Logger = app.Logger;

            // Map the endpoints.
            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetService<PeasieEndpointHandler>()?.RegisterAPIs(app, symmetricKey, signingCertificateKey, encryptingCertificateKey);
            }

            // Configure the HTTP request pipeline.
            app.UseResponseCompression();
            app.UseRequestDecompression();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.MapHealthChecks("/Health");
            app.UseHangfireDashboard("/Hangfire/Dashboard", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            app.UseIPWhitelist();
            app.UseRateLimiter();

            if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                // Error Handler.
                app.UseExceptionHandler(new ExceptionHandlerOptions
                {
                    AllowStatusCode404Response = true,
                    ExceptionHandler = async (HttpContext context) =>
                    {
                        var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                        await context.Response.WriteAsJsonAsync(new { error = error?.Message ?? "Bad Request", statusCode = StatusCodes.Status400BadRequest });
                    }
                });
            }

            var recurringJobManager = new RecurringJobManager();
            recurringJobManager.AddOrUpdate("EveryMinute", Job.FromExpression(() => EveryMinute()), Cron.Minutely());

            // Run the app.
            app.Run();
        }

        public static IResult EveryMinute()
        {
            return Results.Ok(null);
        }
    }
}