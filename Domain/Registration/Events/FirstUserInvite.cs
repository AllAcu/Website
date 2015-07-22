using System;
using Microsoft.Its.Domain;

namespace Domain.Registration
{
    public partial class Registration
    {
        public class FirstUserInvite : Event<Registration>
        {
            public string Email { get; set; }
            public string Token { get; set; }
            public Invitation Invitation { get; set; }
            public DateTime ConfirmationSentDate { get; set; }

            public override void Update(Registration registration)
            {
                registration.Email = Email;
                registration.Invitations.Add(Invitation);
                registration.Confirmation = new EmailConfirmation
                {
                    ConfirmationSentDate = ConfirmationSentDate,
                    Token = Token
                };
            }
        }
    }
}
