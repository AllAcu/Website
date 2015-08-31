using System;
using Domain.User;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class OutstandingConfirmation
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public DateTime WhenRequested { get; set; }
    }

    public class OutstandingConfirmationEventHandler :
        IUpdateProjectionWhen<Domain.User.User.SignedUp>,
        IUpdateProjectionWhen<Domain.User.User.Registered>
    {
        private readonly AllAcuSiteDbContext dbcontext;

        public OutstandingConfirmationEventHandler(AllAcuSiteDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public void UpdateProjection(Domain.User.User.SignedUp @event)
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

        public void UpdateProjection(Domain.User.User.Registered @event)
        {
            dbcontext.Confirmations.Remove(dbcontext.Confirmations.Find(@event.AggregateId));

            dbcontext.SaveChanges();
        }
    }
}