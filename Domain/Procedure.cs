using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public class Procedure
    {
        ProcedureCode Code { get; set; }
        public string Name { get; set; }

    }

    public class ProcedureCode : String<ProcedureCode>
    {
        
    }
}