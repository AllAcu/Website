using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Verification;
using Microsoft.Its.Domain;

namespace AllAcu
{
    public class InsuranceVerification
    {
        public Guid VerificationId { get; set; }
        public Guid PatientId { get; set; }
        public string Status { get; set; }
        public string ProviderStatus { get; set; }

        public VerificationRequest Request { get; set; }
        public PatientInfo Patient { get; set; } = new PatientInfo();
        public Benefits Benefits { get; set; } = new Benefits();

        public class PatientInfo
        {
            public string InsuranceCarrier { get; set; }
            public string InsurancePhoneNumber { get; set; }
            public string Acupuncturist { get; set; }
            public string AcupuncturistNpiNumber { get; set; }
            public string AcupuncturistTaxId { get; set; }
            public string AcupuncturistAddress { get; set; }
            public string AcupuncturistPhoneNumber { get; set; }
            public string PatientName { get; set; }
            public string PatientDateOfBirth { get; set; }
            public string PatientInsurancePolicy { get; set; }
        }

        public class VerificationCall
        {
            public DateTimeOffset StartTime { get; set; }
            public DateTimeOffset? EndTime { get; set; }
            public string ServiceCenterRepresentative { get; set; }
            public string ReferenceNumber { get; set; }
            public string Result { get; set; }
            public string Comments { get; set; }
        }

        public class Assignment
        {
            public DateTimeOffset AssignmentDate { get; set; }
            public User AssignedTo { get; set; }
            public string Comments { get; set; }
        }

        public class BillerApproval
        {
            public DateTimeOffset ApprovalDate { get; set; }
            public string ApproverName { get; set; }
        }

        public string ServiceCenterRepresentative { get; set; }
        public string CallReferenceNumber { get; set; }
        public string CallTime { get; set; }
        public virtual CareProvider Provider { get; set; }
        public virtual User AssignedTo { get; set; }
        public DateTimeOffset AssignmentTime { get; set; }
        public DateTimeOffset? CurrentCallStartTime { get; set; }
        public CallList CallHistory { get; set; } = new CallList();
        public AssignmentList AssignmentHistory { get; set; } = new AssignmentList();
        public virtual BillerApproval Approval { get; set; } = new BillerApproval();
        public string RejectionReason { get; set; }

        public class CallList : SerialList<VerificationCall>
        {
        }
        public class AssignmentList : SerialList<Assignment>
        {
        }
    }


