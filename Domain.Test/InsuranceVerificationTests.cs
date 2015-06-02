using System;
using System.Linq;
using Domain.CareProvider;
using Domain.Verification;
using Microsoft.Its.Domain;
using Xunit;

namespace Domain.Test
{
    public class InsuranceVerificationTests
    {
        public InsuranceVerificationTests()
        {
            Command<CareProvider.CareProvider>.AuthorizeDefault = (provider, command) => true;
            Command<InsuranceVerification>.AuthorizeDefault = (provider, command) => true;
        }

        public class Update : InsuranceVerificationTests
        {
            [Fact]
            public void WhenVerificationDraftDoesNotExist_ReturnsValidationError()
            {
                var id = Guid.NewGuid();
                var command = new InsuranceVerification.UpdateVerificationRequestDraft(id,
                    new VerificationRequest());

                var verification = new InsuranceVerification();
                verification.PendingVerifications.Add(new PendingVerification
                {
                    Id = Guid.NewGuid()
                });

                Assert.Contains("doesn't exist",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(verification))
                        .ValidationReport.Failures.First().Message);
            }

            [Fact]
            public void WhenVerificationDraftExists_DraftIsUpdated()
            {
                var id = Guid.NewGuid();

                var verification = new InsuranceVerification();
                verification.PendingVerifications.Add(new PendingVerification
                {
                    Id = id,
                    Request = new VerificationRequest
                    {
                        Comments = "first draft"
                    }
                });

                var command = new InsuranceVerification.UpdateVerificationRequestDraft(id,
                    new VerificationRequest
                    {
                        Comments = "second draft"
                    });

                command.ApplyTo(verification);

                Assert.Equal("second draft", verification.PendingVerifications.First().Request.Comments);
            }

            [Fact]
            public void WhenVerificationIsAlreadySubmitted_ReturnsValidationError()
            {
                var id = Guid.NewGuid();
                var command = new InsuranceVerification.UpdateVerificationRequestDraft(id,
                    new VerificationRequest());

                var verification = new InsuranceVerification();
                verification.PendingVerifications.Add(new PendingVerification
                {
                    Id = id,
                    Status = PendingVerification.RequestStatus.Submitted
                });

                Assert.Contains("submitted",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(verification))
                        .ValidationReport.Failures.First().Message);
            }
        }

        public class Start : InsuranceVerificationTests
        {
            [Fact]
            public void WhenVerificationIsStarted_BecomesPartOfProviderDraftList()
            {
                var patientId = Guid.NewGuid();
                var command = new InsuranceVerification.StartVerificationRequestDraft
                (
                    requestDraft: new VerificationRequest
                    {
                        Comments = "Created draft"
                    }
                )
                {
                    PatientId = patientId
                };

                var verification = new InsuranceVerification();
                verification.Patients.Add(new Patient(patientId)
                {
                    InsurancePolicies = new InsurancePolicy[] { new InsurancePolicy<MedicalInsurance>(new MedicalInsurance()) }
                });
                command.ApplyTo(verification);

                Assert.Equal("Created draft", verification.PendingVerifications.Single().Request.Comments);
            }

            [Fact]
            public void WhenVerificationIsStartedWithoutRequestData_FailsCommandValidation()
            {
                var command = new InsuranceVerification.CreateVerification();

                Assert.Contains("Must supply",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(new InsuranceVerification()))
                        .ValidationReport.Failures.First().Message);
            }
        }

        public class Submit : InsuranceVerificationTests
        {
            [Fact]
            public void WhenDraftIdNullAndRequestNull_FailsCommandValidation()
            {
                var command = new InsuranceVerification.SubmitVerificationRequest(null, null);

                Assert.Contains("supply a request",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(new Careverification.Careverification.CareProvider()))
                        .ValidationReport.Failures.First().Message);
            }

            [Fact]
            public void WhenVerificationDoesNotExistAndRequestNotSupplied_ReturnsValidationError()
            {
                var command = new InsuranceVerification.SubmitVerificationRequest(verificationId: Guid.NewGuid());

                Assert.Contains("doesn't exist",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(new Careverification.Careverification.CareProvider()))
                        .ValidationReport.Failures.First().Message);
            }

            [Fact]
            public void WhenVerificationAlreadySubmitted_ReturnsValidationError()
            {
                var id = Guid.NewGuid();
                var command = new InsuranceVerification.SubmitVerificationRequest
                (
                    verificationId: id
                );
                var verification = new InsuranceVerification();
                verification.PendingVerifications.Add(new PendingVerification
                {
                    Id = id,
                    Status = PendingVerification.RequestStatus.Submitted 
                });

                Assert.Contains("submitted",
                    Assert.Throws<CommandValidationException>(() => command.ApplyTo(verification))
                        .ValidationReport.Failures.First().Message);
            }

            [Fact]
            public void WhenVerificationExistsAndDraftIdSupplied_MovesDraftToOutstanding()
            {
                var id = Guid.NewGuid();
                var command = new InsuranceVerification.SubmitVerificationRequest
                (
                    verificationId: id
                );
                var verification = new InsuranceVerification();
                verification.PendingVerifications.Add(new PendingVerification
                {
                    Id = id,
                    Request = new VerificationRequest
                    {
                        Comments = "draft comments"
                    }
                });

                command.ApplyTo(verification);

                Assert.Equal("draft comments", verification.PendingVerifications.Single().Request.Comments);
                Assert.Equal(PendingVerification.RequestStatus.Submitted, verification.PendingVerifications.Single().Status);
            }

            [Fact]
            public void WhenVerificationDoesNotExistAndOnlyRequestSupplied_SendsNewRequestToOutstanding()
            {
                var command = new InsuranceVerification.SubmitVerificationRequest
                (
                    verificationRequest: new VerificationRequest
                    {
                        Comments = "request comments"
                    }
                );
                var verification = new InsuranceVerification();

                command.ApplyTo(verification);

                Assert.Equal("request comments", verification.PendingVerifications.Single().Request.Comments);
                Assert.Equal(PendingVerification.RequestStatus.Submitted, verification.PendingVerifications.Single().Status);
            }

            [Fact]
            public void WhenVerificationExistsAndRequestAlsoSupplied_SendsUpdatedRequestToOutstanding()
            {
                var id = Guid.NewGuid();

                var command = new InsuranceVerification.SubmitVerificationRequest
                (
                    verificationId: id,
                    verificationRequest: new VerificationRequest
                    {
                        Comments = "final request comments"
                    }
                );
                var verification = new InsuranceVerification();
                verification.PendingVerifications.Add(new PendingVerification
                {
                    Id = id,
                    Request = new VerificationRequest
                    {
                        Comments = "draft comments"
                    }
                });

                command.ApplyTo(verification);

                Assert.Equal(PendingVerification.RequestStatus.Submitted, verification.PendingVerifications.Single().Status);
                Assert.Equal("final request comments", verification.PendingVerifications.Single().Request.Comments);
            }
        }
    }
}
