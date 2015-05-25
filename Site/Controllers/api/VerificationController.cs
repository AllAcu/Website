﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain.Authentication;
using Domain.CareProvider;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [CareProviderIdFilter]
    [RoutePrefix("api")]
    public class VerificationController : ApiController
    {
        private readonly IEventSourcedRepository<CareProvider> providerEventSourcedRepository;
        private readonly AllAcuSiteDbContext allAcuSiteDbContext;

        public VerificationController(IEventSourcedRepository<CareProvider> providerEventSourcedRepository, AllAcuSiteDbContext allAcuSiteDbContext)
        {
            this.providerEventSourcedRepository = providerEventSourcedRepository;
            this.allAcuSiteDbContext = allAcuSiteDbContext;
        }

        [Route("{PatientId}/insurance/verify"), HttpGet]
        public IEnumerable<PendingInsuranceVerificationListItemViewModel> GetListViewItems(Guid patientId)
        {
            return allAcuSiteDbContext.VerificationList.Where(v => v.PatientId == patientId);
        }

        [Route("insurance/verify"), HttpGet]
        public IEnumerable<PendingInsuranceVerificationListItemViewModel> GetAllListViewItems()
        {
            return allAcuSiteDbContext.VerificationList;
        }

        [Route("insurance/verifyRequest/{VerificationId}")]
        public PendingVerificationRequest GetVerificationRequest(Guid verificationId)
        {
            return allAcuSiteDbContext.VerificationRequestDrafts.Find(verificationId);
        }

        [Route("insurance/verification/{VerificationId}")]
        public InsuranceVerificationForm GetVerification(Guid verificationId)
        {
            return allAcuSiteDbContext.VerificationForms.Find(verificationId);
        }

        [Route("{PatientId}/insurance/verify"), HttpPost]
        public void StartVerification(Guid patientId, CareProvider.StartVerificationRequestDraft command)
        {
            // TODO (brett) - this is a little weird
            command.PatientId = patientId;
            var provider = providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            command.ApplyTo(provider);

            providerEventSourcedRepository.Save(provider);
        }

        [Route("insurance/verify"), HttpPut]
        public void UpdateVerificationRequest(CareProvider.UpdateVerificationRequestDraft command)
        {
            var provider = providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            command.ApplyTo(provider);

            providerEventSourcedRepository.Save(provider);
        }

        [Route("insurance/verify/submit"), HttpPost]
        public void SubmitVerificationRequest(CareProvider.SubmitVerificationRequest command)
        {
            var provider = providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            command.ApplyTo(provider);

            providerEventSourcedRepository.Save(provider);
        }

        [Route("insurance/verify/approve"), HttpPost]
        public void ApproveVerification(CareProvider.ApproveVerification command)
        {
            var provider = providerEventSourcedRepository.CurrentProvider(ActionContext.ActionArguments);
            command.ApplyTo(provider);

            providerEventSourcedRepository.Save(provider);
        }
    }
}
