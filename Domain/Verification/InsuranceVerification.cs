using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification : EventSourcedAggregate<InsuranceVerification>
    {
        public Guid PatientId { get; set; }
        public VerificationRequest Request { get; set; } = new VerificationRequest();
        public VerificationRequestStatus Status { get; set; } = VerificationRequestStatus.Draft;
        public VerificationAssignment Assignment { get; set; }

        public InsuranceVerification(Guid? id = default(Guid?)) : base(id)
        {
        }

        public InsuranceVerification(Guid id, IEnumerable<IEvent> eventHistory) : base(id, eventHistory)
        {
        }

        public InsuranceVerification(Create command) : base(command.AggregateId)
        {
            RecordEvent(new NewVerification
            {
                PatientId = command.PatientId,
                RequestDraft = command.RequestDraft ?? new VerificationRequest()
            });
        }

        public class VerificationAssignment
        {
            public Guid UserId { get; set; }
            public string Comments { get; set; }
        }
    }
}
