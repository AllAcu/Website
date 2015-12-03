using System;

namespace Domain.User
{
    public static class SignupToken
    {
        public static string GenerateToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Replace("{", "").Replace("}", "");
        }
    }
}
