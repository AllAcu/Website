using Microsoft.Its.Domain;

namespace Domain.Registration
{
    public partial class Registration
    {
        public class EmailConfirmed : Event<Registration>
        {
            public override void Update(Registration registration)
            {
                registration.Confirmation.ConfirmationReceivedDate = Timestamp.UtcDateTime;
            }
        }
    }
}
