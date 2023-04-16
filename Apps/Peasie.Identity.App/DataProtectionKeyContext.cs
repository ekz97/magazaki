using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
// TODO:
// One time public key as  voor email => key + session request for other apps; private keys for Peasie service

public class DataProtectionKeyContext : DbContext, IDataProtectionKeyContext
{
    public DataProtectionKeyContext(DbContextOptions<DataProtectionKeyContext> options) : base(options) { }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
}
