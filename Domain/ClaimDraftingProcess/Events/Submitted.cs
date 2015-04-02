using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public partial class ClaimFilingProcess
    {
        public class Submitted : Event<ClaimFilingProcess>
        {
            public override void Update(ClaimFilingProcess aggregate)
            {
                //aggregate.submissions.Add(new Submission
                //{
                //    SubmissionDate = Now()
                //});
            }
        }
    }
}