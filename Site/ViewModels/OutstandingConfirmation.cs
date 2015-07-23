using System;
using Domain.User;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class OutstandingConfirmationHandler :
        IUpdateProjectionWhen<User.SignedUp>,
        IUpdateProjectionWhen<User.Registered>
    {
        private readonly AllAcuSiteDbContext dbcontext;

        public OutstandingConfirmationHandler(AllAcuSiteDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public void UpdateProjection(User.SignedUp @event)
        {
            if (dbcontext.Confirmations.Find(@event.AggregateId) != null) return;

            dbcontext.Confirmations.Add(new OutstandingConfirmation
            {
                UserId = @event.AggregateId,
                Email = @event.Email,
                Token = @event.Token,
                WhenRequested = @event.ConfirmationSentDate
            });
            dbcontext.SaveChanges();
        }

        public void UpdateProjection(User.Registered @event)
        {
            dbcontext.Confirmations.Remove(dbcontext.Confirmations.Find(@event.AggregateId));

            dbcontext.SaveChanges();
        }

        //public void UpdateProjection(User.Invited @event)
        //{
        //    var confirmation = dbcontext.Confirmations.Find(@event.AggregateId);
        //    var provider = dbcontext.CareProviders.Find(@event.Invitation.ProviderId);

        //    confirmation.Invites.Add(new OutstandingConfirmation.Invite
        //    {
        //        Provider = provider,
        //        Role = @event.Invitation.Role.ToString()
        //    });

        //    dbcontext.SaveChanges();
        //}

        //public void UpdateProjection(Registration.FirstUserInvite @event)
        //{
        //    var provider = dbcontext.CareProviders.Find(@event.Invitation.ProviderId);

        //    dbcontext.Confirmations.Add(new OutstandingConfirmation
        //    {
        //        RegistrationId = @event.AggregateId,
        //        Email = @event.Email,
        //        Token = @event.Token,
        //        WhenRequested = DateTime.UtcNow,
        //        Invites = new[]
        //        {
        //            new OutstandingConfirmation.Invite
        //            {
        //                Provider = provider,
        //                Role = @event.Invitation.Role.ToString()
        //            }
        //        }
        //    });
        //}
    }

    public class OutstandingConfirmation
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public DateTime WhenRequested { get; set; }

    }
    public class Invitation
    {
        public Guid InviteId { get; set; } = Guid.NewGuid();
        public CareProviderDetails Provider { get; set; }
        public RoleList Roles { get; set; } = new RoleList();

        public class RoleList : SerialList<string>
        {

        }
    }
}