using Microsoft.Its.Domain;

namespace Domain.Registration
{
    public partial class Registration
    {
        public class Register : Command<Registration>
        {
            public string Name { get; set; }
            public string Password { get; set; }
            public string Token { get; set; }
        }
    }
}