using System;
using Microsoft.Its.Domain;

namespace Domain.Organization
{
    public partial class Organization
    {
        public class UserLeft : Event<Organization>
        {
            public override void Update(Organization aggregate)
            {
                throw new NotImplementedException();
            }

            public Guid UserId { get; set; }
        }         
    }
}
