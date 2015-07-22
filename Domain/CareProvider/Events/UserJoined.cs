using System;
using Domain.Authentication;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class UserJoined : Event<CareProvider>
        {
            public Guid UserId { get; set; }
            public override void Update(CareProvider provider)
            {
                provider.Users.Add(new UserAccess(UserId));
            }
        }
    }
}
