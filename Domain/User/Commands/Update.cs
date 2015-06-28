using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class Update : Command<User>
        {
            public string Name { get; set; }
        }
    }
}
