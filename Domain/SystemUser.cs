using System;

namespace Domain
{
    public class SystemUser
    {
        public Guid BillerId { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
