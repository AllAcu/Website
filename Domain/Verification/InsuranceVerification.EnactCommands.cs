using System;
using System.Threading.Tasks;
using Microsoft.Its.Domain;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Domain.Verification
{
    public partial class InsuranceVerification
    {
        public class VerificationCommandHandler :
            ICommandHandler<InsuranceVerification, Complete>,
            ICommandHandler<InsuranceVerification, DelegateRequest>,
            ICommandHandler<InsuranceVerification, EndCall>,
            ICommandHandler<InsuranceVerification, RejectRequest>,
            ICommandHandler<InsuranceVerification, StartCall>,
            ICommandHandler<InsuranceVerification, SubmitForApproval>,
            ICommandHandler<InsuranceVerification, SubmitRequest>,
            ICommandHandler<InsuranceVerification, Update>,
            ICommandHandler<InsuranceVerification, UpdateRequestDraft>
        {
            public async Task EnactCommand(InsuranceVerification verification, Complete command)
            {
                verification.RecordEvent(new Completed());
            }

            public async Task EnactCommand(InsuranceVerification verification, DelegateRequest command)
            {
                verification.RecordEvent(new Delegated
                {
                    UserId = command.AssignedToUserId,
                    Comments = command.Comments
                });
            }

            public async Task EnactCommand(InsuranceVerification verification, EndCall command)
            {
                verification.RecordEvent(new CallEnded());
            }

            public async Task EnactCommand(InsuranceVerification verification, RejectRequest command)
            {
                verification.RecordEvent(new RequestRejected
                {
                    Reason = command.Comments
                });
            }

            public async Task EnactCommand(InsuranceVerification verification, StartCall command)
            {
                if (command.Benefits != null)
                {
                    verification.RecordEvent(new Updated
                    {
                        Benefits = command.Benefits
                    });
                }

                verification.RecordEvent(new Completed());
            }

            public async Task EnactCommand(InsuranceVerification verification, SubmitForApproval command)
            {
                verification.RecordEvent(new SubmittedForApproval());
            }

            public async Task EnactCommand(InsuranceVerification verification, SubmitRequest command)
            {
                verification.RecordEvent(new RequestSubmitted());
            }

            public async Task EnactCommand(InsuranceVerification verification, Update command)
            {
                verification.RecordEvent(new Updated
                {
                    Benefits = command.Benefits
                });
            }

            public async Task EnactCommand(InsuranceVerification verification, UpdateRequestDraft command)
            {
                verification.RecordEvent(new DraftUpdated
                {
                    Request = command.RequestDraft
                });
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification, CommandFailed<Complete> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification, CommandFailed<DelegateRequest> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification, CommandFailed<EndCall> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification, CommandFailed<RejectRequest> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification, CommandFailed<StartCall> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification, CommandFailed<SubmitForApproval> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification, CommandFailed<SubmitRequest> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification, CommandFailed<Update> command)
            {
            }

            public async Task HandleScheduledCommandException(InsuranceVerification verification, CommandFailed<UpdateRequestDraft> command)
            {
            }
        }
    }
}

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously