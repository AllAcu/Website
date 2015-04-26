using System.Data.Entity;
using Domain.CareProvider;
using Domain.ClaimFiling;
using Microsoft.Its.Domain.Sql;

namespace Domain.Repository
{
    public class CareProviderReadModelDbContext : ReadModelDbContext
    {
        public static string ConnectionString;

        public CareProviderReadModelDbContext()
            : base(ConnectionString ?? NameOrConnectionString)
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

            modelBuilder.ComplexType<InsurancePolicy>();
            modelBuilder.ComplexType<Address>();
            modelBuilder.ComplexType<Visit>()
                .Ignore(x => x.Procedures);

            base.OnModelCreating(modelBuilder);
        }
    }
}
