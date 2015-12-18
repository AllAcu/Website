using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class Register : ConstructorCommand<User>
        {
            public string Email { get; set; }
        }
    }
}
