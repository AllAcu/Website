using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class Invited : Event<User>
        {
            public Invitation Invitation { get; set; }

            public override void Update(User registration)
            {
                registration.Invitations.Add(Invitation);
            }
        }
    }
}
