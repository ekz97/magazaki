﻿using Microsoft.AspNetCore.Authorization;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.Common;
using Flurl.Http;
using Peasie.Contracts;
using PeasieLib.Services;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PeasieLib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PeasieLib.Authorization;
using PeasieLib.Interfaces;
using PeasieLib.Middleware;
using Serilog;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.RateLimiting;
using PeasieLib.Extensions;
using System.IdentityModel.Tokens.Jwt;
using WebshopApi.Infrastructure.Handlers;
using WebshopApi.Infrastructure.Authorization;
using WebshopApi.Infrastructure.Data;
using WebshopApi.Domain.Interfaces;
using WebshopApi.Domain.IServices;
using WebshopApi.Domain.Services;
using WebshopApi.Infrastructure.Repositories;
using WebshopApi.REST.Mapping;
using Microsoft.AspNetCore.Identity;

namespace WebshopApi.Presentation
{
    public class Program
    {
        private static PeasieApplicationContextService? ApplicationContextService;

        public static void Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            ApplicationContextService = new();

            // Create the app builder.
            var builder = WebApplication.CreateBuilder(args);

            // Add configurations to the container.
            // ------------------------------------
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
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

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                                  });
            });

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

            // Add white listing to the container.
            // -----------------------------------
            builder.Services.Configure<IPWhitelistOptions>(builder.Configuration.GetSection("IPWhitelistOptions"));

            // Parameters.
            // -----------
            var connectionString = builder.Configuration.GetConnectionString("PeasieAPIDB") ?? "";
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!));

            ApplicationContextService.Issuer = builder.Configuration["Jwt:Issuer"]!;
            ApplicationContextService.Audience = builder.Configuration["Jwt:Audience"]!;
            ApplicationContextService.WebHook = builder.Configuration["WebHook"]!;
            ApplicationContextService.PeasieUrl = builder.Configuration["PeasieUrl"]!;
            ApplicationContextService.DemoMode = bool.Parse(builder.Configuration["DemoMode"]!);
            ApplicationContextService.PeasieClientId = builder.Configuration["ClientId"]!;
            ApplicationContextService.PeasieClientSecret = builder.Configuration["ClientSecret"]!;

            var signingCertificate = new CertificateRequest("cn=foobar", RSA.Create(), HashAlgorithmName.SHA512, RSASignaturePadding.Pss).CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1));
            var encryptingCertificate = new CertificateRequest("cn=foobar", RSA.Create(), HashAlgorithmName.SHA512, RSASignaturePadding.Pss).CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1));
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

            // Add health check services.
            // --------------------------
            builder.Services.AddHealthChecks();

            // https://andrewlock.net/a-look-behind-the-jwt-bearer-authentication-middleware-in-asp-net-core/
            // https://auth0.com/docs/quickstart/backend/aspnet-core-webapi/03-troubleshooting
            // LVET TODO: Bearer was not authenticated. Failure message: No SecurityTokenValidator available for token.

            // Add AuthZ and AuthN services.
            // -----------------------------
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
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

            // Add DB services.
            // ----------------
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
            builder.Services.AddDbContext<MyDbContext>(options =>
                    options.UseMySql(connectionString, serverVersion)
                    .UseLoggerFactory(LoggerFactory.Create(b => b.AddFilter(level => level >= LogLevel.Debug)))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                );
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Add performance booster services.
            // ---------------------------------
            builder.Services.AddResponseCompression();
            builder.Services.AddRequestDecompression();

            // Add Controller support - difference minimal apis
            // ----------------------
            builder.Services.AddControllers();

            // Add custom services to the container.
            // -------------------------------------
            builder.Services.AddHostedService<StartupHostedService>();
            builder.Services.AddSingleton<IPeasieApplicationContextService>(ApplicationContextService);
            builder.Services.AddScoped<IAuthorizationHandler, BeneficiaryAuthorizationHandler>();
            builder.Services.AddScoped<BeneficiaryEndpointHandler>();

            builder.Services.AddAuthorization(options => options.AddPolicy("IsAuthorized", policy => policy.Requirements.Add(new AuthorizationRequirement())));

            // Add third-parties services to the container.
            // --------------------------------------------
            builder.Services.AddHangfire(configuration => configuration.UseMemoryStorage()).AddHangfireServer();
            //JobStorage.Current = new MemoryStorage();

            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddScoped<IPasswordHasher<Domain.Models.Customer>, PasswordHasher<Domain.Models.Customer>>();
            builder.Services.AddScoped<ICategoryRepository, EfCategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICustomerRepository, EfCustomerRepository>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IProductRepository, EfProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderLineRepository, EfOrderLineRepository>();
            builder.Services.AddScoped<IOrderLineService, OrderLineService>();
            builder.Services.AddScoped<IPriceTypeRepository, EfPriceTypeRepository>();
            builder.Services.AddScoped<IPriceTypeService, PriceTypeService>();
            builder.Services.AddScoped<IStoreRepository, EfStoreRepository>();
            builder.Services.AddScoped<IStoreService, StoreService>();

            if (builder.Environment.IsDevelopment())
            {
                // Move services you want to use in development only here.
            }

            // Add the app to the container.
            // -----------------------------
            var app = builder.Build();

            ApplicationContextService.Logger = app.Logger;
            ApplicationContextService.Configuration = app.Configuration;

            // Map the endpoints.
            // ------------------
            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetService<BeneficiaryEndpointHandler>()?.RegisterAPIs(app, symmetricKey, signingCertificateKey, encryptingCertificateKey);
            }

            // Configure the HTTP request pipeline.
            // ------------------------------------
            app.UseResponseCompression();
            app.UseRequestDecompression();
            //app.UseHsts();
            //app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);

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
                        await context.Response.WriteAsJsonAsync(new { error = error?.Message ?? "Bad Request", statusCode = StatusCodes.Status400BadRequest });
                    }
                });
            }

            var recurringJobManager = new RecurringJobManager();
            recurringJobManager.AddOrUpdate("EveryMinute", Job.FromExpression(() => EveryMinute()), Cron.Minutely());

            // Activate Controllers - difference minimal aps
            app.MapControllers();

            // Run the app.
            // ------------
            app.Run();
        }

        public static IResult EveryMinute()
        {
            ApplicationContextService?.Logger?.LogDebug("-> WebshopApi.Presentation::EveryMinute");
            var ok = true;
            if (ApplicationContextService?.AuthenticationToken == null || ApplicationContextService.Session == null)
            {
                ApplicationContextService?.Logger?.LogDebug("Authentication token or session reference not found");
                ok = false;
            }
            else
            {
                var sessionVerificationDTO = new SessionVerificationRequestDTO();
                var json = JsonSerializer.Serialize<SessionVerificationRequestDTO>(sessionVerificationDTO);
                var encrypted = EncryptionService.EncryptUsingPublicKey(json, ApplicationContextService?.Session?.SessionResponse?.PublicKey);
                var peasieRequestDTO = new PeasieRequestDTO { Id = ApplicationContextService.Session.SessionResponse.SessionGuid.ToString(), Payload = encrypted };
                var url = ApplicationContextService.PeasieUrl + "/session/assert";
                try
                {
                    ApplicationContextService?.Logger?.LogDebug("Verifying session...");
                    var reference = url.WithOAuthBearerToken(ApplicationContextService.AuthenticationToken).PostJsonAsync(peasieRequestDTO).Result;
                    if (reference.ResponseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        ApplicationContextService?.Logger?.LogDebug("Could not verify session");
                        ok = false;
                    }
                }
                catch (Exception ex)
                {
                    if (ApplicationContextService != null)
                    {
                        ApplicationContextService.Logger?.LogError($"Resetting authentication token and session reference: {ex.Message}");
                        ApplicationContextService.AuthenticationToken = null;
                        ApplicationContextService.Session = null;
                    }
                    ok = false;
                }
            }
            if (!ok)
            {
                ApplicationContextService?.Logger?.LogDebug("Requesting new authentication token...");
                ApplicationContextService?.GetAuthenticationToken();
                ApplicationContextService?.Logger?.LogDebug("Requesting new session...");
                bool? sessionOk = ApplicationContextService?.GetSession(new UserDTO() { Email = "glenn.colombie@student.hogent.be", Secret = "Nestrix123", Type = "SHOP", Designation = "Colruyt" });
                ok = (ApplicationContextService?.AuthenticationToken != null && ApplicationContextService.Session != null);
            }
            if (ok && ApplicationContextService?.DemoMode == true)
            {
                ApplicationContextService?.Logger?.LogInformation("Sending payment request (demo mode is active)");
                try
                {
                    BeneficiaryEndpointHandler.MakePaymentRequest(ApplicationContextService, new PaymentTrxDTO() { IBAN = "BE68539007547999", Amount = 50, Currency = "EUR", Comment = $"Ticket {Guid.NewGuid().ToString()}" });
                }
                catch(Exception e)
                {
                    ApplicationContextService?.Logger?.LogError(e.Message);
                }
            }
            ApplicationContextService?.Logger?.LogDebug("<- WebshopApi.Presentation::EveryMinute");
            return Results.Ok(null);
        }
    }
}