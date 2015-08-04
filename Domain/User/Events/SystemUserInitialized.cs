using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class SystemUserInitialized : Event<User>
        {
            public override void Update(User user)
            {
                user.Confirmed = true;
            }
        }
    }
}
