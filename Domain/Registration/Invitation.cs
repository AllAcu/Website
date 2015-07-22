using System;
using Domain.Authentication;

namespace Domain.Registration
{
    public partial class Registration
    {
        public class Invitation
        {
            public Guid ProviderId { get; set; }
            public Role Role { get; set; }
        }
    }
}