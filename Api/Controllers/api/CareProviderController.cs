using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using AllAcu.Authentication;
using Domain.CareProvider;
using Microsoft.AspNet.Identity;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [CareProviderIdFilter]
    [RoutePrefix("provider")]
    [Authorize]
    public class CareProviderController : ApiController
    {
        private readonly IEventSourcedRepository<Domain.CareProvider.CareProvider> careProviderEventRepository;
        private readonly AllAcuSiteDbContext dbContext;

        public CareProviderController(IEventSourcedRepository<Domain.CareProvider.CareProvider> careProviderEventRepository, AllAcuSiteDbContext dbContext)
        {
            this.careProviderEventRepository = careProviderEventRepository;
            this.dbContext = dbContext;
        }

        [Route("new"), HttpPost]
        public async Task<Guid> CreateProvider(Domain.CareProvider.CareProvider.CreateProvider command)
        {
            command.AggregateId = Guid.NewGuid();
            command.CreatingUserId = Guid.Parse(User.Identity.GetUserId());
            var provider = new Domain.CareProvider.CareProvider(command);
            await careProviderEventRepository.Save(provider);

            return provider.Id;
        }

        [Route("{providerId}"), HttpPut]
        public async Task Update(Guid providerId, Domain.CareProvider.CareProvider.UpdateProvider command)
        {
            var provider = await careProviderEventRepository.GetLatest(providerId);
            await command.ApplyToAsync(provider);
            await careProviderEventRepository.Save(provider);
        }

        [Route("who"), HttpGet]
        public async Task<Guid?> WhoIsProvider()
        {
            return (await careProviderEventRepository.CurrentProvider(ActionContext.ActionArguments))?.Id;
        }

        [Route(""), HttpGet]
        public async Task<IEnumerable<CareProvider>> GetUserProviders()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var user = await dbContext.Users.FindAsync(userId);

            if (user.BillerRoles.Any())
            {
                return await dbContext.CareProviders.ToListAsync();
            }
            return user.ProviderRoles.Select(p => p.Provider).Distinct();
        }

        [Route("all"), HttpGet]
        public Task<CareProvider[]> GetProviders()
        {
            return dbContext.CareProviders.ToArrayAsync();
        }

        [Route("{providerId}"), HttpGet]
        public Task<CareProvider> GetProvider(Guid providerId)
        {
            return dbContext.CareProviders.FindAsync(providerId);
        }

        [Route("{providerId}/be"), HttpGet]
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

        [Route("{providerId}/join"), HttpPost]
        public async Task JoinProvider(Guid providerId, Domain.CareProvider.CareProvider.WelcomeUser command)
        {
            var provider = await careProviderEventRepository.GetLatest(providerId);
            await command.ApplyToAsync(provider);
            await careProviderEventRepository.Save(provider);
        }

        [Route("{providerId}/leave"), HttpPost]
        public async Task LeaveProvider(Guid providerId, Domain.CareProvider.CareProvider.DismissUser command)
        {
            var provider = await careProviderEventRepository.GetLatest(providerId);
            await command.ApplyToAsync(provider);
            await careProviderEventRepository.Save(provider);
        }

        [Route("{providerId}/grant")]
        public async Task GrantUserRoles(Guid providerId, Domain.CareProvider.CareProvider.GrantRoles command)
        {
            var provider = await careProviderEventRepository.GetLatest(providerId);
            await command.ApplyToAsync(provider);
            await careProviderEventRepository.Save(provider);
        }

        [Route("{providerId}/revoke")]
        public async Task RevokeUserRoles(Guid providerId, Domain.CareProvider.CareProvider.RevokeRoles command)
        {
            var provider = await careProviderEventRepository.GetLatest(providerId);
            await command.ApplyToAsync(provider);
            await careProviderEventRepository.Save(provider);
        }
    }
}
