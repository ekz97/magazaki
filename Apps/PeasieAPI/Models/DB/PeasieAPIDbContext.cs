using Microsoft.EntityFrameworkCore;

namespace PeasieLib.Models.DB;

public partial class PeasieAPIDbContext : DbContext
{
    public PeasieAPIDbContext()
    {
    }

    public PeasieAPIDbContext(DbContextOptions<PeasieAPIDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Nlog> Nlog { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddEnvironmentVariables();

            IConfiguration config = builder.Build();

            var connstring = config.GetConnectionString("PeasieAPIDB")!;

            //_ = optionsBuilder.UseSqlServer(connstring);
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
            _ = optionsBuilder.UseMySql(connstring, serverVersion);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Nlog>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__NLog__3214EC07BD49837C");

            _ = entity.ToTable("NLog");

            _ = entity.Property(e => e.Id).ValueGeneratedNever();
            _ = entity.Property(e => e.Logged).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
