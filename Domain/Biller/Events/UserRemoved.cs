using System;
using Microsoft.Its.Domain;

namespace Domain.Biller
{
    public partial class Biller
    {
        public class UserRemoved : Event<Biller>
        {
            public Guid UserId { get; set; }
            public override void Update(Biller biller)
            {
                biller.Users.Remove(biller.GetUser(UserId));
            }
        }
    }
}
