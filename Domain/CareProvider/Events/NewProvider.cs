using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class NewProvider : Event<CareProvider>
        {
            public string BusinessName { get; set; }
            public string PractitionerName { get; set; }
            public string City { get; set; }

            public override void Update(CareProvider provider)
            {
                provider.BusinessName = BusinessName;
                provider.PractitionerName = PractitionerName;
                provider.City = City;
            }
        }
    }
}
