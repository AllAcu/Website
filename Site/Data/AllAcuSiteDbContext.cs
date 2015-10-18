using System.Data.Entity;
using Microsoft.Its.Domain.Sql;

namespace AllAcu
{
    public class AllAcuSiteDbContext : ReadModelDbContext
    {
        public static string ConnectionString;

        public DbSet<Patient> Patients { get; set; }
        public DbSet<InsuranceVerification> Verifications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OutstandingConfirmation> Confirmations { get; set; }
        public DbSet<ProviderInvitation> ProviderInvitations { get; set; }
        public DbSet<BillerInvitation> BillerInvitations { get; set; }
        public DbSet<ProviderRole> ProviderRoles { get; set; }
        public DbSet<BillerRole> BillerRoles { get; set; }
        public DbSet<Biller> Billers { get; set; }
        public DbSet<CareProvider> CareProviders { get; set; }

        public AllAcuSiteDbContext()
            : base(ConnectionString ?? NameOrConnectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasKey(i => i.PatientId);

            modelBuilder.Entity<InsuranceVerification>()
                .HasKey(i => i.VerificationId)
                .HasOptional(i => i.AssignedTo);

            modelBuilder.Entity<InsuranceVerification>()
                .Property(v => v.CallHistory.Serialized)
                .HasColumnName("CallHistory");

            modelBuilder.Entity<InsuranceVerification>()
                .Property(v => v.AssignmentHistory.Serialized)
                .HasColumnName("AssignmentHistory");

            modelBuilder.Entity<OutstandingConfirmation>()
                .HasKey(i => i.UserId);

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<CareProvider>()
                .HasKey(p => p.Id)
                .ToTable("CareProviders");

            modelBuilder.ComplexType<RoleList>();

            modelBuilder.Entity<BillerInvitation>()
                .HasKey(i => i.InviteId)
                .Property(i => i.Roles.Serialized)
                .HasColumnName("Roles");

            modelBuilder.Entity<ProviderInvitation>()
                .HasKey(i => i.InviteId)
                .Property(i => i.Roles.Serialized)
                .HasColumnName("Roles");

            modelBuilder.Entity<User>()
                .HasMany(u => u.ProviderInvitations)
                .WithRequired(i => i.User);

            modelBuilder.Entity<User>()
                .HasMany(u => u.BillerInvitations)
                .WithRequired(i => i.User);

            modelBuilder.Entity<ProviderInvitation>()
                .HasRequired(i => i.Organization);

            modelBuilder.Entity<BillerInvitation>()
                .HasRequired(i => i.Organization);

            modelBuilder.Entity<ProviderRole>()
                .HasKey(r => r.Id)
                .Property(r => r.Roles.Serialized)
                .HasColumnName("Roles");

            modelBuilder.Entity<ProviderRole>()
                .HasRequired(r => r.User)
                .WithMany(u => u.ProviderRoles);

            modelBuilder.Entity<ProviderRole>()
                .HasRequired(r => r.Provider)
                .WithMany(p => p.Users);

            modelBuilder.Entity<Biller>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Biller>()
                .HasMany(p => p.Users)
                .WithRequired(p => p.Biller);

            modelBuilder.Entity<BillerRole>()
                .HasKey(r => r.Id)
                .Property(r => r.Roles.Serialized)
                .HasColumnName("Roles");

            modelBuilder.Entity<BillerRole>()
                .HasRequired(r => r.User)
                .WithMany(r => r.BillerRoles);

            modelBuilder.Entity<BillerRole>()
                .HasRequired(r => r.Biller)
                .WithMany(p => p.Users);

            modelBuilder.ComplexType<InsuranceVerification.PatientInfo>();
            modelBuilder.ComplexType<InsuranceVerification.BillerApproval>();
            modelBuilder.ComplexType<Benefits>();
            modelBuilder.ComplexType<Patient.LatestVerification>();

            modelBuilder.Entity<Patient>()
                .HasOptional(d => d.MedicalInsurance);
            modelBuilder.Entity<Patient>()
                .HasOptional(d => d.PersonalInjuryProtection);

            base.OnModelCreating(modelBuilder);
        }
    }
}
