﻿using Microsoft.EntityFrameworkCore;

namespace RESTLayer.Context
{

    public partial class FinancialInstituteApiDbContext : DbContext
    {
        private readonly ILogger<FinancialInstituteApiDbContext>? _logger;

        public FinancialInstituteApiDbContext()
        {
        }

        public FinancialInstituteApiDbContext(DbContextOptions<FinancialInstituteApiDbContext> options, ILogger<FinancialInstituteApiDbContext> logger)
            : base(options)
        {
            _logger = logger;
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
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}