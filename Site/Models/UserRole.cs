using System;

namespace AllAcu.Models
{
    public class UserRole<T>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public virtual User User { get; set; }
        protected virtual T Securable { get; set; }
        public RoleList Roles { get; set; } = new RoleList();
    }
}
