using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Peasie.Web;
using Peasie.Web.Data;
using Peasie.Web.Services;
using PeasieLib.Middleware;
using SendGrid.Extensions.DependencyInjection;
using Peasie.Identity.App.Areas.Identity.Data;
using Serilog;
using System.Threading.RateLimiting;


// To create the database contexts:
// --------------------------------
// Add-Migration InitialCreate -Context ApplicationDbContext -OutputDir Data/Migrations/ApplicationDb
// Update-Database -Context ApplicationDbContext -p Peasie.Identity.App
// Add-Migration AddDataProtectionKeys -Context DataProtectionKeyContext
// Update-Database -Context DataProtectionKeyContext -p Peasie.Identity.App
// dotnet tool install --global dotnet-ef
// dotnet ef migrations add InitialCreate -c ApplicationDbContext -o Data/Migrations/ApplicationDb
// dotnet ef migrations add AddDataProtectionKeys -c DataProtectionKeyContext
// dotnet ef database update -c ApplicationDbContext -o Data/Migrations/ApplicationDb
// dotnet ef database update -c DataProtectionKeyContext
// dotnet dev-certs https --trust
// dotnet dev-certs https --check

internal class Program
{
    // TODO:
    // CORS - headers

    private static void Main(string[] args)
    {
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

        // Add services to the container.

        builder.Services.AddHealthChecks();

        #region rate limiting
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
        #endregion

        #region IP white listing
        builder.Services.Configure<IPWhitelistOptions>(builder.Configuration.GetSection("IPWhitelistOptions"));
        #endregion

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, serverVersion)
            //options.UseSqlServer(connectionString)
            );

        var dataProtectionConnectionString = builder.Configuration.GetConnectionString("DataProtectionConnection") ?? throw new InvalidOperationException("Connection string 'DataProtectionConnection' not found.");
        builder.Services.AddCustomDataProtection(dataProtectionConnectionString);

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<PeasieIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddRazorPages();

        builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGridSettings"));

        builder.Services.AddSendGrid(options =>
        {
            options.ApiKey = builder.Configuration.GetSection("SendGridSettings")
            .GetValue<string>("ApiKey");
        });

        builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("TwilioSettings"));

        builder.Services.AddAuthentication().AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = builder.Configuration.GetSection("GoogleAuthSettings")
        .GetValue<string>("ClientId");
            googleOptions.ClientSecret = builder.Configuration.GetSection("GoogleAuthSettings")
        .GetValue<string>("ClientSecret");
        });

        builder.Services.AddScoped<IEmailSender, EmailSenderService>();
        builder.Services.AddScoped<ISMSSenderService, SMSSenderService>();
        builder.Services.AddScoped<IEncryptionService, EncryptionService>();

        var app = builder.Build();

        app.MapHealthChecks("/Health");

        app.UseRateLimiter();
        app.UseIPWhitelist();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}