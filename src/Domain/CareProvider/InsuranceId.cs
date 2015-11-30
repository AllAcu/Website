using Microsoft.Its.Domain;

namespace Domain
{
    public class InsuranceId : String<InsuranceId>
    {
        public InsuranceId(string value) : base(value)
        {
            
        }
    }
}