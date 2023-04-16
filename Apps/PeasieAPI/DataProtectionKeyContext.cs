using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;


// TODO
// Header protection
// https://flerka.github.io/personal-blog/2022-06-21-ahead-of-time-compilation/#lets-try-native-aot-in-console AOT!!

namespace PeasieAPI
{
    public class DataProtectionKeyContext : DbContext, IDataProtectionKeyContext
    {
        public DataProtectionKeyContext(DbContextOptions<DataProtectionKeyContext> options) : base(options) { }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }
}