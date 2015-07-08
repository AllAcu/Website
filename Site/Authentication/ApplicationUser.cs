using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AllAcu.Authentication
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public Guid UserId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            var idClaim = userIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim != null)
            {
                userIdentity.RemoveClaim(idClaim);
            }
            
            userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, UserId.ToString()));

            // Add custom user claims here
            return userIdentity;
        }
    }
}