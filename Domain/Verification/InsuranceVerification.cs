using System;
using System.Collections.Generic;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification : EventSourcedAggregate<InsuranceVerification>
    {
        public Guid PatientId { get; set; }
        public VerificationRequest Request { get; set; } = new VerificationRequest();
        public PendingVerification.RequestStatus Status { get; set; } = PendingVerification.RequestStatus.Draft;

        public InsuranceVerification(Guid? id = default(Guid?)) : base(id)
        {
        }

        public InsuranceVerification(Guid id, IEnumerable<IEvent> eventHistory) : base(id, eventHistory)
        {
        }

        public InsuranceVerification(CreateVerification command) : base(command.AggregateId)
        {
            PatientId = command.PatientId;
            Request = command.RequestDraft ??  new VerificationRequest();
            Status = PendingVerification.RequestStatus.Draft;

            RecordEvent(new VerificationStarted
            {
                PatientId = command.PatientId,
                Request = command.RequestDraft
            });
        }
    }
}
