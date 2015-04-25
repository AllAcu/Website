using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AllAcu.Models.Patients;
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
        private readonly PatientDbContext patientDbContext;

        public PatientController(IEventSourcedRepository<CareProvider> providerEventSourcedRepository, PatientDbContext patientDbContext)
        {
            this.providerEventSourcedRepository = providerEventSourcedRepository;
            this.patientDbContext = patientDbContext;
        }

        [Route(""), HttpGet]
        public IEnumerable<PatientPersonalInformation> GetAll()
        {
            return patientDbContext.Patients;
        }

        [Route("{PatientId}"), HttpGet]
        public PatientPersonalInformation Get(Guid patientId)
        {
            return patientDbContext.Patients.FirstOrDefault(p => p.Id == patientId);
        }

        [Route(""), HttpPost]
        public Guid Intake(CareProvider.IntakePatient command)
        {
            var provider = providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            command.ApplyTo(provider);

            providerEventSourcedRepository.Save(provider);

            return command.PatientId;
        }

        [Route("{PatientId}"), HttpPut]
        public void Update(Guid patientId, CareProvider.UpdatePatientPersonalInformation command)
        {
            command.PatientId = patientId;
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
    }
}
