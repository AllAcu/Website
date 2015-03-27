using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimDraft
    {
        public class Submitted : Event<ClaimDraft>
        {
            public override void Update(ClaimDraft aggregate)
            {
                aggregate.submissions.Add(new Submission
                {
                    SubmissionDate = Now()
                });
            }
        }
    }
}