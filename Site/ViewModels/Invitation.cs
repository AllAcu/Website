using System;
using System.Linq;
using Domain.User;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class Invitation
    {
        public Guid InviteId { get; set; } = Guid.NewGuid();

        public virtual UserDetails User { get; set; }
        public virtual CareProviderDetails Provider { get; set; }
        public RoleList Roles { get; set; } = new RoleList();
    }

    public class RoleList : SerialList<string>
    {

    }

    public class InviteHandler :
        IUpdateProjectionWhen<User.Invited>,
        IUpdateProjectionWhen<User.AcceptedInvite>
    {
        private readonly AllAcuSiteDbContext dbContext;
        public InviteHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(User.Invited @event)
        {
            var invite = dbContext.Invitations.FirstOrDefault(i => i.User.UserId == @event.AggregateId && i.Provider.Id == @event.ProviderId);

            if (invite == null)
            {
                invite = new Invitation
                {
                    User = dbContext.UserDetails.Find(@event.AggregateId),
                    Provider = dbContext.CareProviders.Find(@event.ProviderId)
                };
                dbContext.Invitations.Add(invite);
            }

            invite.Roles.Add(@event.Role.ToString());

            dbContext.SaveChanges();
        }

        public void UpdateProjection(User.AcceptedInvite @event)
        {
            var invite = dbContext.Invitations.First(i => i.User.UserId == @event.AggregateId && i.Provider.Id == @event.ProviderId);

            dbContext.Invitations.Remove(invite);

            dbContext.SaveChanges();
        }
    }
}
