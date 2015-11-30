using System;
using System.Collections.Generic;
using Domain.Authentication;
using Its.Validation;
using Its.Validation.Configuration;
using Microsoft.Its.Domain;

namespace Domain.Biller
{
    public partial class Biller
    {
        public class RevokeRoles : Command<Biller>
        {
            public Guid UserId { get; set; }
            public IList<Role> Roles { get; set; }

            public override IValidationRule<Biller> Validator
            {
                get
                {
                    return Validate.That<Biller>(p =>
                        p.GetUser(UserId).Roles.IsSupersetOf(Roles));
                }
            }
        }
    }
}