    public class InsuranceVerificationEventHandler :
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.NewVerification>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.CallEnded>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.CallStarted>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.Completed>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.Delegated>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.DraftUpdated>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.RequestSubmitted>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.RequestRejected>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.SubmittedForApproval>,
        IUpdateProjectionWhen<Domain.Verification.InsuranceVerification.Updated>,
        IUpdateProjectionWhen<Domain.CareProvider.CareProvider.PatientInformationUpdated>
    {
        private readonly AllAcuSiteDbContext dbContext;

        public InsuranceVerificationEventHandler(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.NewVerification @event)
        {
            var request = new InsuranceVerification
            {
                PatientId = @event.PatientId,
                VerificationId = @event.AggregateId,
                Provider = dbContext.Patients.Find(@event.PatientId).Provider,
                Request = @event.RequestDraft ?? new VerificationRequest(),
                Status = "Draft",
                ProviderStatus = "Draft"
            };

            dbContext.Verifications.Add(request);

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.CallStarted @event)
        {
            var verification = dbContext.Verifications.Find(@event.AggregateId);
            verification.Status = "In Progress";
            verification.CurrentCallStartTime = @event.TimeStarted;
            verification.ServiceCenterRepresentative = @event.ServiceCenterRepresentative;

            verification.CallHistory.Add(new InsuranceVerification.VerificationCall
            {
                StartTime = @event.TimeStarted,
                ServiceCenterRepresentative = @event.ServiceCenterRepresentative
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.CallEnded @event)
        {
            var verification = dbContext.Verifications.Find(@event.AggregateId);
            verification.Status = "Assigned";
            verification.CurrentCallStartTime = null;

            var call = verification.CallHistory.Last();
            call.EndTime = @event.TimeEnded;
            call.Comments = @event.Comments;
            call.Result = @event.Result;
            call.ServiceCenterRepresentative = @event.ServiceCenterRepresentative;
            call.ReferenceNumber = @event.ReferenceNumber;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.Completed @event)
        {
            var verification = dbContext.Verifications.Find(@event.AggregateId);
            verification.Status = "Verified";
            verification.ProviderStatus = "Verified";
            verification.Approval = new InsuranceVerification.BillerApproval
            {
                ApprovalDate = @event.Timestamp,
                ApproverName = dbContext.Users.Find(@event.ApproverUserId).Name
            };

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.Delegated @event)
        {
            var verification = dbContext.Verifications.Find(@event.AggregateId);
            verification.AssignedTo = dbContext.Users.Find(@event.AssignedToUserId);
            verification.AssignmentTime = @event.Timestamp;
            verification.Status = "Assigned";

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.DraftUpdated @event)
        {
            var verification = dbContext.Verifications.First(f => f.VerificationId == @event.AggregateId);
            verification.Request = @event.Request;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.RequestRejected @event)
        {
            var verification = dbContext.Verifications.First(f => f.VerificationId == @event.AggregateId);
            verification.Status = "Rejected";
            verification.ProviderStatus = "Missing Information";
            verification.RejectionReason = @event.Reason;

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.RequestSubmitted @event)
        {
            var verification = dbContext.Verifications.First(f => f.VerificationId == @event.AggregateId);
            var patient = dbContext.Patients.First(p => p.PatientId == verification.PatientId);
            var provider = patient.Provider;

            verification.Status = "Submitted";
            verification.ProviderStatus = "Submitted";
            verification.Patient = new InsuranceVerification.PatientInfo
            {
                InsuranceCarrier = patient.MedicalInsurance != null
                    ? patient.MedicalInsurance.InsuranceCompany
                    : patient.PersonalInjuryProtection.InsuranceCarrier,
                InsurancePhoneNumber = patient.MedicalInsurance != null
                    ? patient.MedicalInsurance.ProviderPhoneNumber
                    : patient.PersonalInjuryProtection.AdjusterPhone,
                Acupuncturist = "Jimmy Needles",
                AcupuncturistNpiNumber = provider.NpiNumber,
                AcupuncturistAddress = provider.Address,
                AcupuncturistPhoneNumber = provider.PhoneNumber,
                AcupuncturistTaxId = provider.TaxId,
                PatientName = patient.Name,
                PatientDateOfBirth = patient.DateOfBirth,
                PatientInsurancePolicy = patient.MedicalInsurance != null
                    ? patient.MedicalInsurance.Plan
                    : patient.PersonalInjuryProtection.ClaimNumber
            };

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.SubmittedForApproval @event)
        {
            var verification = dbContext.Verifications.Find(@event.AggregateId);
            verification.Status = "Pending Approval";
            verification.AssignmentTime = @event.Timestamp;
            verification.AssignmentHistory.Add(new InsuranceVerification.Assignment
            {
                AssignedTo = dbContext.Users.Find(@event.AssignedToUserId),
                Comments = @event.Comments,
                AssignmentDate = @event.Timestamp
            });

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.Verification.InsuranceVerification.Updated @event)
        {
            var verification = dbContext.Verifications.Find(@event.AggregateId);
            verification.Benefits = @event.Benefits.ToBenefits();

            dbContext.SaveChanges();
        }

        public void UpdateProjection(Domain.CareProvider.CareProvider.PatientInformationUpdated @event)
        {
            var verification = dbContext.Verifications.SingleOrDefault(f => f.PatientId == @event.PatientId);

            if (verification != null)
            {
                verification.Patient.PatientName = @event.UpdatedName ?? verification.Patient.PatientName;
                verification.Patient.PatientDateOfBirth = @event.UpdatedDateOfBirth?.ToShortDateString() ??
                                                          verification.Patient.PatientDateOfBirth;
            }
        }
    }
}
