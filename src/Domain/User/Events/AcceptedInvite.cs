using System;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class AcceptedInvite : Event<User>
        {
            public Guid OrganizationId { get; set; }
            public override void Update(User user)
            {
                user.Invitations.Remove(user.Invitations.First(i => i.OrganizationId == OrganizationId));
            }

            public bool IsBillerId()
            {
                return OrganizationId == Biller.Biller.AllAcuBillerId;
            }
        }
    }
}
