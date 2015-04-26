using System;
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
