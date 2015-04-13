using System;

namespace Domain.Repository
{
    public class CareProviderInfo
    {
        public Guid Id { get; set; }
        public string BusinessName { get; set; }
        public string PractitionerName { get; set; }
        public string City { get; set; }
    }
}
