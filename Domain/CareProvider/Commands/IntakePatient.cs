using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class IntakePatient : Command<CareProvider>
        {
            public string Name { get; set; }
        }
    }
}
