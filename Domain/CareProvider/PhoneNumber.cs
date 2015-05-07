using Microsoft.Its.Domain;

namespace Domain
{
    public class PhoneNumber : String<PhoneNumber>
    {
        public PhoneNumber(string value) : base(value)
        {
            
        }
    }
}
