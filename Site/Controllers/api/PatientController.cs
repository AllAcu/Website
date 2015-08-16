using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AllAcu.Authentication;
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
            var currentProviderId = this.CurrentProviderId();
            if (currentProviderId != null)
            {
                return allAcuSiteDbContext.PatientList
                    .Where(p => p.ProviderId == currentProviderId);
            }
            return Enumerable.Empty<PatientListItemViewModel>();
        }

        [Route("{PatientId}"), HttpGet]
        public PatientDetails Details(Guid patientId)
        {
            var patient = allAcuSiteDbContext.PatientDetails.FirstOrDefault(p => p.PatientId == patientId);
            Debug.WriteLine(patient?.MedicalInsurance?.Plan);
            return patient;
        }

        [Route("edit/{PatientId}"), HttpGet]
        public PatientDetails Edit(Guid patientId)
        {
            return allAcuSiteDbContext.PatientDetails.FirstOrDefault(p => p.PatientId == patientId);
        }

        [Route(""), HttpPost]
        public async Task<Guid> Intake(CareProvider.IntakePatient command)
        {
            var provider = await providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            await command.ApplyToAsync(provider);
            await providerEventSourcedRepository.Save(provider);

            return command.PatientId;
        }

        [Route("{PatientId}"), HttpPut]
        public async Task Update(Guid patientId, CareProvider.UpdatePatientPersonalInformation command)
        {
            command.PatientId = patientId;
            var provider = await providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            await command.ApplyToAsync(provider);
            await providerEventSourcedRepository.Save(provider);
        }

        [Route("{PatientId}/insurance"), HttpPost]
        public async Task UpdateInsurance(Guid patientId, CareProvider.UpdateInsurance command)
        {
            command.PatientId = patientId;
            var provider = await providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            await command.ApplyToAsync(provider);
            await providerEventSourcedRepository.Save(provider);
        }

        [Route("{PatientId}/contact"), HttpPut]
        public async Task UpdateContactInfo(Guid patientId, CareProvider.UpdatePatientContactInformation command)
        {
            command.PatientId = patientId;
            var provider = await providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            await command.ApplyToAsync(provider);
            await providerEventSourcedRepository.Save(provider);
        }
    }
}
