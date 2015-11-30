using System;
using System.Linq;
using Domain.Authentication;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class Invited : Event<User>
        {
            public Guid OrganizationId { get; set; }
            public Role Role { get; set; }

            public override void Update(User user)
            {
                var invite = user.Invitations.FirstOrDefault(i => i.OrganizationId == OrganizationId);
                if (invite == null)
                {
                    invite = new Invitation { OrganizationId = OrganizationId };
                    user.Invitations.Add(invite);
                }
                invite.Roles.Add(Role);
            }

            public bool IsBillerId()
            {
                return OrganizationId == Biller.Biller.AllAcuBillerId;
            }
        }
    }
}
