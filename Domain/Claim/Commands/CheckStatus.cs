using System;
using Microsoft.Its.Domain;

namespace Domain
{
    public partial class Claim
    {
        public class CheckStatus : Command<Claim>
        {
            public InsuranceCall Call = new InsuranceCall();
        }
    }
}