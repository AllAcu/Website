using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Authentication;
using Microsoft.Its.Domain;

namespace Domain.Biller
{
    public partial class Biller : EventSourcedAggregate<Biller>
    {
        public static Guid AllAcuBillerId;

        protected Biller(Guid? id = default(Guid?)) : base(id)
        {
        }

        protected Biller(ISnapshot snapshot, IEnumerable<IEvent> eventHistory) : base(snapshot, eventHistory)
        {
        }

        public Biller(Guid id, IEnumerable<IEvent> eventHistory) : base(id, eventHistory)
        {
        }

        public Biller(InitializeBiller command) : base(command.AggregateId)
        {
            RecordEvent(new BillerInitialized
            {
                Name = command.Name
            });
        }

        public string Name { get; set; }
        public IList<UserAccess> Users { get; } = new List<UserAccess>();

        public static class Roles
        {
            public static Role System => new Role("system");
            public static Role Owner => new Role("owner");
            public static Role Approver => new Role("approver");
            public static Role Verifier => new Role("verifier");
        }
    }
}
