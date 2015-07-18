using System;
using System.Threading.Tasks;
using System.Web.Http;
using Domain.Organization;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    public class OrganizationController : ApiController
    {
        private readonly IEventSourcedRepository<Organization> organizationEventSourcedRepository;

        [Route("organization/{organizationId}/join"), HttpPost]
        public async Task JoinProvider(Guid organizationId, Organization.WelcomeUser command)
        {
            var organization = await organizationEventSourcedRepository.GetLatest(organizationId);
            await command.ApplyToAsync(organization);
            await organizationEventSourcedRepository.Save(organization);
        }

        [Route("user/{organizationId}/leave"), HttpPost]
        public async Task LeaveProvider(Guid organizationId, Organization.DismissUser command)
        {
            var organization = await organizationEventSourcedRepository.GetLatest(organizationId);
            await command.ApplyToAsync(organization);
            await organizationEventSourcedRepository.Save(organization);
        }
    }
}
