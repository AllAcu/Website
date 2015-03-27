using Microsoft.Its.Domain;

namespace Domain
{
    public partial class Claim : EventSourcedAggregate<Claim>
    {
        ClaimId ClaimNumber { get; set; }

    }
}