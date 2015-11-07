using System;
using System.Collections.Generic;

namespace Domain.Authentication
{
    public class UserAccess
    {
        public UserAccess(Guid userId, params Role[] roles) : this(userId, (IEnumerable<Role>)roles)
        {
        }

        public UserAccess(Guid userId, IEnumerable<Role> roles)
        {
            this.UserId = userId;
            this.Roles = new HashSet<Role>(roles);
        }

        public Guid UserId { get; }
        public HashSet<Role> Roles { get; }
    }
}