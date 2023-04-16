using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace Peasie.Web
{

    public static class DataProtectionExtensions
    {
        public static IServiceCollection AddCustomDataProtection(this IServiceCollection serviceCollection, string connectionString)
        {
            var dataProtectionBuilder = serviceCollection
                .AddDataProtection()
                .SetApplicationName("PeasieSec");
#if LOCAL
        dataProtectionBuilder
            .PersistKeysToFileSystem(new DirectoryInfo(@"c:\dataprotection-persistkeys"))
            .AddKeyManagementOptions(options =>
            {
                options.NewKeyLifetime = new TimeSpan(365, 0, 0, 0);
                options.AutoGenerateKeys = true;
            });
#else
            serviceCollection.AddDbContext<DataProtectionKeyContext>(o =>
                    {
                        //o.UseInMemoryDatabase("DataProtection_EntityFrameworkCore");
                        // Make sure to create a sql server called DataProtectionApp
                        var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
                        o.UseMySql(connectionString, serverVersion);
                        //o.UseSqlServer(connectionString);
                        o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                        o.EnableSensitiveDataLogging();
                    })
                    .AddDataProtection()
                    .PersistKeysToDbContext<DataProtectionKeyContext>()
                    .SetDefaultKeyLifetime(TimeSpan.FromDays(7))
                    .Services
                    .BuildServiceProvider(validateScopes: true);
#endif
            return serviceCollection;
        }

    }
}