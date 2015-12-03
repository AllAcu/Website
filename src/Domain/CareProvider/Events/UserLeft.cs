using System;
using System.Linq;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class UserLeft : Event<CareProvider>
        {
            public Guid UserId { get; set; }

            public override void Update(CareProvider provider)
            {
                provider.Users.Remove(provider.Users.First(u => u.UserId == UserId));
            }
        }         
    }
}
