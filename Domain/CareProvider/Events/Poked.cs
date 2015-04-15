using System;
using System.Diagnostics;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class Poked : Event<CareProvider>
        {
            public Guid PokeId { get; set; }
            public override void Update(CareProvider aggregate)
            {
                Debug.WriteLine("updating with a poke");
            }
        }
    }
}
