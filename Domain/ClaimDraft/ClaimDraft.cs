using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimDraft : EventSourcedAggregate<ClaimDraft>
    {
        public static Func<DateTimeOffset> Now = () => DateTimeOffset.UtcNow;

        public Patient Patient { get; set; }
        public CareProvider Provider { get; set; }
        public string Diagnosis { get; set; }
        public DateTimeOffset DateOfService { get; set; }
        public IList<Procedure> Procedures { get; set; }

        public ClaimDraft()
        {
            
        }

        public ClaimDraft(Guid id, IEnumerable<IEvent> events) : base(id, events)
        {
            
        }

        // TODO : Note sure I like this... can history handle that?
        private IList<Submission> submissions = new List<Submission>();

        public class Submission
        {
            public DateTimeOffset SubmissionDate { get; set; }
        }
    }
}