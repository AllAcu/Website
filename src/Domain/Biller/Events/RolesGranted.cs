using System;
using System.Collections.Generic;
using Domain.Authentication;
using Microsoft.Its.Domain;

namespace Domain.Biller
{
    public partial class Biller
    {
        public class RolesGranted : Event<Biller>
        {
            public Guid UserId { get; set; }
            public IList<Role> Roles { get; set; }

            public override void Update(Biller biller)
            {
                var user = biller.GetUser(UserId);
                Roles.ForEach(r => user.Roles.Add(r));
            }
        }
    }
}
