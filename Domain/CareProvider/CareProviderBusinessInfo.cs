using System;

namespace Domain.Repository
{
    public class CareProviderBusinessInfo
    {
        public Guid Id { get; set; }
        public string BusinessName { get; set; }
        public string PractitionerName { get; set; }
        public string City { get; set; }
        public string NpiNumber { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxId { get; set; }
    }
}
