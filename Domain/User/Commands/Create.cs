using System;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class Create : ConstructorCommand<User>
        {
            public Guid UserId { get; set; }
        }
    }
}
