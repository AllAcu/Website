using System;
using System.Collections.Generic;
using Domain.Authentication;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.Biller
{
    public partial class Biller
    {
        public class AddUser : Command<Biller>
        {
            public Guid UserId { get; set; }
        }
    }
}
