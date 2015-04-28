using System.Data.Entity;
using AllAcu.Models.Providers;
using Domain.Repository;
using Microsoft.Its.Domain.Sql;

namespace AllAcu
{
    public class AllAcuSiteDbContext : ReadModelDbContext
    {
        public static string ConnectionString;

        public DbSet<CareProviderBusinessInfo> CareProviders { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientListItemViewModel> PatientList { get; set; }

        public AllAcuSiteDbContext()
            : base(ConnectionString ?? NameOrConnectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasKey(i => i.PatientId);
            modelBuilder.Entity<PatientListItemViewModel>()
                .HasKey(i => i.PatientId);

            modelBuilder.ComplexType<PatientPersonalInformation>();
            modelBuilder.ComplexType<PatientContactInformation>();

            modelBuilder.Entity<CareProviderBusinessInfo>()
                .ToTable("CareProviders");

            base.OnModelCreating(modelBuilder);
        }
    }
}
