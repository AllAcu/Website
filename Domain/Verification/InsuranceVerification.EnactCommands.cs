using System.Threading.Tasks;
using Microsoft.Its.Domain;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class VerificationCommandHandler :
            ICommandHandler<InsuranceVerification, UpdateRequestDraft>,
            ICommandHandler<InsuranceVerification, SubmitRequest>,
            ICommandHandler<InsuranceVerification, Update>,
            ICommandHandler<InsuranceVerification, Assign>,
            ICommandHandler<InsuranceVerification, VerifyBenefits>,
            ICommandHandler<InsuranceVerification, ReturnToProvider>
        {
            public async Task EnactCommand(InsuranceVerification verification, UpdateRequestDraft command)
            {
                verification.RecordEvent(new DraftUpdated
                {
                    Request = command.RequestDraft
                });
            }

            public async Task EnactCommand(InsuranceVerification verification, Update command)
            {
                verification.RecordEvent(new Updated
                {
                    Benefits = command.Benefits
                });
            }

            public async Task EnactCommand(InsuranceVerification verification, VerifyBenefits command)
            {
                if (command.Benefits != null)
                {
                    verification.RecordEvent(new Updated
                    {
                        Benefits = command.Benefits
                    });
                }

                verification.RecordEvent(new Approved
                {
                });
            }

            public async Task EnactCommand(InsuranceVerification verification, ReturnToProvider command)
            {
                verification.RecordEvent(new Rejected
                {
                    Reason = command.Reason
                });
            }

            public async Task EnactCommand(InsuranceVerification verification, SubmitRequest command)
            {
                verification.RecordEvent(new RequestSubmitted
                {
                    
                });
            }

            public async Task EnactCommand(InsuranceVerification verification, Assign command)
            {
                verification.RecordEvent(new Assigned
                {
                    UserId = command.UserId,
                    Comments = command.Comments
                });
            }


            public async Task HandleScheduledCommandException(InsuranceVerification verification,
                CommandFailed<UpdateRequestDraft> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification,
                CommandFailed<SubmitRequest> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification,
                CommandFailed<Update> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification,
                CommandFailed<VerifyBenefits> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification,
                CommandFailed<ReturnToProvider> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification aggregate,
                CommandFailed<Assign> command)
            {
            }
        }
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
