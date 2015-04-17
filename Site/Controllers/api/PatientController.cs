using System;
using System.Collections.Generic;
using System.Web.Http;
using Domain;
using Domain.Authentication;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [CareProviderIdFilter]
    [RoutePrefix("api/patient")]
    public class PatientController : ApiController
    {
        private readonly IEventSourcedRepository<CareProvider> providerEventSourcedRepository;
        public PatientController(IEventSourcedRepository<CareProvider> providerEventSourcedRepository)
        {
            this.providerEventSourcedRepository = providerEventSourcedRepository;
        }

        [Route(""), HttpGet]
        public IEnumerable<Patient> GetAll()
        {
            var provider = providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            return provider.Patients;
        }

        [Route("{PatientId}"), HttpGet]
        public Patient Get(Guid patientId)
        {
            var provider = providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            return provider.GetPatient(patientId);
        }

        [Route(""), HttpPost]
        public Guid Intake(IntakeRequest request)
        {
            var command = new CareProvider.IntakePatient(request.Name);

            var provider = providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            command.ApplyTo(provider);

            providerEventSourcedRepository.Save(provider);

            return command.PatientId;
        }

        [Route("{PatientId}"), HttpPut]
        public void Update(Guid patientId, UpdateRequest request)
        {
            var command = new CareProvider.UpdatePatientInformation()
            {
                PatientId = patientId,
                Name = request.Name
            };

            var provider = providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            command.ApplyTo(provider);

            providerEventSourcedRepository.Save(provider);
        }

        [Route("poke"), HttpGet]
        public Guid Poke()
        {
            var command = new CareProvider.Poke() { PokeId = Guid.NewGuid() };
            var provider = providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            command.ApplyTo(provider);

            providerEventSourcedRepository.Save(provider);

            return command.PokeId;
        }

        public class IntakeRequest
        {
            public string Name { get; set; }
        }
        public class UpdateRequest
        {
            public string Name { get; set; }
        }
    }
}
