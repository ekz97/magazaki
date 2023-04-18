using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;

namespace PeasieAPI.Models.DB
{
    public class DataProtectionKeyDbContext : DbContext, IDataProtectionKeyContext
    {
        public DataProtectionKeyDbContext(DbContextOptions<DataProtectionKeyDbContext> options) : base(options) { }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }
}