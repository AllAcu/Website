using System.Data.Entity;
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
        public DbSet<InsuranceVerification> Verifications { get; set; }
        public DbSet<UserListItemViewModel> UserList { get; set; }
        public DbSet<UserDetailsViewModel> UserDetails { get; set; }

        public AllAcuSiteDbContext()
            : base(ConnectionString ?? NameOrConnectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserListItemViewModel>()
                .HasKey(i => i.UserId);
            modelBuilder.Entity<PatientListItemViewModel>()
                .HasKey(i => i.PatientId);
            modelBuilder.Entity<PatientDetails>()
                .HasKey(i => i.PatientId);
            modelBuilder.Entity<PendingInsuranceVerificationListItemViewModel>()
                .HasKey(i => i.VerificationId);
            modelBuilder.Entity<InsuranceVerification>()
                .HasKey(i => i.VerificationId);
            modelBuilder.Entity<CareProviderBusinessInfo>()
                .HasKey(p => p.Id)
                .ToTable("CareProviders");
            modelBuilder.Entity<UserDetailsViewModel>()
                .HasKey(u => u.UserId);

            modelBuilder.ComplexType<InsuranceVerification.PatientInfo>();
            modelBuilder.ComplexType<Benefits>();
            modelBuilder.ComplexType<PatientDetails.LatestVerification>();
            modelBuilder.ComplexType<UserDetailsViewModel.ProviderIdList>()
                .Property(p => p.Serialized)
                .HasColumnName("Providers");

            modelBuilder.Entity<PatientDetails>()
                .HasOptional(d => d.MedicalInsurance);
            modelBuilder.Entity<PatientDetails>()
                .HasOptional(d => d.PersonalInjuryProtection);

            base.OnModelCreating(modelBuilder);
        }
    }
}
