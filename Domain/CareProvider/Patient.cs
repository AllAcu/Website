using System;

namespace Domain
{
    public class Patient
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Insurance { get; set; }
    }
}