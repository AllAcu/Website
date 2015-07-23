using System;
using System.Threading.Tasks;
using Microsoft.Its.Domain;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Domain.User
{
    public partial class User
    {
        public class UserCommandHandler :
            ICommandHandler<User, Update>,
            ICommandHandler<User, SignUp>,
            ICommandHandler<User, Register>
        {
            public async Task EnactCommand(User user, Update command)
            {
                user.RecordEvent(new Updated
                {
                    Name = command.Name
                });
            }

            public async Task EnactCommand(User user, SignUp command)
            {
                user.RecordEvent(new SignedUp
                {
                    Email = command.Email,
                    ConfirmationSentDate = DateTime.UtcNow,
                    Token = SignupToken.GenerateToken()
                });
            }

            public async Task EnactCommand(User user, Register command)
            {
                user.RecordEvent(new Registered());
                user.RecordEvent(new Updated
                {
                    Name = command.Name
                });
            }

            public async Task HandleScheduledCommandException(User user, CommandFailed<Update> command)
            {
            }

            public async Task HandleScheduledCommandException(User user, CommandFailed<SignUp> command)
            {
            }

            public async Task HandleScheduledCommandException(User user, CommandFailed<Register> command)
            {
            }
        }
    }
}

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
