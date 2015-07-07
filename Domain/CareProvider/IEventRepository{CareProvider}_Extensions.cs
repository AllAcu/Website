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
            var id = properties.CurrentProviderId();
            return repository.GetLatest(id ?? new Guid());
        }

        public static Guid? CurrentProviderId(this IDictionary<string, object> properties)
        {
            return properties[providerCookieName] != null ? Guid.Parse(properties[providerCookieName].ToString()) : (Guid?)null;
        }
    }
}
