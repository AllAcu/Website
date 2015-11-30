using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AllAcu.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public Guid UserId { get; set; }

        public override string Id
        {
            get { return UserId.ToString(); }
            set { UserId = Guid.Parse(value); }
        }
    }
}
