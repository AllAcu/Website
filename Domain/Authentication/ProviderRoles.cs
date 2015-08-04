namespace Domain.Authentication
{
    public static class Roles
    {
        public static class System
        {
            public static Role Administrator => new Role("systemAdministrator");
        }
    }
}