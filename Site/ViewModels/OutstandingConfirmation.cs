using System;
using System.Collections.Generic;
using Domain.Authentication;
using Domain.Registration;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class OutstandingConfirmation
    {
        public Guid RegistrationId { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public DateTime WhenRequested { get; set; }
        public virtual IList<Invite> Invites { get; set; } = new List<Invite>();

        public class Invite
        {
            public Guid InviteId { get; set; } = Guid.NewGuid();
            public CareProviderDetails Provider { get; set; }
            public Role Role { get; set; }
        }
    }

    public class OutstandingConfirmationHandler :
        IUpdateProjectionWhen<Registration.FirstUserInvite>,
        IUpdateProjectionWhen<Registration.InviteAddedToUser>
    {
        private AllAcuSiteDbContext dbcontext;

        public OutstandingConfirmationHandler(AllAcuSiteDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public void UpdateProjection(Registration.InviteAddedToUser @event)
        {
            var confirmation = dbcontext.Confirmations.Find(@event.AggregateId);
            var provider = dbcontext.CareProviders.Find(@event.Invitation.ProviderId);

            confirmation.Invites.Add(new OutstandingConfirmation.Invite
            {
                Provider = provider,
                Role = @event.Invitation.Role
            });

            dbcontext.SaveChanges();
        }

        public void UpdateProjection(Registration.FirstUserInvite @event)
        {
            var provider = dbcontext.CareProviders.Find(@event.Invitation.ProviderId);

            dbcontext.Confirmations.Add(new OutstandingConfirmation
            {
                RegistrationId = @event.AggregateId,
                Email = @event.Email,
                Token = @event.Token,
                WhenRequested = DateTime.UtcNow,
                Invites = new[]
                {
                    new OutstandingConfirmation.Invite
                    {
                        Provider = provider,
                        Role = @event.Invitation.Role
                    }
                }
            });
        }
    }
}