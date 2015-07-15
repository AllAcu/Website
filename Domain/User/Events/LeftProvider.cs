using System;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class LeftProvider : Event<User>
        {
            public Guid ProviderId { get; set; }

            public override void Update(User user)
            {
                user.RemoveProviderAccess(ProviderId);
            }
        }
    }
}