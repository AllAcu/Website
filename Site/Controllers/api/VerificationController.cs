﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain.Authentication;
using Domain.CareProvider;
using Domain.Verification;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [CareProviderIdFilter]
    [RoutePrefix("api")]
    public class VerificationController : ApiController
    {
        private readonly IEventSourcedRepository<Domain.Verification.InsuranceVerification> verificationEventSourcedRepository;
        private readonly AllAcuSiteDbContext allAcuSiteDbContext;

        public VerificationController(IEventSourcedRepository<Domain.Verification.InsuranceVerification> verificationEventSourcedRepository, AllAcuSiteDbContext allAcuSiteDbContext)
        {
            this.allAcuSiteDbContext = allAcuSiteDbContext;
            this.verificationEventSourcedRepository = verificationEventSourcedRepository;
        }

        [Route("{PatientId}/insurance/verification"), HttpGet]
        public IEnumerable<PendingInsuranceVerificationListItemViewModel> GetListViewItems(Guid patientId)
        {
            return allAcuSiteDbContext.VerificationList.Where(v => v.PatientId == patientId);
        }

        //[Authorize]
        [Route("insurance/verification"), HttpGet]
        public IEnumerable<PendingInsuranceVerificationListItemViewModel> GetAllListViewItems()
        {
            return allAcuSiteDbContext.VerificationList;
        }

        [Route("insurance/verification/{VerificationId}")]
        public InsuranceVerification GetVerification(Guid verificationId)
        {
            return allAcuSiteDbContext.Verifications.Find(verificationId);
        }

        [Route("{PatientId}/insurance/verification/request"), HttpPost]
        public Guid StartVerification(Guid patientId, Domain.Verification.InsuranceVerification.CreateVerification command)
        {
            command.AggregateId = Guid.NewGuid();
            // TODO (bremor) - still a little wonky, and should probably be generated from a patient aggregate?
            command.PatientId = patientId;
            var verification = new Domain.Verification.InsuranceVerification(command);
            verificationEventSourcedRepository.Save(verification);

            return verification.Id;
        }

        [Route("{PatientId}/insurance/verification/submit"), HttpPost]
        public void CreateAndSubmitVerificationRequest(Guid patientId, Domain.Verification.InsuranceVerification.SubmitVerificationRequest submitCommand)
        {
            // kind of hacky
            var createCommmand = new Domain.Verification.InsuranceVerification.CreateVerification
            {
                AggregateId = Guid.NewGuid(),
                PatientId = patientId,
                RequestDraft = submitCommand.Request
            };

            var verification = new Domain.Verification.InsuranceVerification(createCommmand);

            submitCommand.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/request"), HttpPut]
        public void UpdateVerificationRequest(Guid verificationId, Domain.Verification.InsuranceVerification.UpdateVerificationRequestDraft command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}"), HttpPut]
        public void UpdateVerification(Guid verificationId, Domain.Verification.InsuranceVerification.UpdateVerification command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/submit"), HttpPost]
        public void SubmitVerificationRequest(Guid verificationId, Domain.Verification.InsuranceVerification.SubmitVerificationRequest command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/approve"), HttpPost]
        public void ApproveVerification(Guid verificationId, Domain.Verification.InsuranceVerification.ApproveVerification command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/revise"), HttpPost]
        public void ReviseVerification(Guid verificationId, Domain.Verification.InsuranceVerification.ReviseVerification command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }
    }
}
