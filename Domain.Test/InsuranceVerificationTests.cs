using System;
using System.Linq;
using Domain.CareProvider;
using Microsoft.Its.Domain;
using Xunit;

namespace Domain.Test
{
    public class InsuranceVerificationTests
    {
        public InsuranceVerificationTests()
        {
            Command<CareProvider.CareProvider>.AuthorizeDefault = (provider, command) => true;
        }

        public class Update : InsuranceVerificationTests
        {
            [Fact]
            public void WhenVerificationDraftDoesNotExist_ReturnsValidationError()
            {
                var id = Guid.NewGuid();
                var command = new CareProvider.CareProvider.UpdateVerificationRequestDraft(id,
                    new VerificationRequest());

                var provider = new CareProvider.CareProvider();
                provider.PendingVerifications.Add(new PendingVerification
                {
                    Id = Guid.NewGuid()
                });

                Assert.Contains("doesn't exist",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(provider))
                        .ValidationReport.Failures.First().Message);
            }

            [Fact]
            public void WhenVerificationDraftExists_DraftIsUpdated()
            {
                var id = Guid.NewGuid();

                var provider = new CareProvider.CareProvider();
                provider.PendingVerifications.Add(new PendingVerification
                {
                    Id = id,
                    Request = new VerificationRequest
                    {
                        Comments = "first draft"
                    }
                });

                var command = new CareProvider.CareProvider.UpdateVerificationRequestDraft(id,
                    new VerificationRequest
                    {
                        Comments = "second draft"
                    });

                command.ApplyTo(provider);

                Assert.Equal("second draft", provider.PendingVerifications.First().Request.Comments);
            }

            [Fact]
            public void WhenVerificationIsAlreadySubmitted_ReturnsValidationError()
            {
                var id = Guid.NewGuid();
                var command = new CareProvider.CareProvider.UpdateVerificationRequestDraft(id,
                    new VerificationRequest());

                var provider = new CareProvider.CareProvider();
                provider.PendingVerifications.Add(new PendingVerification
                {
                    Id = id,
                    Status = PendingVerification.RequestStatus.Submitted
                });

                Assert.Contains("submitted",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(provider))
                        .ValidationReport.Failures.First().Message);
            }
        }

        public class Start : InsuranceVerificationTests
        {
            [Fact]
            public void WhenVerificationIsStarted_BecomesPartOfProviderDraftList()
            {
                var command = new CareProvider.CareProvider.StartVerificationRequestDraft
                (
                    requestDraft: new VerificationRequest
                    {
                        Comments = "Created draft"
                    }
                );

                var provider = new CareProvider.CareProvider();
                command.ApplyTo(provider);

                Assert.Equal("Created draft", provider.PendingVerifications.Single().Request.Comments);
            }

            [Fact]
            public void WhenVerificationIsStartedWithoutRequestData_FailsCommandValidation()
            {
                var command = new CareProvider.CareProvider.StartVerificationRequestDraft
                (
                    requestDraft: null
                );

                Assert.Contains("Must supply",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(new CareProvider.CareProvider()))
                        .ValidationReport.Failures.First().Message);
            }
        }

        public class Submit : InsuranceVerificationTests
        {
            [Fact]
            public void WhenDraftIdNullAndRequestNull_FailsCommandValidation()
            {
                var command = new CareProvider.CareProvider.SubmitVerificationRequest(null, null);

                Assert.Contains("supply a request",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(new CareProvider.CareProvider()))
                        .ValidationReport.Failures.First().Message);
            }

            [Fact]
            public void WhenVerificationDoesNotExistAndRequestNotSupplied_ReturnsValidationError()
            {
                var command = new CareProvider.CareProvider.SubmitVerificationRequest(verificationId: Guid.NewGuid());

                Assert.Contains("doesn't exist",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(new CareProvider.CareProvider()))
                        .ValidationReport.Failures.First().Message);
            }

            [Fact]
            public void WhenVerificationAlreadySubmitted_ReturnsValidationError()
            {
                var id = Guid.NewGuid();
                var command = new CareProvider.CareProvider.SubmitVerificationRequest
                (
                    verificationId: id
                );
                var provider = new CareProvider.CareProvider();
                provider.PendingVerifications.Add(new PendingVerification
                {
                    Id = id,
                    Status = PendingVerification.RequestStatus.Submitted 
                });

                Assert.Contains("submitted",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(provider))
                        .ValidationReport.Failures.First().Message);
            }

            [Fact]
            public void WhenVerificationExistsAndDraftIdSupplied_MovesDraftToOutstanding()
            {
                var id = Guid.NewGuid();
                var command = new CareProvider.CareProvider.SubmitVerificationRequest
                (
                    verificationId: id
                );
                var provider = new CareProvider.CareProvider();
                provider.PendingVerifications.Add(new PendingVerification
                {
                    Id = id,
                    Request = new VerificationRequest
                    {
                        Comments = "draft comments"
                    }
                });

                command.ApplyTo(provider);

                Assert.Equal("draft comments", provider.PendingVerifications.Single().Request.Comments);
                Assert.Equal(PendingVerification.RequestStatus.Submitted, provider.PendingVerifications.Single().Status);
            }

            [Fact]
            public void WhenVerificationDoesNotExistAndOnlyRequestSupplied_SendsNewRequestToOutstanding()
            {
                var command = new CareProvider.CareProvider.SubmitVerificationRequest
                (
                    verificationRequest: new VerificationRequest
                    {
                        Comments = "request comments"
                    }
                );
                var provider = new CareProvider.CareProvider();

                command.ApplyTo(provider);

                Assert.Equal("request comments", provider.PendingVerifications.Single().Request.Comments);
                Assert.Equal(PendingVerification.RequestStatus.Submitted, provider.PendingVerifications.Single().Status);
            }

            [Fact]
            public void WhenVerificationExistsAndRequestAlsoSupplied_SendsUpdatedRequestToOutstanding()
            {
                var id = Guid.NewGuid();

                var command = new CareProvider.CareProvider.SubmitVerificationRequest
                (
                    verificationId: id,
                    verificationRequest: new VerificationRequest
                    {
                        Comments = "final request comments"
                    }
                );
                var provider = new CareProvider.CareProvider();
                provider.PendingVerifications.Add(new PendingVerification
                {
                    Id = id,
                    Request = new VerificationRequest
                    {
                        Comments = "draft comments"
                    }
                });

                command.ApplyTo(provider);

                Assert.Equal(PendingVerification.RequestStatus.Submitted, provider.PendingVerifications.Single().Status);
                Assert.Equal("final request comments", provider.PendingVerifications.Single().Request.Comments);
            }
        }
    }
}
