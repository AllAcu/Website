namespace AllAcu.Models
{
    public class VerificationRole : UserRole<InsuranceVerification>
    {
        public virtual InsuranceVerification Verification
        {
            get { return Securable; }
            set { Securable = value; }
        }
    }
}
