using System;
using System.Threading.Tasks;
using Microsoft.Its.Domain;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Domain.Registration
{
    public partial class Registration
    {
        public class RegistrationCommandHandler :
            ICommandHandler<Registration, Invite>,
            ICommandHandler<Registration, Register>,
            ICommandHandler<Registration, SignUp>
        {
            public Func<DateTime> Now = () => DateTime.UtcNow;

            public async Task EnactCommand(Registration registration, SignUp command)
            {
                registration.RecordEvent(new SignedUp
                {
                    Email = command.Email,
                    ConfirmationSentDate = Now(),
                    Token = GenerateToken()
                });
            }

            public async Task EnactCommand(Registration registration, Invite command)
            {
                registration.RecordEvent(new InviteAddedToUser
                {
                    Invitation = new Invitation
                    {
                        ProviderId = command.ProviderId,
                        Role = command.Role
                    }
                });
            }

            public async Task EnactCommand(Registration registration, Register command)
            {
                //UserId = Guid.NewGuid();
                //var userCommand = new User.User.Register
                //{
                //    AggregateId = UserId.Value,
                //    Name = command.Name,
                //    Email = Email,
                //};

                //// TODO (bremor) - figure out how to schedule this command

            }

            public async Task HandleScheduledCommandException(Registration registration, CommandFailed<Register> command)
            {
            }

            public async Task HandleScheduledCommandException(Registration registration, CommandFailed<Invite> command)
            {
            }

            public Task HandleScheduledCommandException(Registration aggregate, CommandFailed<SignUp> command)
            {
                throw new NotImplementedException();
            }
        }

    }
}

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
