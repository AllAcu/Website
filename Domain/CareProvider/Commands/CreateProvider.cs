﻿using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class CreateProvider : ConstructorCommand<CareProvider>
        {
            public Guid CreatingUserId { get; set; }

            public string BusinessName { get; set; }
            public string City { get; set; }
            public string NpiNumber { get; set; }
            public string TaxId { get; set; }
        }
    }
}
