using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Its.Domain;

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class VerificationCommandHandler :
            ICommandHandler<InsuranceVerification, UpdateVerificationRequestDraft>,
            ICommandHandler<InsuranceVerification, SubmitVerificationRequest>,
            ICommandHandler<InsuranceVerification, UpdateVerification>,
            ICommandHandler<InsuranceVerification, ApproveVerification>,
            ICommandHandler<InsuranceVerification, ReviseVerification>
        {
            //public async Task EnactCommand(CareProvider provider, SubmitVerificationRequest command)
            //{
            //    var verificationId = command.VerificationId ?? Guid.NewGuid();
            //    if (command.VerificationId == null)
            //    {
            //        provider.RecordEvent(new VerificationDraftCreated
            //        {
            //            VerificationId = verificationId
            //        });
            //    }

            //    if (command.VerificationRequest != null)
            //    {
            //        provider.RecordEvent(new VerificationDraftUpdated
            //        {
            //            VerificationId = verificationId,
            //            Request = command.VerificationRequest
            //        });
            //    }

            //    provider.RecordEvent(new VerificationRequestSubmitted
            //    {
            //        VerificationId = verificationId
            //    });
            //}

            public async Task EnactCommand(InsuranceVerification verification, UpdateVerificationRequestDraft command)
            {
                verification.RecordEvent(new VerificationDraftUpdated
                {
                    VerificationId = command.VerificationId,
                    Request = command.RequestDraft
                });
            }

            public async Task EnactCommand(InsuranceVerification verification, UpdateVerification command)
            {
                verification.RecordEvent(new VerificationUpdated
                {
                    VerificationId = command.VerificationId,
                    Benefits = command.Benefits
                });
            }

            public async Task EnactCommand(InsuranceVerification verification, ApproveVerification command)
            {
                if (command.Benefits != null)
                {
                    verification.RecordEvent(new VerificationUpdated
                    {
                        VerificationId = command.VerificationId,
                        Benefits = command.Benefits
                    });
                }

                verification.RecordEvent(new VerificationApproved
                {
                    VerificationId = command.VerificationId
                });
            }

            public async Task EnactCommand(InsuranceVerification verification, ReviseVerification command)
            {
                verification.RecordEvent(new VerificationRevised
                {
                    VerificationId = command.VerificationId,
                    Reason = command.Reason
                });
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification,
                CommandFailed<UpdateVerificationRequestDraft> command)
            {
            }

            public async Task EnactCommand(InsuranceVerification verification, SubmitVerificationRequest command)
            {
                var verificationId = Guid.NewGuid();
                // terminate old one if it's there


                //verification.RecordEvent(new VerificationStarted
                //{
                //    PatientId = command.PatientId,
                //    VerificationId = verificationId
                //});

                //verification.RecordEvent(new VerificationDraftUpdated
                //{
                //    VerificationId = verificationId,
                //    Request = command.RequestDraft
                //});
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification,
                CommandFailed<SubmitVerificationRequest> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification,
                CommandFailed<UpdateVerification> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification,
                CommandFailed<ApproveVerification> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification,
                CommandFailed<ReviseVerification> command)
            {
            }
        }

    }

    public partial class InsuranceVerification
    {
        //public class VerificationCommandHandler :
        //    ICommandHandler<InsuranceVerification.SubmitVerification>
        //{
        //    public async Task EnactCommand(InsuranceVerification verification, InsuranceVerification.SubmitVerification command)
        //    {
        //        var verificationId = Guid.NewGuid();
        //        // terminate old one if it's there


        //        provider.RecordEvent(new InsuranceVerification.VerificationStarted
        //        {
        //            PatientId = command.PatientId,
        //            VerificationId = verificationId
        //        });

        //        provider.RecordEvent(new CareProvider.CareProvider.VerificationDraftUpdated
        //        {
        //            VerificationId = verificationId,
        //            Request = command.RequestDraft
        //        });
        //    }


        //}


    }
}
