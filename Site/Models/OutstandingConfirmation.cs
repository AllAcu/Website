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
    }
}