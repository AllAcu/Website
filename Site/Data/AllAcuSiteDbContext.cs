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
        public DbSet<PatientListItemViewModel> PatientList { get; set; }
        public DbSet<PatientEditViewModel> PatientEditViewModels { get; set; }
        public DbSet<PatientDetailsViewModel> PatientDetailsViewModels { get; set; }

        public AllAcuSiteDbContext()
            : base(ConnectionString ?? NameOrConnectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientListItemViewModel>()
                .HasKey(i => i.PatientId);
            modelBuilder.Entity<PatientEditViewModel>()
                .HasKey(i => i.PatientId);
            modelBuilder.Entity<PatientDetailsViewModel>()
                .HasKey(i => i.PatientId);

            modelBuilder.Entity<CareProviderBusinessInfo>()
                .ToTable("CareProviders");

            base.OnModelCreating(modelBuilder);
        }
    }
}
