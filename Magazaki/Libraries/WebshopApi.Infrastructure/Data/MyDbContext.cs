using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebshopApi.Infrastructure.DTO;

namespace WebshopApi.Infrastructure.Data
{
    public class MyDbContext : DbContext
    {
        //private readonly IConfiguration _configuration;

        private readonly ILogger<MyDbContext>? _logger;
        public DbSet<CategoryDbDTO> Categories { get; set; }
        public DbSet<CustomerDbDTO> Customers { get; set; }
        public DbSet<OrderDbDTO> Orders { get; set; }
        public DbSet<OrderLineDbDTO> OrderLines { get; set; }
        public DbSet<PriceTypeDbDTO> PriceTypes { get; set; }
        public DbSet<ProductDbDTO> Products { get; set; }
        public DbSet<StoreDbDTO> Stores { get; set; }


        public MyDbContext()
        {
            
        }



        public MyDbContext(DbContextOptions<MyDbContext> options, ILogger<MyDbContext> logger) : base(options)
        {

            _logger = logger;
            //// To ensure that database is created through dbcontext
            //Database.EnsureCreated();
        }

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

                // _ = optionsBuilder.UseMySql(connstring);
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
                _ = optionsBuilder.UseMySql(connstring, serverVersion);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
        }

      



    }
}
