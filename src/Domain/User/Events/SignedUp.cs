using System;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class SignedUp : Event<User>
        {
            public string Email { get; set; }
            public string Token { get; set; }
            public DateTime ConfirmationSentDate { get; set; }

            public override void Update(User user)
            {
                user.Email = Email;
            }
        }
    }
}
