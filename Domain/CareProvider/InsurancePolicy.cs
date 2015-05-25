using System;
using Domain.CareProvider;

namespace Domain
{
    public class InsurancePolicy<TInsurance> : InsurancePolicy
    {
        public TInsurance Insurance { get; }

        public InsurancePolicy(TInsurance insurance)
        {
            Insurance = insurance;
        }
    }

    public class MedicalInsurance
    {
        public string InsuranceCompany { get; set; }
        public string Plan { get; set; }
        public string ProviderPhoneNumber { get; set; }
        public string InsuranceId { get; set; }
        public string GroupNumber { get; set; }
        public bool SecondaryCoverage { get; set; }
    }

    public class PersonalInjuryProtection
    {
        public DateTime DateOfInjury { get; set; }
        public string PlaceOfAccident { get; set; }
        public string InsuranceCarrier { get; set; }
        public string PolicyNumber { get; set; }
        public string ClaimNumber { get; set; }
        public string InsuranceCompanyAddress { get; set; }
        public string AdjusterName { get; set; }
        public string AdjusterPhone { get; set; }
        public string Injury { get; set; }
        public string Notes { get; set; }
    }

    public class Benefits
    {
        public bool IsCovered { get; set; }
        public bool IsDiagnosisRequirement { get; set; }
        public bool IsReferralRequired { get; set; }
        public string ReferralSource { get; set; }
        public bool IsPreCertificationRequired { get; set; }
        public string DiagnosisRequirement { get; set; }
        public string PreCertification { get; set; }
        public bool IsOfficeVisitCopay { get; set; }
        public string OfficeVisitCopay { get; set; }
        public bool IsCalendarYearPlan { get; set; }
        public string CalendarYearComments { get; set; }
        public DateTime CalendarYearPlanEnd { get; set; }
        public DateTime CalendarYearPlanBegin { get; set; }
        public bool IsDeductable { get; set; }
        public bool IndividualDeductable { get; set; }
        public bool IndividualDeductableUsed { get; set; }
        public string FamilyDeductable { get; set; }
        public string FamilyDeductableUsed { get; set; }
        public string InsurancePercentResponsibility { get; set; }
        public string PatientPercentResponsiblity { get; set; }
        public bool IsOutOfPocketMaximum { get; set; }
        public string AnnualIndividualOutOfPocketMaximum { get; set; }
        public string AnnualIndividualOutOfPocketMaximumUsed { get; set; }
        public string AnnualFamilyOutOfPocketMaximum { get; set; }
        public string AnnualFamilyOutOfPocketMaximumUsed { get; set; }
        public bool IsDeductableAppliedTowardOutOfPocket { get; set; }
        public bool HasAnnualVisitLimit { get; set; }
        public string AnnualVisitLimit { get; set; }
        public string AnnualVisitsUsed { get; set; }
        public string AnnualVisitsRemaining { get; set; }
        public bool IsBillableForCode99203 { get; set; }
        //public string Approval { get; set; }
        //public string ServiceCenterRepresentative { get; set; }
        //public string CallReferenceNumber { get; set; }
        //public string CallTime { get; set; }
        //public string AssignedTo { get; set; }
    }

    public abstract class InsurancePolicy
    {
        // relationship to policy for a patient?


        public Benefits Benefits { get; set; }
        public DateTimeOffset IssueDate { get; set; }
        public DateTimeOffset PolicyDate { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public DateTimeOffset? TerminationDate { get; set; }
    }
}
