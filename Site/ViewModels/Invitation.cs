using System;
using System.Linq;
using Domain.User;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class Invitation
    {
        public Guid InviteId { get; set; } = Guid.NewGuid();
        public RoleList Roles { get; set; } = new RoleList();
    }

    public class Invitation<TOrganization> : Invitation
    {
        public virtual UserDetails User { get; set; }
        public virtual TOrganization Organization { get; set; }
    }

    public class ProviderInvitation : Invitation<CareProviderDetails>
    {
    }

    public class BillerInvitation : Invitation<BillerDetails>
    {

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
            Invitation invite;
            if (@event.IsBillerId())
            {
                invite = dbContext.BillerInvitations.FirstOrDefault(i => i.User.UserId == @event.AggregateId && i.Organization.Id == @event.OrganizationId);
                if (invite == null)
                {
                    invite = new BillerInvitation
                    {
                        User = dbContext.UserDetails.Find(@event.AggregateId),
                        Organization = dbContext.Billers.Find(@event.OrganizationId)
                    };
                    dbContext.BillerInvitations.Add((BillerInvitation)invite);
                }
            }
            else
            {
                invite = dbContext.ProviderInvitations.FirstOrDefault(i => i.User.UserId == @event.AggregateId && i.Organization.Id == @event.OrganizationId);
                if (invite == null)
                {
                    invite = new ProviderInvitation
                    {
                        User = dbContext.UserDetails.Find(@event.AggregateId),
                        Organization = dbContext.CareProviders.Find(@event.OrganizationId)
                    };
                    dbContext.ProviderInvitations.Add((ProviderInvitation)invite);
                }
            }

            invite.Roles.Add(@event.Role.ToString());

            dbContext.SaveChanges();
        }

        public void UpdateProjection(User.AcceptedInvite @event)
        {
            var invite = dbContext.ProviderInvitations.First(i => i.User.UserId == @event.AggregateId && i.Organization.Id == @event.ProviderId);

            dbContext.ProviderInvitations.Remove(invite);

            dbContext.SaveChanges();
        }
    }
}
