using System;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public class VerificationRequestStatus : String<VerificationRequestStatus>
    {
        public VerificationRequestStatus(string value) : base(value)
        {
                    
        }
        public static VerificationRequestStatus Parse(string value)
        {
            if (String.Compare(value, "Draft", StringComparison.OrdinalIgnoreCase) == 0)
                return Draft;
            if (String.Compare(value, "Submitted", StringComparison.OrdinalIgnoreCase) == 0)
                return Submitted;
            if (String.Compare(value, "Approved", StringComparison.OrdinalIgnoreCase) == 0)
                return Approved;
            throw new ArgumentException($"Not a valid request status: {value}");
        }

        public static readonly VerificationRequestStatus Draft = new VerificationRequestStatus("Draft");
        public static readonly VerificationRequestStatus Submitted = new VerificationRequestStatus("Submitted");
        public static readonly VerificationRequestStatus Approved = new VerificationRequestStatus("Approved");
    }
}
