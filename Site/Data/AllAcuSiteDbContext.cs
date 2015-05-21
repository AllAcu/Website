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
        public DbSet<PatientDetails> PatientDetails { get; set; }
        public DbSet<PendingInsuranceVerificationListItemViewModel> VerificationList { get; set; }
        public DbSet<PendingVerificationRequest> VerificationRequestDrafts { get; set; }
        //public DbSet<object> VerificationRequests { get; set; }
        //public DbSet<object> ApprovedVerifications { get; set; }

        public AllAcuSiteDbContext()
            : base(ConnectionString ?? NameOrConnectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientListItemViewModel>()
                .HasKey(i => i.PatientId);
            modelBuilder.Entity<PatientDetails>()
                .HasKey(i => i.PatientId);
            modelBuilder.Entity<PendingInsuranceVerificationListItemViewModel>()
                .HasKey(i => i.VerificationId);
            modelBuilder.Entity<PendingVerificationRequest>()
                .HasKey(i => i.VerificationId);

            modelBuilder.ComplexType<PatientDetails.Verification>();

            modelBuilder.Entity<PatientDetails>()
                .HasOptional(d => d.MedicalInsurance);
            modelBuilder.Entity<PatientDetails>()
                .HasOptional(d => d.PersonalInjuryProtection);

            modelBuilder.Entity<CareProviderBusinessInfo>()
                .ToTable("CareProviders");

            base.OnModelCreating(modelBuilder);
        }
    }
}
