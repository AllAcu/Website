using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Domain.Authentication;
using Domain.CareProvider;
using Domain.Repository;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [CareProviderIdFilter]
    [RoutePrefix("api/provider")]
    public class CareProviderController : ApiController
    {
        private readonly IEventSourcedRepository<CareProvider> careProviderEventRepository;
        private readonly AllAcuSiteDbContext dbContext;

        public CareProviderController(IEventSourcedRepository<CareProvider> careProviderEventRepository, AllAcuSiteDbContext dbContext)
        {
            this.careProviderEventRepository = careProviderEventRepository;
            this.dbContext = dbContext;
        }

        [Route("new"), HttpPost]
        public Guid CreateProvider(CareProvider.CreateProvider command)
        {
            command.AggregateId = Guid.NewGuid();
            var provider = new CareProvider(command);
            careProviderEventRepository.Save(provider);

            return provider.Id;
        }

        [Route("who"), HttpGet]
        public Guid? WhoIsProvider()
        {
            return careProviderEventRepository.CurrentProvider(ActionContext.ActionArguments)?.Id;
        }

        [Route(""), HttpGet]
        public IEnumerable<CareProviderBusinessInfo> GetProviders()
        {
            return dbContext.CareProviders;
        }

        [Route("be/{providerId}"), HttpGet]
        public HttpResponseMessage BeProvider(Guid providerId)
        {
            var response = Request.CreateResponse();
            var cookie = new CookieHeaderValue(CareProviderIdFilter.providerCookieName, providerId.ToString())
            {
                Expires = DateTimeOffset.Now.AddDays(1),
                Domain = Request.RequestUri.Host == "localhost" ? null : Request.RequestUri.Host,
                Path = "/"
            };

            response.Headers.AddCookies(new[] { cookie });

            return response;
        }
    }
}
