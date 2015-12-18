using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class Registered : Event<User>
        {
            public string Email { get; set; }

            public override void Update(User user)
            {
                user.Email = Email;
            }
        }
    }
}
