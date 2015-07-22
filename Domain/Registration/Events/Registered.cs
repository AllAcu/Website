using Microsoft.Its.Domain;

namespace Domain.Registration
{
    public partial class Registration
    {
        public class Registered : Event<Registration>
        {
            public string Name { get; set; }
            public string HashedPassword { get; set; }

            public override void Update(Registration registration)
            {
            }
        }
    }
}
