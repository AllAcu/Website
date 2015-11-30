using System;
using Domain.Authentication;
using Microsoft.Its.Domain;

namespace Domain.Biller
{
    public partial class Biller
    {
        public class UserAdded : Event<Biller>
        {
            public Guid UserId { get; set; }

            public override void Update(Biller biller)
            {
                biller.Users.Add(new UserAccess(UserId));
            }
        }
    }
}
