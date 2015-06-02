using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification : EventSourcedAggregate<InsuranceVerification>
    {
        public Guid PatientId { get; set; }
        public VerificationRequest Request { get; set; }

        public InsuranceVerification(Guid? id = default(Guid?)) : base(id)
        {
        }

        public InsuranceVerification(Guid id, IEnumerable<IEvent> eventHistory) : base(id, eventHistory)
        {
        }

        public InsuranceVerification(CreateVerification command) : base(command.AggregateId)
        {
            RecordEvent(new VerificationStarted
            {
                PatientId = command.PatientId,
                Request = command.Request
            });
        }
    }
}
