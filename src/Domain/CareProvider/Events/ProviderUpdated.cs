using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class ProviderUpdated : Event<CareProvider>
        {
            public string BusinessName { get; set; }
            public string City { get; set; }
            public string NpiNumber { get; set; }
            public string TaxId { get; set; }

            public override void Update(CareProvider provider)
            {
                provider.BusinessName = BusinessName;
                provider.City = City;
                provider.NpiNumber = NpiNumber;
                provider.TaxId = TaxId;
            }
        }
    }
}
