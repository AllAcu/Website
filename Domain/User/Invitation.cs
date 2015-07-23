using System;
using System.Collections.Generic;
using Domain.Authentication;

namespace Domain.User
{
    public partial class User
    {
        public class Invitation
        {
            public Guid Id { get; set; }
            public Guid ProviderId { get; set; }
            public IList<Role> Roles { get; set; } = new List<Role>();
        }
    }
}