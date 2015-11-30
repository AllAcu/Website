using System;
using System.Collections.Generic;
using Domain.Authentication;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class RolesRevoked : Event<CareProvider>
        {
            public Guid UserId { get; set; }
            public IList<Role> Roles { get; set; }

            public override void Update(CareProvider provider)
            {
                var user = provider.GetUser(UserId);
                user.Roles.RemoveWhere(r => Roles.Contains(r));
            }
        }
    }
}
