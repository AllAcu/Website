using Microsoft.Its.Domain;

namespace Domain.Registration
{
    public partial class Registration
    {
        public class SignUp : ConstructorCommand<Registration>
        {
            public string Email { get; set; }
        }
    }
}