using System;
using Microsoft.Its.Domain;
using static System.String;

namespace Domain.Verification
{
    public class VerificationRequestStatus : String<VerificationRequestStatus>
    {
        public VerificationRequestStatus(string value) : base(value)
        {
                    
        }

        public static VerificationRequestStatus Parse(string value)
        {
            switch (value)
            {
                case "Draft":
                    return Draft;
                case "Submitted":
                    return Submitted;
                case "Assigned":
                    return Assigned;
                case "InProgress":
                    return InProgress;
                case "PendingApproval":
                    return PendingApproval;
                case "Verified":
                    return Verified;
                case "Rejected":
                    return Rejected;
            }
            throw new ArgumentException($"Not a valid request status: {value}");
        }

        public static readonly VerificationRequestStatus Draft = new VerificationRequestStatus("Draft");
        public static readonly VerificationRequestStatus Submitted = new VerificationRequestStatus("Submitted");
        public static readonly VerificationRequestStatus Assigned = new VerificationRequestStatus("Assigned");
        public static readonly VerificationRequestStatus InProgress = new VerificationRequestStatus("InProgress");
        public static readonly VerificationRequestStatus PendingApproval = new VerificationRequestStatus("PendingApproval");
        public static readonly VerificationRequestStatus Verified = new VerificationRequestStatus("Verified");
        public static readonly VerificationRequestStatus Rejected = new VerificationRequestStatus("Rejected");
    }
}
