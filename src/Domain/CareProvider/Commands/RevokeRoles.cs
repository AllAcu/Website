using System;
using System.Collections.Generic;
using Domain.Authentication;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public partial class CareProvider
    {
        public class RevokeRoles : Command<CareProvider>
        {
            public Guid UserId { get; set; }
            public IList<Role> Roles { get; set; }

            public override IValidationRule<CareProvider> Validator
            {
                get
                {
                    return Validate.That<CareProvider>(p =>
                        p.GetUser(UserId).Roles.IsSupersetOf(Roles));
                }
            }
        }
    }
}
