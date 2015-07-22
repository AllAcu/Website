using System;
using Domain.Authentication;
using Microsoft.Its.Domain;

namespace Domain.Registration
{
    public partial class Registration
    {
        public class Invite : ConstructorCommand<Registration>
        {
            public string Email { get; set; }
            public Guid ProviderId { get; set; }
            public Role Role { get; set; }
        }
    }
}
