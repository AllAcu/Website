using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class Register : Command<User>
        {
            public string Name { get; set; }
            public string Password { get; set; }
            public string Token { get; set; }
        }
    }
}
