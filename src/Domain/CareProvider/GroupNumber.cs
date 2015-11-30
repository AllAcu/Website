using Microsoft.Its.Domain;

namespace Domain
{
    public class GroupNumber : String<GroupNumber>
    {
        public GroupNumber(string value) : base(value)
        {
            
        }
    }
}