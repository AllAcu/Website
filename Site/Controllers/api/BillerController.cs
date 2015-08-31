using System;
using System.Threading.Tasks;
using System.Web.Http;
using Domain.Biller;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [RoutePrefix("api/biller")]
    [Authorize]
    public class BillerController : ApiController
    {
        public static Guid AllAcuBillerId { get; set; }

        private readonly AllAcuSiteDbContext dbContext;
        private readonly IEventSourcedRepository<Domain.Biller.Biller> billerEventSourcedRepository;

        public BillerController(AllAcuSiteDbContext dbContext, IEventSourcedRepository<Domain.Biller.Biller> billerEventSourcedRepository)
        {
            this.dbContext = dbContext;
            this.billerEventSourcedRepository = billerEventSourcedRepository;
        }

        [Route(""), HttpGet]
        public Task<Biller> GetBiller()
        {
            return dbContext.Billers.FindAsync(AllAcuBillerId);
        }

        [Route("grant")]
        public async Task GrantUserRoles(Domain.Biller.Biller.GrantRoles command)
        {
            var biller = await billerEventSourcedRepository.GetAllAcu();
            await command.ApplyToAsync(biller);
            await billerEventSourcedRepository.Save(biller);
        }

        [Route("revoke")]
        public async Task RevokeUserRoles(Domain.Biller.Biller.RevokeRoles command)
        {
            var biller = await billerEventSourcedRepository.GetAllAcu();
            await command.ApplyToAsync(biller);
            await billerEventSourcedRepository.Save(biller);
        }
    }
}
