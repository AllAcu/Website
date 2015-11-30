using System;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class CreateSystemUser : ConstructorCommand<User>
        {
            public Guid UserId { get; set; }
            public string Email { get; set; }
            public string Username { get; set; }
        }
    }
}
