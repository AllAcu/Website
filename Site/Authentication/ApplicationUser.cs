using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AllAcu.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public Guid UserId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            var idClaim = userIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim != null)
            {
                userIdentity.RemoveClaim(idClaim);
            }
            
            userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, UserId.ToString()));

            return userIdentity;
        }
    }
}
