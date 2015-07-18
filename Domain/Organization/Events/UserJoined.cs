using System;
using Microsoft.Its.Domain;

namespace Domain.Organization
{
    public partial class Organization
    {
        public class UserJoined : Event<Organization>
        {
            public Guid UserId { get; set; }
            public override void Update(Organization aggregate)
            {
                throw new NotImplementedException();
            }
        }
    }
}
