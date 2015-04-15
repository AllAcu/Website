using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Domain;
using Domain.Authentication;
using Domain.CareProvider;
using Microsoft.Its.Domain;
using Newtonsoft.Json;

namespace AllAcu.Controllers.api
{
    [CareProviderIdFilter]
    [RoutePrefix("api/patient")]
    public class PatientController : ApiController
    {
        private IEventSourcedRepository<CareProvider> providerEventSourcedRepository;
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

        [Route(""), HttpPost]
        public Guid Intake(IntakeRequest request)
        {
            var command = new CareProvider.IntakePatient(request.name);

            var provider = providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            command.ApplyTo(provider);

            providerEventSourcedRepository.Save(provider);

            return command.PatientId;
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
            public string name { get; set; }
        }
    }
}
