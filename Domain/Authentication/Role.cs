using Microsoft.Its.Domain;

namespace Domain.Authentication
{
    public class Role : String<Role>
    {
        public Role(string role) : base(role)
        {
        }
    }
}