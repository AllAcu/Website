using System;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class JoinedProvider : Event<User>
        {
            public Guid ProviderId { get; set; }

            public override void Update(User user)
            {
                user.Providers.Add(ProviderId);
            }
        }
    }
}