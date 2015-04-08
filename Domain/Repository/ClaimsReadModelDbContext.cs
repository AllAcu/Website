using System.Data.Entity;
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
        public DbSet<SubmittedClaim> SubmittedClaims { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //modelBuilder.ComplexType<Address>();
            //modelBuilder.ComplexType<PaymentInstrument>();

            //modelBuilder.Entity<ActiveCart>()
            //    .HasOptional(x => x.PaymentInstrument);

            modelBuilder.Entity<ClaimDraft>()
                .HasKey(x => new { x.Id });

            modelBuilder.Entity<ClaimDraft>()
                .Ignore(x => x.Procedures);

            modelBuilder.Entity<ClaimDraft>()
                .Ignore(x => x.Provider);

            modelBuilder.Entity<SubmittedClaim>()
                .HasKey(x => new { x.Id });

            base.OnModelCreating(modelBuilder);
        }
    }
}
