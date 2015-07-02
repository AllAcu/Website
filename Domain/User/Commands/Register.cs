using System;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class Register : ConstructorCommand<User>
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }
    }
}
