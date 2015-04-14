﻿using System;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class NewPatient : Event<CareProvider>
        {
            public string Name { get; set; }
            public Guid PatientId { get; set; }

            public override void Update(CareProvider provider)
            {
                provider.Patients.Add(new Patient(PatientId)
                {
                    Name = Name
                });
            }
        }
    }
}
