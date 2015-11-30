using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllAcu.Authentication;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace AllAcu.Authentication
{
    public class AuthorizationDbContext : IdentityDbContext<ApplicationUser>
    {
        public static string ConnectionString;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
