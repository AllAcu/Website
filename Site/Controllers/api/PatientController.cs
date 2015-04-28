using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AllAcu.Models.Providers;
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
        private readonly AllAcuSiteDbContext allAcuSiteDbContext;

        public PatientController(IEventSourcedRepository<CareProvider> providerEventSourcedRepository,
            AllAcuSiteDbContext allAcuSiteDbContext)
        {
            this.providerEventSourcedRepository = providerEventSourcedRepository;
            this.allAcuSiteDbContext = allAcuSiteDbContext;
        }

        [Route(""), HttpGet]
        public IEnumerable<PatientListItemViewModel> GetAll()
        {
            var currentProviderId = ActionContext.ActionArguments.CurrentProviderId();
            if (currentProviderId != null)
            {
                return allAcuSiteDbContext.PatientList
                    .Where(p => p.ProviderId == currentProviderId);
            }
            return Enumerable.Empty<PatientListItemViewModel>();
        }


        [Route("{PatientId}"), HttpGet]
        public PatientEditViewModel Get(Guid patientId)
        {
            return new PatientEditViewModel
            {
                PatientId = patientId,
                PersonalInformation = allAcuSiteDbContext.Patients.FirstOrDefault(p => p.PatientId == patientId)?.PersonalInfo
            };
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
