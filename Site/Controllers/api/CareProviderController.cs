using System;
using System.Web.Http;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [System.Web.Mvc.RoutePrefix("api/provider")]
    public class CareProviderController : ApiController
    {
        private readonly IEventSourcedRepository<CareProvider> careProviderEventRepository;

        public CareProviderController(IEventSourcedRepository<CareProvider> careProviderEventRepository)
        {
            this.careProviderEventRepository = careProviderEventRepository;
        }

        [Route("new"), HttpGet]
        public Guid CreateProvider()
        {
            var provider = new CareProvider(CareProvider.HardCodedId);
            provider.EnactCommand(new CareProvider.CreateProvider
            {
                PractitionerName = "Dr Philson",
                BusinessName = "Visibility Care",
                City = "Seattle"
            });

            careProviderEventRepository.Save(provider);

            return provider.Id;
        }
    }
}
