using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using AllAcu.Authentication;
using Domain.Authentication;
using Domain.CareProvider;
using Domain.Repository;
using Microsoft.AspNet.Identity;
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
        public async Task<Guid> CreateProvider(CareProvider.CreateProvider command)
        {
            command.AggregateId = Guid.NewGuid();
            var provider = new CareProvider(command);
            await careProviderEventRepository.Save(provider);

            return provider.Id;
        }

        [Route("who"), HttpGet]
        public async Task<Guid?> WhoIsProvider()
        {
            return (await careProviderEventRepository.CurrentProvider(ActionContext.ActionArguments))?.Id;
        }

        [Route(""), HttpGet]
        public async Task<IEnumerable<CareProviderBusinessInfo>> GetProviders()
        {
            //var user = await dbContext.UserDetails.FindAsync(Guid.Parse(User.Identity.GetUserId()));
            return dbContext.CareProviders; //.Where(p => user.Providers.Contains(p.Id));
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
