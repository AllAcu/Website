using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Domain;
using Domain.Biller;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [RoutePrefix("api/biller")]
    //[Authorize]
    public class BillerController : ApiController
    {
        public static Guid AllAcuBillerId { get; set; }

        private readonly AllAcuSiteDbContext dbContext;
        private readonly IEventSourcedRepository<Biller> billerEventSourcedRepository; 

        public BillerController(AllAcuSiteDbContext dbContext, IEventSourcedRepository<Biller> billerEventSourcedRepository)
        {
            this.dbContext = dbContext;
            this.billerEventSourcedRepository = billerEventSourcedRepository;
        }

        [Route(""), HttpGet]
        public Task<BillerDetails> GetProvider()
        {
            return dbContext.Billers.FindAsync(AllAcuBillerId);
        }

        [Route("add"), HttpPost]
        public async Task JoinProvider(Guid providerId, Biller.AddUser command)
        {
            var provider = await billerEventSourcedRepository.GetLatest(providerId);
            await command.ApplyToAsync(provider);
            await billerEventSourcedRepository.Save(provider);
        }

        [Route("remove"), HttpPost]
        public async Task LeaveProvider(Guid providerId, Biller.RemoveUser command)
        {
            var provider = await billerEventSourcedRepository.GetLatest(providerId);
            await command.ApplyToAsync(provider);
            await billerEventSourcedRepository.Save(provider);
        }

        [Route("grant")]
        public async Task GrantUserRoles(Guid providerId, Biller.GrantRoles command)
        {
            var provider = await billerEventSourcedRepository.GetLatest(providerId);
            await command.ApplyToAsync(provider);
            await billerEventSourcedRepository.Save(provider);
        }

        [Route("revoke")]
        public async Task RevokeUserRoles(Guid providerId, Biller.RevokeRoles command)
        {
            var provider = await billerEventSourcedRepository.GetLatest(providerId);
            await command.ApplyToAsync(provider);
            await billerEventSourcedRepository.Save(provider);
        }
    }
}
