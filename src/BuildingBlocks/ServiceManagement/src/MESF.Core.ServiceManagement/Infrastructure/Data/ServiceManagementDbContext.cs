using MESF.Core.ServiceManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MESF.Core.ServiceManagement.Infrastructure
{
    public class ServiceManagementDbContext : DbContext
    {
        public DbSet<HostedService> Services { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer("");
        //    //base.OnConfiguring(optionsBuilder);
        //}

        public ServiceManagementDbContext(DbContextOptions<ServiceManagementDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HostedService>(ConfigureHostedService);
        }

        private void ConfigureHostedService(EntityTypeBuilder<HostedService> builder)
        {
            builder.ToTable("Services");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .ForSqlServerUseSequenceHiLo("catalog_hilo")
                .IsRequired();
        }

    }

    

}
