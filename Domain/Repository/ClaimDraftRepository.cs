using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Its.Domain.Sql;

namespace Domain.Repository
{
    public class ClaimDraftRepository : ReadModelDbContext
    {
        public ClaimDraftRepository()
            : base(NameOrConnectionString)
        {

        }

        public DbSet<ClaimDraft> Drafts { get; set; }

        public void StartDraft(ClaimDraft draft)
        {
            Drafts.Add(draft);
            SaveChanges();
        }

        public IEnumerable<ClaimDraft> GetDrafts()
        {
            return Drafts.Take(10);
        }

        public ClaimDraft GetDraft(Guid id)
        {
            return Drafts.Find(id);
        }

        public void Update(ClaimDraft updated)
        {
            var draft = Drafts.Find(updated.Id);

            draft.Patient = updated.Patient;
            draft.Diagnosis = updated.Diagnosis;
            draft.DateOfService = updated.DateOfService;

            SaveChanges();
        }

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

            base.OnModelCreating(modelBuilder);
        }
    }
}
