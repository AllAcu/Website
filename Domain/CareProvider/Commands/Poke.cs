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
        public class Poke : Command<CareProvider>
        {
            public Guid PokeId { get; set; }
        }
    }
}
