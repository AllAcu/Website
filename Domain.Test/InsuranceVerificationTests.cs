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
                var draftId = Guid.NewGuid();
                var command = new CareProvider.CareProvider.UpdateVerificationRequestDraft(draftId,
                    new VerificationRequest());

                var provider = new CareProvider.CareProvider();
                provider.VerificationRequestDrafts.Add(new CareProvider.CareProvider.VerificationRequestDraft
                {
                    DraftId = Guid.NewGuid()
                });

                Assert.Contains("doesn't exist",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(provider))
                        .ValidationReport.Failures.First().Message);
            }

            [Fact]
            public void WhenVerificationDraftExists_DraftIsUpdated()
            {
                var draftId = Guid.NewGuid();

                var provider = new CareProvider.CareProvider();
                provider.VerificationRequestDrafts.Add(new CareProvider.CareProvider.VerificationRequestDraft
                {
                    DraftId = draftId,
                    Request = new VerificationRequest
                    {
                        Comment = "first draft"
                    }
                });

                var command = new CareProvider.CareProvider.UpdateVerificationRequestDraft(draftId,
                    new VerificationRequest
                    {
                        Comment = "second draft"
                    });

                command.ApplyTo(provider);

                Assert.Equal("second draft", provider.VerificationRequestDrafts.First().Request.Comment);
            }

            [Fact]
            public void WhenVerificationIsOutstanding_ReturnsValidationError()
            {
                var draftId = Guid.NewGuid();
                var command = new CareProvider.CareProvider.UpdateVerificationRequestDraft(draftId,
                    new VerificationRequest());

                var provider = new CareProvider.CareProvider();
                provider.OutstandingVerifications.Add(new CareProvider.CareProvider.OutstandingVerification
                {
                    RequestId = draftId
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
                {
                    RequestDraft = new VerificationRequest
                    {
                        Comment = "Created draft"
                    }
                };

                var provider = new CareProvider.CareProvider();
                command.ApplyTo(provider);

                Assert.Equal("Created draft", provider.VerificationRequestDrafts.Single().Request.Comment);
            }

            [Fact]
            public void WhenVerificationIsStartedWithoutRequestData_FailsCommandValidation()
            {
                var command = new CareProvider.CareProvider.StartVerificationRequestDraft
                {
                    RequestDraft = null
                };

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
                var command = new CareProvider.CareProvider.SubmitVerificationRequest
                {
                    DraftId = null,
                    VerificationRequest = null
                };

                Assert.Contains("supply a request",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(new CareProvider.CareProvider()))
                        .ValidationReport.Failures.First().Message);
            }

            [Fact]
            public void WhenVerificationDoesNotExistAndRequestNotSupplied_ReturnsValidationError()
            {
                var command = new CareProvider.CareProvider.SubmitVerificationRequest
                {
                    DraftId = Guid.NewGuid()
                };

                Assert.Contains("doesn't exist",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(new CareProvider.CareProvider()))
                        .ValidationReport.Failures.First().Message);
            }

            [Fact]
            public void WhenVerificationAlreadySubmitted_ReturnsValidationError()
            {
                var draftId = Guid.NewGuid();
                var command = new CareProvider.CareProvider.SubmitVerificationRequest
                {
                    DraftId = draftId
                };
                var provider = new CareProvider.CareProvider();
                provider.OutstandingVerifications.Add(new CareProvider.CareProvider.OutstandingVerification
                {
                    RequestId = draftId
                });

                Assert.Contains("submitted",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(provider))
                        .ValidationReport.Failures.First().Message);
            }

            [Fact]
            public void WhenVerificationExistsAndDraftIdSupplied_MovesDraftToOutstanding()
            {
                var draftId = Guid.NewGuid();
                var command = new CareProvider.CareProvider.SubmitVerificationRequest
                {
                    DraftId = draftId
                };
                var provider = new CareProvider.CareProvider();
                provider.VerificationRequestDrafts.Add(new CareProvider.CareProvider.VerificationRequestDraft
                {
                    DraftId = draftId,
                    Request = new VerificationRequest
                    {
                        Comment = "draft comment"
                    }
                });

                command.ApplyTo(provider);

                Assert.Equal("draft comment", provider.OutstandingVerifications.Single().Request.Comment);
            }

            [Fact]
            public void WhenVerificationDoesNotExistAndOnlyRequestSupplied_SendsNewRequestToOutstanding()
            {
                var command = new CareProvider.CareProvider.SubmitVerificationRequest
                {
                    VerificationRequest = new VerificationRequest
                    {
                        Comment = "request comment"
                    }
                };
                var provider = new CareProvider.CareProvider();

                command.ApplyTo(provider);

                Assert.Equal("request comment", provider.OutstandingVerifications.Single().Request.Comment);
            }

            [Fact]
            public void WhenVerificationExistsAndRequestAlsoSupplied_SendsUpdatedRequestToOutstanding()
            {
                var draftId = Guid.NewGuid();

                var command = new CareProvider.CareProvider.SubmitVerificationRequest
                {
                    DraftId = draftId,
                    VerificationRequest = new VerificationRequest
                    {
                        Comment = "final request comment"
                    }
                };
                var provider = new CareProvider.CareProvider();
                provider.VerificationRequestDrafts.Add(new CareProvider.CareProvider.VerificationRequestDraft
                {
                    DraftId = draftId,
                    Request = new VerificationRequest
                    {
                        Comment = "draft comment"
                    }
                });

                command.ApplyTo(provider);

                Assert.Equal("final request comment", provider.OutstandingVerifications.Single().Request.Comment);
            }
        }
    }
}
