using System;
using System.Collections.Generic;

namespace Domain
{
    public class Visit
    {
        public string Diagnosis { get; set; }
        public DateTimeOffset DateOfService { get; set; }
        public IList<Procedure> Procedures { get; set; }
    }
}