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
        private readonly IEventSourcedRepository<InsuranceVerification> verificationEventSourcedRepository;
        private readonly AllAcuSiteDbContext allAcuSiteDbContext;

        public VerificationController(IEventSourcedRepository<InsuranceVerification> verificationEventSourcedRepository, AllAcuSiteDbContext allAcuSiteDbContext)
        {
            this.allAcuSiteDbContext = allAcuSiteDbContext;
            this.verificationEventSourcedRepository = verificationEventSourcedRepository;
        }

        [Route("{PatientId}/insurance/verify"), HttpGet]
        public IEnumerable<PendingInsuranceVerificationListItemViewModel> GetListViewItems(Guid patientId)
        {
            return allAcuSiteDbContext.VerificationList.Where(v => v.PatientId == patientId);
        }

        //[Authorize]
        [Route("insurance/verify"), HttpGet]
        public IEnumerable<PendingInsuranceVerificationListItemViewModel> GetAllListViewItems()
        {
            return allAcuSiteDbContext.VerificationList;
        }

        [Route("insurance/verifyRequest/{VerificationId}")]
        public InsuranceVerificationForm GetVerificationRequest(Guid verificationId)
        {
            return allAcuSiteDbContext.VerificationForms.Find(verificationId);
        }

        [Route("{PatientId}/insurance/verifyRequest"), HttpPost]
        public Guid StartVerification(Guid patientId, InsuranceVerification.CreateVerification command)
        {
            command.AggregateId = Guid.NewGuid();
            // TODO (bremor) - still a little wonky, and should probably be generated from a patient aggregate?
            command.PatientId = patientId;
            var verification = new InsuranceVerification(command);
            verificationEventSourcedRepository.Save(verification);

            return verification.Id;
        }

        [Route("{PatientId}/insurance/verifyRequest/submit"), HttpPost]
        public void CreateAndSubmitVerificationRequest(Guid patientId, InsuranceVerification.SubmitVerificationRequest submitCommand)
        {
            // kind of hacky
            var createCommmand = new InsuranceVerification.CreateVerification
            {
                AggregateId = Guid.NewGuid(),
                PatientId = patientId,
                RequestDraft = submitCommand.Request
            };

            var verification = new InsuranceVerification(createCommmand);

            submitCommand.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verifyRequest/{VerificationId}"), HttpPut]
        public void UpdateVerificationRequest(Guid verificationId, InsuranceVerification.UpdateVerificationRequestDraft command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verifyRequest/{VerificationId}/submit"), HttpPost]
        public void SubmitVerificationRequest(Guid verificationId, InsuranceVerification.SubmitVerificationRequest command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}"), HttpPut]
        public void UpdateVerification(Guid verificationId, InsuranceVerification.UpdateVerification command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/approve"), HttpPost]
        public void ApproveVerification(Guid verificationId, InsuranceVerification.ApproveVerification command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/revise"), HttpPost]
        public void ReviseVerification(Guid verificationId, InsuranceVerification.ReviseVerification command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}")]
        public CompletedVerificationDetails GetApprovedVerification(Guid verificationId)
        {
            return allAcuSiteDbContext.ApprovedVerifications.Find(verificationId);
        }
    }
}
