using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AllAcu.Authentication;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers
{
    [CareProviderIdFilter]
    [RoutePrefix("patient")]
    public class PatientController : ApiController
    {
        private readonly IEventSourcedRepository<Domain.CareProvider.CareProvider> providerEventSourcedRepository;
        private readonly AllAcuSiteDbContext allAcuSiteDbContext;

        public PatientController(IEventSourcedRepository<Domain.CareProvider.CareProvider> providerEventSourcedRepository,
            AllAcuSiteDbContext allAcuSiteDbContext)
        {
            this.providerEventSourcedRepository = providerEventSourcedRepository;
            this.allAcuSiteDbContext = allAcuSiteDbContext;
        }

        [Route(""), HttpGet]
        public IEnumerable<Patient> GetAll()
        {
            var currentProviderId = this.CurrentProviderId();
            if (currentProviderId != null)
            {
                return allAcuSiteDbContext.Patients
                    .Where(p => p.Provider.Id == currentProviderId).ToArray();
            }
            return Enumerable.Empty<Patient>();
        }

        [Route("{PatientId}"), HttpGet]
        public Patient Details(Guid patientId)
        {
            var patient = allAcuSiteDbContext.Patients.FirstOrDefault(p => p.PatientId == patientId);
            Debug.WriteLine(patient?.MedicalInsurance?.Plan);
            return patient;
        }

        [Route("edit/{PatientId}"), HttpGet]
        public Patient Edit(Guid patientId)
        {
            return allAcuSiteDbContext.Patients.FirstOrDefault(p => p.PatientId == patientId);
        }

        [Route(""), HttpPost]
        public async Task<Guid> Intake(Domain.CareProvider.CareProvider.IntakePatient command)
        {
            var provider = await providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            await command.ApplyToAsync(provider);
            await providerEventSourcedRepository.Save(provider);

            return command.PatientId;
        }

        [Route("{PatientId}"), HttpPut]
        public async Task Update(Guid patientId, Domain.CareProvider.CareProvider.UpdatePatientPersonalInformation command)
        {
            command.PatientId = patientId;
            var provider = await providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            await command.ApplyToAsync(provider);
            await providerEventSourcedRepository.Save(provider);
        }

        [Route("{PatientId}/insurance"), HttpPost]
        public async Task UpdateInsurance(Guid patientId, Domain.CareProvider.CareProvider.UpdateInsurance command)
        {
            command.PatientId = patientId;
            var provider = await providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            await command.ApplyToAsync(provider);
            await providerEventSourcedRepository.Save(provider);
        }

        [Route("{PatientId}/contact"), HttpPut]
        public async Task UpdateContactInfo(Guid patientId, Domain.CareProvider.CareProvider.UpdatePatientContactInformation command)
        {
            command.PatientId = patientId;
            var provider = await providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            await command.ApplyToAsync(provider);
            await providerEventSourcedRepository.Save(provider);
        }
    }
}
