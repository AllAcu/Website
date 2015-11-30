using System;
using System.Linq;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class DismissUser : Command<CareProvider>
        {
            public Guid UserId { get; set; }

            public override IValidationRule<CareProvider> Validator
            {
                get
                {
                    return Validate.That<CareProvider>(p => p.Users.Any(u => u.UserId == UserId));
                }
            }
        }
    }
}
