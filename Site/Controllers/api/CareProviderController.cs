﻿using System;
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
        private readonly CareProviderReadModelDbContext dbContext;

        public CareProviderController(IEventSourcedRepository<CareProvider> careProviderEventRepository, CareProviderReadModelDbContext dbContext)
        {
            this.careProviderEventRepository = careProviderEventRepository;
            this.dbContext = dbContext;
        }

        [Route("new"), HttpGet]
        public Guid CreateProvider(string businessName, string practitionerName)
        {
            var command = new CareProvider.CreateProvider
            {
                AggregateId = Guid.NewGuid(),
                PractitionerName = practitionerName,
                BusinessName = businessName,
                City = "Seattle",
            };
            var provider = new CareProvider(command);
            careProviderEventRepository.Save(provider);

            return provider.Id;
        }

        [Route("who"), HttpGet]
        public Guid? WhoIsProvider()
        {
            return careProviderEventRepository.CurrentProvider(ActionContext.ActionArguments).Id;
        }

        [Route(""), HttpGet]
        public IEnumerable<CareProviderInfo> GetProviders()
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
