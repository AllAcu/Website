using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class SignUp : ConstructorCommand<User>
        {
            public string Email { get; set; }
        }
    }
}