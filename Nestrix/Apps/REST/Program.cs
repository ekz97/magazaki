using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.RateLimiting;
using DataLayer.Repositories;
using LogicLayer.Interfaces;
using LogicLayer.Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PeasieLib;
using PeasieLib.Authorization;
using PeasieLib.Extensions;
using PeasieLib.Interfaces;
using PeasieLib.Middleware;
using RESTLayer.Handlers;
using RESTLayer.Services;
using RESTLayer.Context;
using SecurityLayer;
using Serilog;
using Hangfire;
using Hangfire.Common;
using Flurl.Http;
using Peasie.Contracts;
using System.Text.Json;
using PeasieLib.Services;
using BankingRestAPI.Authorization;
using Hangfire.MemoryStorage;

namespace RESTLayer
{

    public class Program
    {
        private static PeasieApplicationContextService? ApplicationContextService;

        public static void Main(string[] args)
        {
            ApplicationContextService = new();

            // Create the app builder.
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            // Add configurations to the container.
            // ------------------------------------
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            // Add loggers to the container.
            // -----------------------------
            builder.Logging.AddJsonConsole();

            builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Debug()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Version", "1.0.0")
                .ReadFrom.Configuration(ctx.Configuration));

            builder.Services.AddHostedService<StartupHostedService>();
            builder.Services.AddSingleton<GebruikerManager>();
            builder.Services.AddSingleton<BankManager>();
            builder.Services.AddSingleton<TransactieManager>();
            builder.Services.AddSingleton<RekeningManager>();
            builder.Services.AddSingleton<AdresManager>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<JwtAuthenticationService>();

            #region health checks

            // Add health check services.
            // --------------------------
            builder.Services.AddHealthChecks();

            #endregion

