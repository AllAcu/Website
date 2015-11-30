﻿using System;
using System.Threading.Tasks;
using Domain.Biller;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers
{
    [Route("biller")]
    [Authorize]
    public class BillerController : Controller
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
