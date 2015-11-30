using System;
using Microsoft.Its.Domain;

namespace Domain.Biller
{
    public partial class Biller
    {
        public class RemoveUser : Command<Biller>
        {
            public Guid UserId { get; set; }
        }
    }
}
