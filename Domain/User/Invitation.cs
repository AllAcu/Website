using System;
using Domain.Authentication;

namespace Domain.User
{
    public partial class User
    {
        public class Invitation
        {
            public Guid ProviderId { get; set; }
            public Role Role { get; set; }
        }
    }
}