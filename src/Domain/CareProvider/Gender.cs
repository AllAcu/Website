using System;
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

        public static Gender Parse(string value)
        {
            if (string.Compare(value, "Male", StringComparison.OrdinalIgnoreCase) == 0)
                return Male;
            if (string.Compare(value, "Female", StringComparison.OrdinalIgnoreCase) == 0)
                return Female;
            throw new ArgumentException($"Not a recognized gender: {value}");
        }
        public static Gender Male = new Gender("Male");
        public static Gender Female = new Gender("Female");
    }
}