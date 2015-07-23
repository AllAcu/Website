using System;
using Domain.Authentication;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class Invited : Event<User>
        {
            public Guid ProviderId { get; set; }
            public Role Role { get; set; }

            public override void Update(User registration)
            {
                registration.Invitations.Add(new Invitation
                {
                    ProviderId = ProviderId,
                    Role = Role
                });
            }
        }
    }
}
