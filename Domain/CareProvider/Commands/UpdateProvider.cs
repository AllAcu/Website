using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class UpdateProvider : Command<CareProvider>
        {
            public string BusinessName { get; set; }
            public string City { get; set; }
            public string NpiNumber { get; set; }
            public string TaxId { get; set; }
        }
    }
}
