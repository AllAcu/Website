using Microsoft.Its.Domain;

namespace Domain.Registration
{
    public partial class Registration
    {
        public class InviteAddedToUser : Event<Registration>
        {
            public Invitation Invitation { get; set; }

            public override void Update(Registration registration)
            {
                registration.Invitations.Add(Invitation);
            }
        }
    }
}
