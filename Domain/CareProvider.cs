using Microsoft.Its.Domain;

namespace Domain
{
    public class CareProvider
    {
        public CareProviderId Id { get; set; }
        public string Name { get; set; }

        public class CareProviderId : String<CareProviderId>
        {
            
        }
    }
}