using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class BillerSystemUserInitialized : Event<User>
        {
            public string Name { get; set; }
            public string Email { get; set; }

            public override void Update(User user)
            {
                user.Name = Name;
                user.Email = Email;
                user.Confirmed = true;
            }
        }
    }
}
