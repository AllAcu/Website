using System;
using Domain.Verification;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public class PendingVerification
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Draft;
        public VerificationRequest Request { get; set; }
        public string ReviewerComments { get; set; }
        public Benefits Benefits { get; set; }

        public class RequestStatus : String<RequestStatus>
        {
            public RequestStatus(string value) : base(value)
            {
                    
            }
            public static RequestStatus Parse(string value)
            {
                if (String.Compare(value, "Draft", StringComparison.OrdinalIgnoreCase) == 0)
                    return Draft;
                if (String.Compare(value, "Submitted", StringComparison.OrdinalIgnoreCase) == 0)
                    return Submitted;
                if (String.Compare(value, "Approved", StringComparison.OrdinalIgnoreCase) == 0)
                    return Approved;
                throw new ArgumentException($"Not a valid request status: {value}");
            }

            public static readonly RequestStatus Draft = new RequestStatus("Draft");
            public static readonly RequestStatus Submitted = new RequestStatus("Submitted");
            public static readonly RequestStatus Approved = new RequestStatus("Approved");
        }
    }
}
