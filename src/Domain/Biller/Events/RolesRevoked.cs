using System;
using System.Collections.Generic;
using Domain.Authentication;
using Microsoft.Its.Domain;

namespace Domain.Biller
{
    public partial class Biller
    {
        public class RolesRevoked : Event<Biller>
        {
            public Guid UserId { get; set; }
            public IList<Role> Roles { get; set; }

            public override void Update(Biller biller)
            {
                var user = biller.GetUser(UserId);
                user.Roles.RemoveWhere(r => Roles.Contains(r));
            }
        }
    }
}
