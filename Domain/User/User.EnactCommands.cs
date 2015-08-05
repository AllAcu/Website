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
            ICommandHandler<User, Register>,
            ICommandHandler<User, Invite>,
            ICommandHandler<User, AcceptInvite>,
            ICommandHandler<User, CreateSystemUser>
        {
            private readonly ICommandScheduler<CareProvider.CareProvider> providerCommands;

            public UserCommandHandler(ICommandScheduler<CareProvider.CareProvider> providerCommands)
            {
                this.providerCommands = providerCommands;
            }

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

            public async Task EnactCommand(User user, CreateSystemUser command)
            {
                user.RecordEvent(new SystemUserInitialized());
            }

            public async Task EnactCommand(User user, Register command)
            {
                user.RecordEvent(new Registered());
                user.RecordEvent(new Updated
                {
                    Name = command.Name
                });
            }

            public async Task EnactCommand(User user, Invite command)
            {
                user.RecordEvent(new Invited
                {
                    ProviderId = command.ProviderId,
                    Role = command.Role
                });
            }

            public async Task EnactCommand(User user, AcceptInvite command)
            {
                user.RecordEvent(new AcceptedInvite
                {
                    ProviderId = command.ProviderId,
                });

                await providerCommands.Schedule(command.ProviderId, new CareProvider.CareProvider.WelcomeUser
                {
                    UserId = user.Id
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

            public async Task HandleScheduledCommandException(User user, CommandFailed<Invite> command)
            {
            }

            public async Task HandleScheduledCommandException(User user, CommandFailed<AcceptInvite> command)
            {
            }

            public async Task HandleScheduledCommandException(User user, CommandFailed<CreateSystemUser> command)
            {
            }
        }
    }
}

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
