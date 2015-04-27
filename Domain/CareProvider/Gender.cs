using Microsoft.Its.Domain;

namespace Domain
{
    public class Gender : String<Gender>
    {
        public Gender(string value) : base(value)
        {
            
        }

        public static implicit operator Gender(string gender)
        {
            return new Gender(gender);
        }

        public static Gender Male = new Gender("Male");
        public static Gender Female = new Gender("Female`");
    }
}