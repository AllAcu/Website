using System;
using System.Threading.Tasks;

namespace Domain.Registration
{
    public partial class Registration
    {
        public Func<DateTime> Now = () => DateTime.UtcNow;

        public async Task EnactCommand(Invite command)
        {
            RecordEvent(new InviteAddedToUser
            {
                Invitation = new Invitation
                {
                    ProviderId = command.ProviderId,
                    Role = command.Role
                }
            });
        }

        public async Task EnactCommand(ConfirmEmail command)
        {
            RecordEvent(new EmailConfirmed());
        }

        public async Task EnactCommand(Register command)
        {
            UserId = Guid.NewGuid();
            var userCommand = new User.User.Register
            {
                AggregateId = UserId.Value,
                Name = command.Name,
                Email = Email,
            };

            // TODO (bremor) - figure out how to schedule this command


        }

    }
}
