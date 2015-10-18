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
        public Guid AssignedTo { get; set; }
        public Benefits Benefits { get; set; }

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
    }
}
