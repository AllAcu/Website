using System;
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
        private readonly IEventSourcedRepository<CareProvider> providerEventSourcedRepository;
        private readonly IEventSourcedRepository<InsuranceVerification> verificationEventSourcedRepository;
        private readonly AllAcuSiteDbContext allAcuSiteDbContext;

        public VerificationController(IEventSourcedRepository<CareProvider> providerEventSourcedRepository, IEventSourcedRepository<InsuranceVerification> verificationEventSourcedRepository, AllAcuSiteDbContext allAcuSiteDbContext)
        {
            this.providerEventSourcedRepository = providerEventSourcedRepository;
            this.allAcuSiteDbContext = allAcuSiteDbContext;
            this.verificationEventSourcedRepository = verificationEventSourcedRepository;
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

        [Route("insurance/pendingVerification/{VerificationId}")]
        public InsuranceVerificationForm GetVerification(Guid verificationId)
        {
            return allAcuSiteDbContext.VerificationForms.First(v => v.VerificationId == verificationId);
        }

        [Route("insurance/verification/{VerificationId}")]
        public CompletedVerificationDetails GetApprovedVerification(Guid verificationId)
        {
            return allAcuSiteDbContext.ApprovedVerifications.Find(verificationId);
        }

        [Route("{PatientId}/insurance/verify"), HttpPost]
        public Guid StartVerification(Guid patientId, InsuranceVerification.CreateVerification command)
        {
            command.AggregateId = Guid.NewGuid();
            command.PatientId = patientId;
            var verification = new InsuranceVerification(command);
            verificationEventSourcedRepository.Save(verification);

            return verification.Id;
        }

        [Route("insurance/verify"), HttpPut]
        public void UpdateVerificationRequest(InsuranceVerification.UpdateVerificationRequestDraft command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(command.VerificationId);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verify/submit"), HttpPost]
        public void SubmitVerificationRequest(InsuranceVerification.SubmitVerificationRequest command)
        {
            // TODO (bremor) - account for new verification with no id
            var verification = verificationEventSourcedRepository.GetLatest(command.VerificationId.Value);
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}"), HttpPut]
        public void UpdateVerification(Guid verificationId, InsuranceVerification.UpdateVerification command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(command.VerificationId);
            command.VerificationId = verificationId;
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/approve"), HttpPost]
        public void ApproveVerification(Guid verificationId, InsuranceVerification.ApproveVerification command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(command.VerificationId);
            command.VerificationId = verificationId;
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/revise"), HttpPost]
        public void ReviseVerification(Guid verificationId, InsuranceVerification.ReviseVerification command)
        {
            var verification = verificationEventSourcedRepository.GetLatest(command.VerificationId);
            command.VerificationId = verificationId;
            command.ApplyTo(verification);
            verificationEventSourcedRepository.Save(verification);
        }
    }
}
