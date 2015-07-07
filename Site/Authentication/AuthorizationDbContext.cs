using Microsoft.AspNet.Identity.EntityFramework;

namespace AllAcu.Authentication
{
    public class AuthorizationDbContext : IdentityDbContext<ApplicationUser>
    {
        public static string ConnectionString;

        public AuthorizationDbContext() 
            : base(ConnectionString, throwIfV1Schema: false)
        {
        }
    }
}