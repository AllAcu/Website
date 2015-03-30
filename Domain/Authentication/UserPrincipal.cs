using System;
using System.Security.Principal;

namespace Domain.Authentication
{
    public class UserPrincipal : IPrincipal, IIdentity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string AuthenticationType { get; private set; }
        public string Ticket { get; set; }
        public string Email { get; set; }

        public UserPrincipal(Guid? id = null, string name = null, string email = null, string ticket = null)
        {
            Id = id ?? Guid.NewGuid();
            Name = name ?? "Anonymous";
            Email = email;
            Ticket = ticket;
        }

        public bool IsInRole(string role)
        {
            return true;
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public IIdentity Identity => this;

        public bool IsAuthenticated => !string.IsNullOrEmpty(Ticket);
    }
}
