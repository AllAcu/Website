using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class CreateProvider : ConstructorCommand<CareProvider>
        {
            public string BusinessName { get; set; }
            public string PractitionerName { get; set; }
            public string City { get; set; }
        }
    }
}
