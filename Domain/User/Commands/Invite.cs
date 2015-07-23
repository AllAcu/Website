using System;
using Domain.Authentication;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class Invite : Command<User>
        {
            public string Email { get; set; }
            public Guid ProviderId { get; set; }
            public Role Role { get; set; }
        }
    }
}
