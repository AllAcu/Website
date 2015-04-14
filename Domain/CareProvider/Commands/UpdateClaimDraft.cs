using System;
using System.Linq;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class UpdateClaimDraft : Command<CareProvider>
        {
            public Guid ClaimDraftId { get; set; }
            public Visit Visit { get; set; }

            public override IValidationRule<CareProvider> Validator
            {
                get { return Validate.That<CareProvider>(provider => provider.Drafts.SingleOrDefault(d => d.Id == ClaimDraftId) != null); }
            }
        }
    }
}
