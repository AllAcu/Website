﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AllAcu.Authentication;
using AllAcu.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [CareProviderIdFilter]
    [RoutePrefix("api")]
    [Authorize]
    public class VerificationController : ApiController
    {
        private readonly IEventSourcedRepository<Domain.Verification.InsuranceVerification> verificationEventSourcedRepository;
        private readonly AllAcuSiteDbContext dbContext;
        private readonly VerificationRepository verificationRepository;

        public VerificationController(IEventSourcedRepository<Domain.Verification.InsuranceVerification> verificationEventSourcedRepository, AllAcuSiteDbContext dbContext, VerificationRepository verificationRepository)
        {
            this.dbContext = dbContext;
            this.verificationRepository = verificationRepository;
            this.verificationEventSourcedRepository = verificationEventSourcedRepository;
        }

        [Route("{PatientId}/insurance/verification"), HttpGet]
        public IEnumerable<InsuranceVerification> GetListViewItems(Guid patientId)
        {
            return dbContext.Verifications.Where(v => v.PatientId == patientId);
        }

        [Route("insurance/verification"), HttpGet]
        public async Task<IHttpActionResult> GetAllListViewItems()
        {
            var currentProviderId = this.CurrentProviderId();
            if (currentProviderId == null)
            {
                return await Task.FromResult(NotFound());
            }
            var content = await verificationRepository.Get(Guid.Parse(User.Identity.GetUserId()));
            return Ok(content.Where(v => v.Provider.Id == currentProviderId.Value).ToArray());
        }

        [Route("insurance/verification/{VerificationId}")]
        public InsuranceVerification GetVerification(Guid verificationId)
        {
            return dbContext.Verifications.Find(verificationId);
        }

        [Route("{PatientId}/insurance/verification/request"), HttpPost]
        public Guid StartVerification(Guid patientId, Domain.Verification.InsuranceVerification.Create command)
        {
            command.AggregateId = Guid.NewGuid();
            // TODO (bremor) - still a little wonky, and should probably be generated from a patient aggregate?
            command.PatientId = patientId;
            var verification = new Domain.Verification.InsuranceVerification(command);
            verificationEventSourcedRepository.Save(verification);

            return verification.Id;
        }

        [Route("{PatientId}/insurance/verification/submit"), HttpPost]
        public async Task CreateAndSubmitVerificationRequest(Guid patientId, Domain.Verification.InsuranceVerification.SubmitRequest command)
        {
            // kind of hacky
            var createCommmand = new Domain.Verification.InsuranceVerification.Create
            {
                AggregateId = Guid.NewGuid(),
                PatientId = patientId,
                RequestDraft = command.Request
            };

            var verification = new Domain.Verification.InsuranceVerification(createCommmand);

            await command.ApplyToAsync(verification);
            await verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/request"), HttpPut]
        public async Task UpdateVerificationRequest(Guid verificationId, Domain.Verification.InsuranceVerification.UpdateRequestDraft command)
        {
            var verification = await verificationEventSourcedRepository.GetLatest(verificationId);
            await command.ApplyToAsync(verification);
            await verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}"), HttpPut]
        public async Task UpdateVerification(Guid verificationId, Domain.Verification.InsuranceVerification.Update command)
        {
            var verification = await verificationEventSourcedRepository.GetLatest(verificationId);
            await command.ApplyToAsync(verification);
            await verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/submit"), HttpPost]
        public async Task SubmitVerificationRequest(Guid verificationId, Domain.Verification.InsuranceVerification.SubmitRequest command)
        {
            var verification = await verificationEventSourcedRepository.GetLatest(verificationId);
            await command.ApplyToAsync(verification);
            await verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/approve"), HttpPost]
        public async Task ApproveVerification(Guid verificationId, Domain.Verification.InsuranceVerification.StartCall command)
        {
            var verification = await verificationEventSourcedRepository.GetLatest(verificationId);
            await command.ApplyToAsync(verification);
            await verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/revise"), HttpPost]
        public async Task ReviseVerification(Guid verificationId, Domain.Verification.InsuranceVerification.RejectRequest command)
        {
            var verification = await verificationEventSourcedRepository.GetLatest(verificationId);
            await command.ApplyToAsync(verification);
            await verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/assign"), HttpPost]
        public async Task AssignVerification(Guid verificationId, Domain.Verification.InsuranceVerification.DelegateRequest command)
        {
            var verification = await verificationEventSourcedRepository.GetLatest(verificationId);
            await command.ApplyToAsync(verification);
            await verificationEventSourcedRepository.Save(verification);
        }
    }
}
