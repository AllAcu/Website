using System;
using System.Threading.Tasks;
using Microsoft.Its.Domain;

namespace Domain.Biller
{
    public static class BillerEventStoreRepository_Extensions
    {
        public static Guid AllAcuBillerId;

        public static Task<Biller> GetAllAcu(this IEventSourcedRepository<Biller> repository)
        {
            return repository.GetLatest(AllAcuBillerId);
        }
    }
}
