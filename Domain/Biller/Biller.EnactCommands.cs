using System.Linq;
using System.Threading.Tasks;
using Microsoft.Its.Domain;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
namespace Domain.Biller
{
    public partial class Biller
    {
        public class BillerCommandHandler :
            ICommandHandler<Biller, GrantRoles>,
            ICommandHandler<Biller, RevokeRoles>
        {
            public async Task EnactCommand(Biller biller, GrantRoles command)
            {
                biller.RecordEvent(new RolesGranted
                {
                    UserId = command.UserId,
                    Roles = command.Roles.ToArray()
                });
            }

            public async Task EnactCommand(Biller biller, RevokeRoles command)
            {
                if (biller.GetUser(command.UserId).Roles.Except(command.Roles).Any())
                {
                    biller.RecordEvent(new RolesRevoked
                    {
                        UserId = command.UserId,
                        Roles = command.Roles.ToArray()
                    });
                }
                else
                {
                    biller.RecordEvent(new UserRemoved
                    {
                        UserId = command.UserId
                    });
                }
            }

            public async Task HandleScheduledCommandException(Biller biller, CommandFailed<GrantRoles> command)
            {
            }

            public async Task HandleScheduledCommandException(Biller biller, CommandFailed<RevokeRoles> command)
            {
            }
        }
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
