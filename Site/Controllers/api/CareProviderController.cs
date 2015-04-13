using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [RoutePrefix("api/provider")]
    public class CareProviderController : ApiController
    {
        private readonly IEventSourcedRepository<CareProvider> careProviderEventRepository;

        public CareProviderController(IEventSourcedRepository<CareProvider> careProviderEventRepository)
        {
            this.careProviderEventRepository = careProviderEventRepository;
        }

        [Route("new-laksdjflsajdfiosadfj"), HttpGet]
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

        [Route(""), HttpGet]
        public IEnumerable<CareProvider> GetProviders()
        {
            return new[]
            {
                careProviderEventRepository.GetLatest(CareProvider.HardCodedId)
            };
        }
    }
}
