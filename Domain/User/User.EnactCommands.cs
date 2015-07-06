using System.Threading.Tasks;
using Microsoft.Its.Domain;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Domain.User
{
    public partial class User
    {
        public class UserCommandHandler :
            ICommandHandler<User, Update>,
            ICommandHandler<User, JoinProvider>,
            ICommandHandler<User, LeaveProvider>
        {
            public async Task EnactCommand(User user, Update command)
            {
                user.RecordEvent(new Updated
                {
                    Name = command.Name
                });
            }

            public async Task EnactCommand(User user, JoinProvider command)
            {
                user.RecordEvent(new JoinedProvider
                {
                    ProviderId = command.ProviderId
                });
            }

            public async Task EnactCommand(User user, LeaveProvider command)
            {
                user.RecordEvent(new LeftProvider
                {
                    ProviderId = command.ProviderId
                });
            }

            public async Task HandleScheduledCommandException(User user, CommandFailed<Update> command)
            {
            }

            public async Task HandleScheduledCommandException(User aggregate, CommandFailed<JoinProvider> command)
            {
            }

            public async Task HandleScheduledCommandException(User aggregate, CommandFailed<LeaveProvider> command)
            {
            }
        }
    }
}

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
