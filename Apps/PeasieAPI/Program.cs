global using Hangfire;
global using Hangfire.Dashboard;
global using Hangfire.MemoryStorage;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using PeasieLib.Filters;
global using PeasieLib.Handlers;
global using System.Text.Json;
using Hangfire.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using PeasieLib;
using PeasieLib.Interfaces;
using PeasieLib.Middleware;
using PeasieLib.Models.DB;
using Serilog;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.RateLimiting;
using PeasieLib.Extensions;
using System.IdentityModel.Tokens.Jwt;
using PeasieAPI.Services.Interfaces;
using PeasieAPI.Services;
using Microsoft.AspNetCore.HttpOverrides;
//using LettuceEncrypt;

// TODO
// Header protection
// https://flerka.github.io/personal-blog/2022-06-21-ahead-of-time-compilation/#lets-try-native-aot-in-console AOT!!


namespace PeasieAPI
{
    public class Program
    {
        private static PeasieApplicationContextService? ApplicationContextService;
        private static DataManagerService? DataManagerService;

        public static void Main(string[] args)
        {
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

            /*
            // Terminating TLS at a reverse proxy part 1
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            */

            builder.Services.Configure<IPWhitelistOptions>(builder.Configuration.GetSection("IPWhitelistOptions"));

            ApplicationContextService = new();
            DataManagerService = new();

            // Parameters:
            var connectionString = builder.Configuration.GetConnectionString("PeasieAPIDB") ?? "";
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!));
            ApplicationContextService.Audience = builder.Configuration["Jwt:Audience"]!;
            ApplicationContextService.Issuer = builder.Configuration["Jwt:Issuer"]!;
            ApplicationContextService.FinancialInstituteUrl = builder.Configuration["FinancialInstituteUrl"]!;
            ApplicationContextService.Lifetime = new TimeSpan(0, 0, 30); // 30 seconds
            var signingCertificate = new CertificateRequest("cn=foobar", RSA.Create(), HashAlgorithmName.SHA512, RSASignaturePadding.Pss).CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1));
            var encryptingCertificate = new CertificateRequest("cn=foobar", RSA.Create(), HashAlgorithmName.SHA512, RSASignaturePadding.Pss).CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1));
            var signingCertificateKey = new X509SecurityKey(signingCertificate);
            var encryptingCertificateKey = new X509SecurityKey(encryptingCertificate);
            var signingKeys = new List<SecurityKey> { symmetricKey, signingCertificateKey };

            IdentityModelEventSource.ShowPII = true; // NOT GDPR safe!

            // Add API services to the container.
            //ILettuceEncryptServiceBuilder encryptBuilder = builder.Services.AddLettuceEncrypt();
            //encryptBuilder.PersistDataToDirectory(new DirectoryInfo("/tmp/LettuceEncrypt/"), "Password123");
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
                    ValidIssuer = ApplicationContextService.Issuer,
                    ValidAudience = ApplicationContextService.Audience,
                    IssuerSigningKeys = signingKeys,
                    TokenDecryptionKeys = new List<SecurityKey>
                    {
                        new EncryptingCredentials(symmetricKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512).Key,
                        new EncryptingCredentials(encryptingCertificateKey, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512).Key
                    },
                    IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) => signingKeys
                };
            });
            builder.Services.AddAuthorization(options => options.AddPolicy("IsAuthorized", policy => policy.Requirements.Add(new PeasieAuthorizationRequirement(ApplicationContextService, DataManagerService))));

            // Add DB services.
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
            // Add DB services.
            builder.Services.AddDbContext<PeasieAPIDbContext>(options =>
                    options.UseMySql(connectionString, serverVersion)
                    .UseLoggerFactory(LoggerFactory.Create(b => b.AddFilter(level => level >= LogLevel.Debug)))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                );

            var dataProtectionConnectionString = builder.Configuration.GetConnectionString("DataProtectionConnection") ?? throw new InvalidOperationException("Connection string 'DataProtectionConnection' not found.");
            builder.Services.AddCustomDataProtection(dataProtectionConnectionString);

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Add performance booster services.
            builder.Services.AddResponseCompression();
            builder.Services.AddRequestDecompression();

            // Is no longer added by default:
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add custom services to the container.
            builder.Services.AddSingleton<IPeasieApplicationContextService>(ApplicationContextService);
            builder.Services.AddSingleton<IDataManagerService>(DataManagerService);
            builder.Services.AddScoped<IAuthorizationHandler, PeasieAuthorizationHandler>();
            builder.Services.AddScoped<PeasieEndpointHandler>();

            // Add third-parties services to the container.
            builder.Services.AddHangfire(configuration => configuration.UseMemoryStorage()).AddHangfireServer();
            JobStorage.Current = new MemoryStorage();

            // Add the app to the container.
            var app = builder.Build();

            if (builder.Environment.IsDevelopment())
            {
                app.Logger.LogDebug("Development environment detected.");
            }
            else if(builder.Environment.IsStaging())
            {
                app.Logger.LogDebug("Staging environment detected.");
            }
            else
            {
                app.Logger.LogDebug("Not development or staging.");
            }

            ApplicationContextService.Logger = app.Logger;
            DataManagerService.Logger = app.Logger;

            // Map the endpoints.
            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetService<PeasieEndpointHandler>()?.RegisterAPIs(app, symmetricKey, signingCertificateKey, encryptingCertificateKey);
            }

            /*
            // forwarded headers from a reverse proxy, part 2:
            app.UseForwardedHeaders();
            */

            // Configure the HTTP request pipeline.
            app.UseResponseCompression();
            app.UseRequestDecompression();

            //app.UseHsts();
            //app.UseHttpsRedirection();

            app.MapHealthChecks("/Health");
            app.UseHangfireDashboard("/Hangfire/Dashboard", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            //app.UseHttpChallengeResponseMiddleware();
            app.UseIPWhitelist();
            app.UseRateLimiter();

            app.Use((context, next) => {
                context.Request.EnableBuffering(1000000);
                return next();
            });

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
            DataManagerService?.Recycle();
            return Results.Ok(null);
        }
    }
}