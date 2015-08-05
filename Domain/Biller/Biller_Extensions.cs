using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Authentication;
using Microsoft.Its.Domain;

namespace Domain.Biller
{
    public static class Biller_Extensions
    {
        public static UserAccess GetUser(this Biller biller, Guid id)
        {
            return biller.Users.SingleOrDefault(user => user.UserId == id);
        }

        public static Task<Biller> GetAllAcu(this IEventSourcedRepository<Biller> repository)
        {
            return repository.GetLatest(Biller.AllAcuBillerId);
        }
    }
}