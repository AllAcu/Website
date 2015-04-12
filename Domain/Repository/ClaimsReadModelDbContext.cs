using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using Microsoft.Its.Domain.Sql;

namespace Domain.Repository
{
    public class ClaimsReadModelDbContext : ReadModelDbContext
    {
        public ClaimsReadModelDbContext()
            : base(NameOrConnectionString)
        {

        }

        public DbSet<ClaimDraft> Drafts { get; set; }
        public DbSet<ClaimSubmissionRequest> SubmittedClaims { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClaimDraft>()
                .HasKey(x => new { x.Id });

            modelBuilder.Entity<ClaimDraft>()
                .Property(x => x.Visit.Diagnosis).HasColumnName("Diagnosis");
            modelBuilder.Entity<ClaimDraft>()
                .Property(x => x.Visit.DateOfService).HasColumnName("DateOfService");
            modelBuilder.Entity<ClaimDraft>()
                .Property(x => x.Patient.Name).HasColumnName("PatientName");

            modelBuilder.Entity<ClaimDraft>()
                .Ignore(x => x.Provider);

            modelBuilder.ComplexType<Visit>()
                .Ignore(x => x.Procedures);

            modelBuilder.Entity<ClaimSubmissionRequest>()
                .HasKey(x => new { x.Id });

            base.OnModelCreating(modelBuilder);
        }
    }
}
