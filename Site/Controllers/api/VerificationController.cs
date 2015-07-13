using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AllAcu.Authentication;
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
        public async Task ApproveVerification(Guid verificationId, Domain.Verification.InsuranceVerification.VerifyBenefits command)
        {
            var verification = await verificationEventSourcedRepository.GetLatest(verificationId);
            await command.ApplyToAsync(verification);
            await verificationEventSourcedRepository.Save(verification);
        }

        [Route("insurance/verification/{VerificationId}/revise"), HttpPost]
        public async Task ReviseVerification(Guid verificationId, Domain.Verification.InsuranceVerification.ReturnToProvider command)
        {
            var verification = await verificationEventSourcedRepository.GetLatest(verificationId);
            await command.ApplyToAsync(verification);
            await verificationEventSourcedRepository.Save(verification);
        }
    }
}
