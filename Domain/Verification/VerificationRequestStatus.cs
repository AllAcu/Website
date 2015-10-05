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
                    return VerificationRequestStatus.Draft;
                case "Submitted":
                    return VerificationRequestStatus.Submitted;
                case "Assigned":
                    return VerificationRequestStatus.Assigned;
                case "PendingApproval":
                    return VerificationRequestStatus.PendingApproval;
                case "Flagged":
                    return VerificationRequestStatus.Flagged;
                case "Verified":
                    return VerificationRequestStatus.Verified;
                case "Rejected":
                    return VerificationRequestStatus.Rejected;
            }
            throw new ArgumentException($"Not a valid request status: {value}");
        }

        public static readonly VerificationRequestStatus Draft = new VerificationRequestStatus("Draft");
        public static readonly VerificationRequestStatus Submitted = new VerificationRequestStatus("Submitted");
        public static readonly VerificationRequestStatus Assigned = new VerificationRequestStatus("Assigned");
        public static readonly VerificationRequestStatus PendingApproval = new VerificationRequestStatus("PendingApproval");
        public static readonly VerificationRequestStatus Flagged = new VerificationRequestStatus("Flagged");
        public static readonly VerificationRequestStatus Verified = new VerificationRequestStatus("Verified");
        public static readonly VerificationRequestStatus Rejected = new VerificationRequestStatus("Rejected");
    }
}
