using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class Registered : Event<User>
        {
            public override void Update(User user)
            {
                user.Confirmed = true;
            }
        }
    }
}
