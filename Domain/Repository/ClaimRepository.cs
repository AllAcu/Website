using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class ClaimRepository : Microsoft.Its.Domain.InMemorySnapshotRepository
    {
        public Claim GetClaimById(ClaimId id)
        {
            return null;
        }
    }
}
