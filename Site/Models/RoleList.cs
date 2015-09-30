using System;
using System.Linq;
using Domain.Authentication;

namespace AllAcu
{
    public class RoleList : SerialList<string>
    {

    }

    public static class RoleListExtensions
    {
        private static string[] BillerRoles = new [] { "approver", "verifier" };
        public static bool IsInRole(this RoleList roles, string role)
        {
            return roles.Contains(role, StringComparer.OrdinalIgnoreCase);
        }

        public static bool IsInRole(this RoleList roles, Role role)
        {
            return IsInRole(roles, role.ToString());
        }

        public static bool IsBiller(this RoleList roles)
        {
            return roles.Intersect(BillerRoles).Any();
        }
    }

}