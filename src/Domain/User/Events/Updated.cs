using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class Updated : Event<User>
        {
            public string Name { get; set; }
            public override void Update(User user)
            {
                user.Name = Name;
            }
        }
    }
}
