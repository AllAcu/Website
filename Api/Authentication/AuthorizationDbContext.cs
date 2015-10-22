using System.Data.Entity;
using System.Diagnostics;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Trace.WriteLine("OnModelCreating");
            base.OnModelCreating(modelBuilder);
        }
    }
}