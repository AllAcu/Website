using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public static class IEventRepository_CareProvider__Extensions
    {
        public const string providerCookieName = "CareProviderId";

        public static Task<CareProvider> CurrentProvider(this IEventSourcedRepository<CareProvider> repository, IDictionary<string, object> properties)
        {
            var id = properties[providerCookieName] != null ? Guid.Parse(properties[providerCookieName].ToString()) : (Guid?)null;
            return repository.GetLatest(id ?? new Guid());
        }
    }
}
