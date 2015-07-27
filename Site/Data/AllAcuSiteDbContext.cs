﻿using System.Data.Entity;
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
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<ProviderRole> ProviderRoles { get; set; }

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
                .HasKey(i => i.UserId);

            modelBuilder.Entity<UserDetails>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<CareProviderDetails>()
                .HasKey(p => p.Id)
                .ToTable("CareProviders");

            modelBuilder.ComplexType<RoleList>();

            modelBuilder.Entity<Invitation>()
                .HasKey(i => i.InviteId)
                .Property(i => i.Roles.Serialized)
                .HasColumnName("Roles");

            modelBuilder.Entity<UserDetails>()
                .HasMany(u => u.OutstandingInvites)
                .WithRequired(i => i.User);

            modelBuilder.Entity<Invitation>()
                .HasRequired(i => i.Provider);

            modelBuilder.Entity<ProviderRole>()
                .HasKey(r => r.Id)
                .Property(r => r.Roles.Serialized)
                .HasColumnName("Roles");

            modelBuilder.Entity<ProviderRole>()
                .HasRequired(r => r.User)
                .WithMany(u => u.ProviderRoles);

            modelBuilder.Entity<ProviderRole>()
                .HasRequired(r => r.Provider)
                .WithMany(p => p.Practitioners);

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
