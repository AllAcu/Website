using System.Data.Entity;
using Microsoft.Its.Domain.Sql;

namespace AllAcu
{
    public class AllAcuSiteDbContext : ReadModelDbContext
    {
        public static string ConnectionString;

        public DbSet<CareProviderDetails> CareProviders { get; set; }
        public DbSet<PatientListItemViewModel> PatientList { get; set; }
        public DbSet<PatientDetails> PatientDetails { get; set; }
        public DbSet<PendingInsuranceVerificationListItemViewModel> VerificationList { get; set; }
        public DbSet<InsuranceVerification> Verifications { get; set; }
        public DbSet<UserListItemViewModel> UserList { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<OutstandingConfirmation> Confirmations { get; set; }

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

            modelBuilder.Entity<OutstandingConfirmation>()
                .HasKey(i => i.RegistrationId);

            modelBuilder.Entity<OutstandingConfirmation.Invite>()
                .HasRequired(i => i.Provider);

            modelBuilder.Entity<CareProviderDetails>()
                .HasKey(p => p.Id)
                .ToTable("CareProviders");

            modelBuilder.Entity<UserDetails>()
                .HasKey(u => u.UserId)
                .HasMany(u => u.Providers)
                .WithMany();

            modelBuilder.ComplexType<InsuranceVerification.PatientInfo>();
            modelBuilder.ComplexType<Benefits>();
            modelBuilder.ComplexType<PatientDetails.LatestVerification>();

            modelBuilder.Entity<PatientDetails>()
                .HasOptional(d => d.MedicalInsurance);
            modelBuilder.Entity<PatientDetails>()
                .HasOptional(d => d.PersonalInjuryProtection);

            base.OnModelCreating(modelBuilder);
        }
    }
}
