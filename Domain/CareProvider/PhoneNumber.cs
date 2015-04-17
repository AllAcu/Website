using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public class PhoneNumber : String<PhoneNumber>
    {
        public PhoneNumber(string value) : base(value)
        {
            
        }
    }
}
