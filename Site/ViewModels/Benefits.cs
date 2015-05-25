using System;

namespace AllAcu
{
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
        public DateTime? CalendarYearPlanEnd { get; set; }
        public DateTime? CalendarYearPlanBegin { get; set; }
        public bool IsDeductable { get; set; }
        public string IndividualDeductable { get; set; }
        public string IndividualDeductableUsed { get; set; }
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


    public static class Verification_Extensions
    {
        public static Benefits ToBenefits(this Domain.Benefits benefits)
        {
            return new Benefits
            {
                IsCovered = benefits.IsCovered,
                IsDiagnosisRequirement = benefits.IsDiagnosisRequirement,
                IsReferralRequired = benefits.IsReferralRequired,
                ReferralSource = benefits.ReferralSource,
                IsPreCertificationRequired = benefits.IsPreCertificationRequired,
                DiagnosisRequirement = benefits.DiagnosisRequirement,
                PreCertification = benefits.PreCertification,
                IsOfficeVisitCopay = benefits.IsOfficeVisitCopay,
                OfficeVisitCopay = benefits.OfficeVisitCopay,
                IsCalendarYearPlan = benefits.IsCalendarYearPlan,
                CalendarYearComments = benefits.CalendarYearComments,
                CalendarYearPlanEnd = benefits.CalendarYearPlanEnd,
                CalendarYearPlanBegin = benefits.CalendarYearPlanBegin,
                IsDeductable = benefits.IsDeductable,
                IndividualDeductable = benefits.IndividualDeductable,
                IndividualDeductableUsed = benefits.IndividualDeductableUsed,
                FamilyDeductable = benefits.FamilyDeductable,
                FamilyDeductableUsed = benefits.FamilyDeductableUsed,
                InsurancePercentResponsibility = benefits.InsurancePercentResponsibility,
                PatientPercentResponsiblity = benefits.PatientPercentResponsiblity,
                IsOutOfPocketMaximum = benefits.IsOutOfPocketMaximum,
                AnnualIndividualOutOfPocketMaximum = benefits.AnnualIndividualOutOfPocketMaximum,
                AnnualIndividualOutOfPocketMaximumUsed = benefits.AnnualIndividualOutOfPocketMaximumUsed,
                AnnualFamilyOutOfPocketMaximum = benefits.AnnualFamilyOutOfPocketMaximum,
                AnnualFamilyOutOfPocketMaximumUsed = benefits.AnnualFamilyOutOfPocketMaximumUsed,
                IsDeductableAppliedTowardOutOfPocket = benefits.IsDeductableAppliedTowardOutOfPocket,
                HasAnnualVisitLimit = benefits.HasAnnualVisitLimit,
                AnnualVisitLimit = benefits.AnnualVisitLimit,
                AnnualVisitsUsed = benefits.AnnualVisitsUsed,
                AnnualVisitsRemaining = benefits.AnnualVisitsRemaining,
                IsBillableForCode99203 = benefits.IsBillableForCode99203
            };
        }
    }
}
