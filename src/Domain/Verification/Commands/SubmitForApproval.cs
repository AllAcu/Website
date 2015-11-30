using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class SubmitForApproval : Command<InsuranceVerification>
        {
            public Guid AssignedToUserId { get; set; }
            public Benefits Benefits { get; set; }
            public string Comments { get; set; }
            public string Result { get; set; }
        }
    }
}