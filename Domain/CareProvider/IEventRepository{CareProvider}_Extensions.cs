using System;
using System.Collections.Generic;
using Microsoft.Its.Domain;

namespace Domain.CareProvider
{
    public static class IEventRepository_CareProvider__Extensions
    {
        public const string providerCookieName = "CareProviderId";

        public static readonly Guid HardCodedId = new Guid("949D90DD-8A4F-4466-B383-1A4B78468951");

        public static CareProvider CurrentProvider(this IEventSourcedRepository<CareProvider> repository, IDictionary<string, object> properties)
        {
            var id = properties[providerCookieName] != null ? Guid.Parse(properties[providerCookieName].ToString()) : HardCodedId;
            return repository.GetLatest(id);
        }
    }
}
