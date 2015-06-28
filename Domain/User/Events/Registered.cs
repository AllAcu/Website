using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Its.Domain;

namespace Domain.User
{
    public partial class User
    {
        public class Registered : Event<User>
        {
            public override void Update(User aggregate)
            {
                
            }
        }
    }
}
