using System.Threading.Tasks;
using Microsoft.Its.Domain;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Domain.User
{
    public partial class User
    {
        public class UserCommandHandler :
            ICommandHandler<User, Update>
        {
            public async Task EnactCommand(User user, Update command)
            {
                user.RecordEvent(new Updated
                {
                    Name = command.Name
                });
            }

            public async Task HandleScheduledCommandException(User user, CommandFailed<Update> command)
            {
            }
        }
    }
}

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
