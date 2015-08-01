namespace Domain.Authentication
{
    public static class Roles
    {
        public static class Biller
        {
            public static Role Know => new Role("know");
            public static Role Rob => new Role("rob");
            public static Role Verifier => new Role("verifier");
        }

        public static class Provider
        {
            public static Role Owner => new Role("owner");
            public static Role Practitioner => new Role("practitioner");
            public static Role OfficeAdministrator => new Role("officeAdministrator");
        }
         
    }
}