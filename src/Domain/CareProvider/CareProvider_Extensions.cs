using System;
using System.Linq;
using Domain.Authentication;

namespace Domain.CareProvider
{
    public static class CareProvider_Extensions
    {
        public static UserAccess GetUser(this CareProvider provider, Guid id)
        {
            return provider.Users.SingleOrDefault(user => user.UserId == id);
        }

        public static Patient GetPatient(this CareProvider provider, Guid id)
        {
            return provider.Patients.SingleOrDefault(patient => patient.Id == id);
        }

        public static bool PatientExists(this CareProvider provider, Guid id)
        {
            return provider.GetPatient(id) != null;
        }
    }
}