            #region rate limiting

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
                            $"Too many requests. Please try again after {retryAfter.TotalMinutes} minute(s)." /*+ $"Read more about our rate limits at https://example.org/docs/ratelimiting."*/,
                            cancellationToken: token);
                    }
                    else
                    {
                        await context.HttpContext.Response.WriteAsync(
                            "Too many requests. Please try again later." /* + "Read more about our rate limits at https://example.org/docs/ratelimiting."*/,
                            cancellationToken: token);
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

            #endregion

            #region IP white listing

            // Add white listing to the container.
            // -----------------------------------
            builder.Services.Configure<IPWhitelistOptions>(builder.Configuration.GetSection("IPWhitelistOptions"));

            #endregion

            // Parameters.
            // -----------
            var connectionString = builder.Configuration.GetConnectionString("PeasieAPIDB") ?? "";

            builder.Services.AddSingleton<IGebruikerRepository>(r => new GebruikerRepository(connectionString));
            builder.Services.AddSingleton<IBankRepository>(r => new BankRepository(connectionString));
            builder.Services.AddSingleton<IRekeningRepository>(r => new RekeningRepository(connectionString));
            builder.Services.AddSingleton<ITransactieRepository>(r => new TransactieRepository(connectionString));
            builder.Services.AddSingleton<IAdresRepository>(r => new AdresRepository(connectionString));

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!));

            ApplicationContextService.Issuer = builder.Configuration["Jwt:Issuer"]!;
            ApplicationContextService.Audience = builder.Configuration["Jwt:Audience"]!;
            ApplicationContextService.WebHook = builder.Configuration["WebHook"]!;
            ApplicationContextService.PeasieUrl = builder.Configuration["PeasieUrl"]!;
            ApplicationContextService.DemoMode = bool.Parse(builder.Configuration["DemoMode"]!);

            var signingCertificate =
                new CertificateRequest("cn=foobar", RSA.Create(), HashAlgorithmName.SHA512, RSASignaturePadding.Pss)
                    .CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1));
            var encryptingCertificate =
                new CertificateRequest("cn=foobar", RSA.Create(), HashAlgorithmName.SHA512, RSASignaturePadding.Pss)
                    .CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1));
            var signingCertificateKey = new X509SecurityKey(signingCertificate);
            var encryptingCertificateKey = new X509SecurityKey(encryptingCertificate);
            var signingKeys = new List<SecurityKey> { symmetricKey, signingCertificateKey };

            // Add API services to the container.
            // ----------------------------------
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

                // Add a custom document filter
                options.DocumentFilter<CustomSwaggerFilter>();
            });

            // Add AuthZ and AuthN services.
            // -----------------------------
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
                        new EncryptingCredentials(symmetricKey, JwtConstants.DirectKeyUseAlg,
                            SecurityAlgorithms.Aes256CbcHmacSha512).Key,
                        new EncryptingCredentials(encryptingCertificateKey, SecurityAlgorithms.RsaOAEP,
                            SecurityAlgorithms.Aes256CbcHmacSha512).Key
                    },
                    IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) => signingKeys
                };
            });
            builder.Services.AddAuthorization(options =>
                options.AddPolicy("IsAuthorized", policy => policy.Requirements.Add(new AuthorizationRequirement())));

            // Add DB services.
            // ----------------
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
            builder.Services.AddDbContext<FinancialInstituteApiDbContext>(options =>
                options.UseMySql(connectionString, serverVersion)
                    .UseLoggerFactory(LoggerFactory.Create(b => b.AddFilter(level => level >= LogLevel.Information)))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Add performance booster services.
            // ---------------------------------
            builder.Services.AddResponseCompression();
            builder.Services.AddRequestDecompression();

            // Add custom services to the container.
            // -------------------------------------
            builder.Services.AddSingleton<IPeasieApplicationContextService>(ApplicationContextService);

            // Next service must be constructed ...
            var paymentTransactionService = new PaymentTransactionService(ApplicationContextService);
            builder.Services.AddSingleton<Interfaces.IPaymentTransactionService>(paymentTransactionService);

            builder.Services.AddScoped<IAuthorizationHandler, FinancialInstituteAuthorizationHandler>();
            builder.Services.AddScoped<FinancialInstituteEndpointHandler>();

            // Add third-parties services to the container.
            // --------------------------------------------
            builder.Services.AddHangfire(configuration => configuration.UseMemoryStorage()).AddHangfireServer();
            JobStorage.Current = new MemoryStorage();

            if (builder.Environment.IsDevelopment())
            {
                // Move services you want to use in development only here.
            }

            // Add the app to the container.
            // -----------------------------
            var app = builder.Build();

            ApplicationContextService.Logger = app.Logger;

            // Map the endpoints.
            // ------------------
            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetService<FinancialInstituteEndpointHandler>()?.RegisterAPIs(app, symmetricKey,
                    signingCertificateKey, encryptingCertificateKey);
            }

            // Configure the HTTP request pipeline.
            // ------------------------------------
            app.UseResponseCompression();
            app.UseRequestDecompression();
            //app.UseHsts();
            //app.UseHttpsRedirection();
            //app.Urls.Add("http://localhost:5296");
            app.MapControllers();

            app.MapHealthChecks("/Health");
            app.UseRateLimiter();
            app.UseIPWhitelist();

            app.UseHangfireDashboard("/Hangfire/Dashboard", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
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
                        await context.Response.WriteAsJsonAsync(new
                            { error = error?.Message ?? "Bad Request", statusCode = StatusCodes.Status400BadRequest });
                    }
                });
            }

            var recurringJobManager = new RecurringJobManager();
            recurringJobManager.AddOrUpdate("EveryMinute", Job.FromExpression(() => EveryMinute()), Cron.Minutely());

            // Run the app.
            // ------------
            app.Run();
        }

        public static IResult EveryMinute()
        {
            var ok = true;
            if (ApplicationContextService?.AuthenticationToken == null || ApplicationContextService.Session == null)
                ok = false;
            else
            {
                var sessionVerificationDTO = new SessionVerificationRequestDTO();
                var json = JsonSerializer.Serialize<SessionVerificationRequestDTO>(sessionVerificationDTO);
                var encrypted = EncryptionService.EncryptUsingPublicKey(json, ApplicationContextService?.Session?.SessionResponse?.PublicKey);
                var peasieRequestDTO = new PeasieRequestDTO { Id = ApplicationContextService.Session.SessionResponse.SessionGuid.ToString(), Payload = encrypted };
                var url = ApplicationContextService.PeasieUrl + "/session/assert";
                try
                {
                    var reference = url.WithOAuthBearerToken(ApplicationContextService.AuthenticationToken).PostJsonAsync(peasieRequestDTO).Result;
                    if (reference.ResponseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        ok = false;
                    }
                }
                catch (Exception ex)
                {
                    ApplicationContextService.AuthenticationToken = null;
                    ApplicationContextService.Session = null;
                    ok = false;
                }
            }
            if (!ok)
            {
                ApplicationContextService?.Logger?.LogDebug("FinancialInstituteAPI::EveryMinute requesting token and session");
                // request authentication token
                var validToken = ApplicationContextService?.GetAuthenticationToken();
                if (validToken != null && validToken == true)
                {
                    // request session
                    ApplicationContextService?.GetSession(new UserDTO() { Email = "glenn.colombie@student.hogent.be", Secret = "Nestrix123", Type = "BANK", Designation = "KBC" });
                }
            }
            return Results.Ok(null);
        }
    }
}