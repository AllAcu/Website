using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Its.Domain;

namespace Domain.Biller
{
    public partial class Biller
    {
        public class InitializeBiller : ConstructorCommand<Biller>
        {
            public string Name { get; set; }
            public Guid SystemUserId { get; set; }
        } 
    }
}
