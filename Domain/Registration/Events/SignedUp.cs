using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Its.Domain;

namespace Domain.Registration
{
    public partial class Registration
    {
        public class SignedUp : Event<Registration>
        {
            public string Email { get; set; }
            public string Token { get; set; }
            public DateTime ConfirmationSentDate { get; set; }

            public override void Update(Registration registration)
            {
                registration.Email = Email;
                registration.Confirmation = new EmailConfirmation
                {
                    Token = Token,
                    ConfirmationSentDate = ConfirmationSentDate
                };
            }
        }
    }
}
