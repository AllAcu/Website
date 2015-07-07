using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AllAcu.Authentication;
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

        [Route("insurance/verification"), HttpGet]
        public IEnumerable<PendingInsuranceVerificationListItemViewModel> GetAllListViewItems()
        {
            return allAcuSiteDbContext.VerificationList;
        }

        [Authorize]
        [Route("insurance/verification2"), HttpGet]
        public IEnumerable<PendingInsuranceVerificationListItemViewModel> GetAllListViewItemsAuthorized()
        {
            return allAcuSiteDbContext.VerificationList;
        }

        [Route("insurance/verification/{VerificationId}")]
        public InsuranceVerification GetVerification(Guid verificationId)
        {
            return allAcuSiteDbContext.Verifications.Find(verificationId);
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
        public void CreateAndSubmitVerificationRequest(Guid patientId, Domain.Verification.InsuranceVerification.SubmitRequest submitCommand)
        {
            // kind of hacky
            var createCommmand = new Domain.Verification.InsuranceVerification.Create
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
        public void UpdateVerificationRequest(Guid verificationId, Domain.Verification.InsuranceVerification.UpdateRequestDraft command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}"), HttpPut]
        public void UpdateVerification(Guid verificationId, Domain.Verification.InsuranceVerification.Update command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/submit"), HttpPost]
        public void SubmitVerificationRequest(Guid verificationId, Domain.Verification.InsuranceVerification.SubmitRequest command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/approve"), HttpPost]
        public void ApproveVerification(Guid verificationId, Domain.Verification.InsuranceVerification.VerifyBenefits command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/revise"), HttpPost]
        public void ReviseVerification(Guid verificationId, Domain.Verification.InsuranceVerification.ReturnToProvider command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(verificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }
    }
}
