using System;
using System.Linq;
using Domain.Authentication;
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
        public virtual User User { get; set; }
        public virtual TOrganization Organization { get; set; }
    }

    public class ProviderInvitation : Invitation<CareProvider>
    {
    }

    public class BillerInvitation : Invitation<Biller>
    {

    }

    public class RoleList : SerialList<string>
    {
        public bool IsInRole(string role)
        {
            return this.Contains(role, StringComparer.OrdinalIgnoreCase);
        }

        public bool IsInRole(Role role)
        {
            return IsInRole(role.ToString());
        } 
    }

    public class InvitionEventHandler :
        IUpdateProjectionWhen<Domain.User.User.Invited>,
        IUpdateProjectionWhen<Domain.User.User.AcceptedInvite>
    {
        private readonly AllAcuSiteDbContext dbContext;
        public InvitionEventHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(Domain.User.User.Invited @event)
        {
            Invitation invite;
            if (@event.IsBillerId())
            {
                invite = dbContext.BillerInvitations.FirstOrDefault(i => i.User.UserId == @event.AggregateId && i.Organization.Id == @event.OrganizationId);
                if (invite == null)
                {
                    invite = new BillerInvitation
                    {
                        User = dbContext.Users.Find(@event.AggregateId),
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
                        User = dbContext.Users.Find(@event.AggregateId),
                        Organization = dbContext.CareProviders.Find(@event.OrganizationId)
                    };
                    dbContext.ProviderInvitations.Add((ProviderInvitation)invite);
                }
            }

            invite.Roles.Add(@event.Role.ToString());

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.User.User.AcceptedInvite @event)
        {
            if (@event.IsBillerId())
            {
                var invite = dbContext.BillerInvitations.First(i => i.User.UserId == @event.AggregateId && i.Organization.Id == @event.OrganizationId);
                dbContext.BillerInvitations.Remove(invite);
            }
            else
            {
                var invite = dbContext.ProviderInvitations.First(i => i.User.UserId == @event.AggregateId && i.Organization.Id == @event.OrganizationId);
                dbContext.ProviderInvitations.Remove(invite);
            }

            dbContext.SaveChanges();
        }
    }
}
