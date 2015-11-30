﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AllAcu.Authentication;
using AllAcu.Repository;
using Microsoft.AspNet.Mvc;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers
{
    [CareProviderIdFilter]
    //[Authorize]
    public class VerificationController : Controller
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

        [Route("verification/test"), HttpGet]
        public object TestEndpoint()
        {
            return new
            {
                Message = "Success"
            };
        }

        [Route("{PatientId}/insurance/verification"), HttpGet]
        public IEnumerable<InsuranceVerification> GetListViewItems(Guid patientId)
        {
            return dbContext.Verifications.Where(v => v.PatientId == patientId);
        }

        [Route("insurance/verification"), HttpGet]
        public async Task<ActionResult> GetAllListViewItems()
        {
            var currentProviderId = this.CurrentProviderId();
            if (currentProviderId == null)
            {
                return await Task.FromResult(HttpNotFound());
            }
            var content = await verificationRepository.Get(Guid.Parse(User.GetUserId()));
            return Ok(content.Where(v => v.Provider.Id == currentProviderId.Value).ToArray());
        }

        [Route("insurance/verification/{VerificationId}"), HttpGet]
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

        [Route("insurance/verification/{VerificationId}/request"), HttpPost]
        public async Task AcceptCommand(Guid verificationId, Domain.Verification.InsuranceVerification.UpdateRequestDraft command)
        {
            await ApplyCommandToVerification(verificationId, command);
        }

        [Route("insurance/verification/{VerificationId}/submitRequest"), HttpPost]
        public async Task AcceptCommand(Guid verificationId, Domain.Verification.InsuranceVerification.SubmitRequest command)
        {
            await ApplyCommandToVerification(verificationId, command);
        }

        [Route("{PatientId}/insurance/verification/submitRequest"), HttpPost]
        public async Task AcceptPatientCommand(Guid patientId, Domain.Verification.InsuranceVerification.SubmitRequest command)
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

        [Route("insurance/verification/{VerificationId}/rejectRequest"), HttpPost]
        public async Task AcceptCommand(Guid verificationId, Domain.Verification.InsuranceVerification.RejectRequest command)
        {
            await ApplyCommandToVerification(verificationId, command);
        }

        [Route("insurance/verification/{VerificationId}/delegate"), HttpPost]
        public async Task AcceptCommand(Guid verificationId, Domain.Verification.InsuranceVerification.Delegate command)
        {
            await ApplyCommandToVerification(verificationId, command);
        }

        [Route("insurance/verification/{VerificationId}/startCall"), HttpPost]
        public async Task AcceptCommand(Guid verificationId, Domain.Verification.InsuranceVerification.StartCall command)
        {
            await ApplyCommandToVerification(verificationId, command);
        }

        [Route("insurance/verification/{VerificationId}"), HttpPut]
        public async Task AcceptCommand(Guid verificationId, Domain.Verification.InsuranceVerification.Update command)
        {
            await ApplyCommandToVerification(verificationId, command);
        }

        [Route("insurance/verification/{VerificationId}/endCall"), HttpPost]
        public async Task AcceptCommand(Guid verificationId, Domain.Verification.InsuranceVerification.EndCall command)
        {
            await ApplyCommandToVerification(verificationId, command);
        }

        [Route("insurance/verification/{VerificationId}/submitForApproval"), HttpPost]
        public async Task AcceptCommand(Guid verificationId, Domain.Verification.InsuranceVerification.SubmitForApproval command)
        {
            await ApplyCommandToVerification(verificationId, command);
        }

        [Route("insurance/verification/{VerificationId}/complete"), HttpPost]
        public async Task AcceptCommand(Guid verificationId, Domain.Verification.InsuranceVerification.Complete command)
        {
            await ApplyCommandToVerification(verificationId, command);
        }

        private async Task ApplyCommandToVerification<TCommand>(Guid verificationId, TCommand command)
            where TCommand : Command<Domain.Verification.InsuranceVerification>
        {
            var verification = await verificationEventSourcedRepository.GetLatest(verificationId);
            await command.ApplyToAsync(verification);
            await verificationEventSourcedRepository.Save(verification);
        }
    }
}
