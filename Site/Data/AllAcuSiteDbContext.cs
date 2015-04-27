using System;
using System.Data.Entity;
using AllAcu.Models.Providers;
using Domain;
using Domain.Repository;
using Microsoft.Its.Domain.Sql;

namespace AllAcu
{
    public class AllAcuSiteDbContext : ReadModelDbContext
    {
        public static string ConnectionString;

        public DbSet<CareProviderBusinessInfo> CareProviders { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public AllAcuSiteDbContext()
            : base(ConnectionString ?? NameOrConnectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasKey(i => i.PatientId);

            //modelBuilder.Entity<Patient>()
            //    .HasRequired(i => i.PersonalInfo);

            modelBuilder.ComplexType<PatientPersonalInformation>();

            modelBuilder.Entity<CareProviderBusinessInfo>()
                .ToTable("CareProviders");

            modelBuilder.Ignore<Address>();

            base.OnModelCreating(modelBuilder);
        }
    }

    public class Patient
    {
        public Guid PatientId { get; set; }
        public Guid ProviderId { get; set; }
        public PatientPersonalInformation PersonalInfo { get; set; }
    }
}
