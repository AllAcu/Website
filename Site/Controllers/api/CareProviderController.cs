using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Domain.CareProvider;
using Domain.Repository;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [RoutePrefix("api/provider")]
    public class CareProviderController : ApiController
    {
        private const string providerCookieName = "CareProviderId";
        private readonly IEventSourcedRepository<CareProvider> careProviderEventRepository;
        private readonly CareProviderReadModelDbContext dbContext;

        public CareProviderController(IEventSourcedRepository<CareProvider> careProviderEventRepository, CareProviderReadModelDbContext dbContext)
        {
            this.careProviderEventRepository = careProviderEventRepository;
            this.dbContext = dbContext;
        }

        private Guid? GetCurrentProvider()
        {
            var cookie = Request.Headers.GetCookies(providerCookieName).FirstOrDefault();

            if (cookie != null)
                return Guid.Parse(cookie[providerCookieName].Value);

            return null;
        }

        [Route("new-laksdjflsajdfiosadfj"), HttpGet]
        public Guid CreateProvider()
        {
            var command = new CareProvider.CreateProvider
            {
                PractitionerName = "Dr Philson",
                BusinessName = "Visibility Care",
                City = "Seattle"
            };
            var provider = new CareProvider(command);
            careProviderEventRepository.Save(provider);

            return provider.Id;
        }

        [Route("who"), HttpGet]
        public Guid? WhoIsProvider()
        {
            return GetCurrentProvider();
        }

        [Route(""), HttpGet]
        public IEnumerable<CareProviderInfo> GetProviders()
        {
            return dbContext.CareProviders;
        }

        [Route("be/{providerId}"), HttpGet]
        public HttpResponseMessage BeProvider(Guid providerId)
        {
            var response = new HttpResponseMessage();
            var cookie = new CookieHeaderValue(providerCookieName, providerId.ToString());
            cookie.Expires = DateTimeOffset.Now.AddDays(1);
            cookie.Domain = Request.RequestUri.Host;
            cookie.Path = "/";

            response.Headers.AddCookies(new[] { cookie });

            return response;
        }
    }
}
