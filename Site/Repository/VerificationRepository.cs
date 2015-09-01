using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AllAcu.Repository
{
    public class VerificationRepository
    {
        private readonly AllAcuSiteDbContext dbContext;

        public VerificationRepository(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // should we be doing verification assignemnts through roles? feels like we're doing that ad-hoc anyway... originator/provider, approver, verifier
        // walk up a chain from document to provider? like, i've been assigned this document, but i'm not part of the provider... or i have no explicit assignment, but i'm
        // in that provider, so i get it due to my provider role...
        // Case to be made there's a patient query as well, as long as you have certain patient access?

            // a couple days later, we're on track to do permissions this way.


        public async Task<IEnumerable<InsuranceVerification>> Get(Guid userId)
        {
            var user = await dbContext.Users.FindAsync(userId);
            var allAcu = user.BillerRoles.AllAcu();
            if (allAcu != null)
            {
                if (allAcu.Roles.IsInRole(Domain.Biller.Biller.Roles.Approver))
                {
                    return await dbContext.Verifications.ToArrayAsync();
                }
            }

            var providers = user.ProviderRoles.Select(r => r.Provider.Id).ToArray();

            var providerVerifications = await dbContext.Verifications.Where(v => providers.Any(p => p == v.Provider.Id)).ToArrayAsync();
            return providerVerifications.Concat(await dbContext.Verifications.Where(v => v.AssignedTo != null && v.AssignedTo.UserId == userId).ToArrayAsync());
        }
    }
}
