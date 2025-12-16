namespace DogShelterMvc.Helpers
{
    public static class PermissionHelper
    {
        public static ulong GetUserPerms(HttpContext context)
        {
            var permsString = context.Session.GetString("Perms");
            return ulong.TryParse(permsString, out var perms) ? perms : 0;
        }

        public static bool HasPermission(HttpContext context, ulong requiredPerms)
        {
            return GetUserPerms(context) >= requiredPerms;
        }

        public static bool IsAdmin(HttpContext context)
        {
            return GetUserPerms(context) >= 100; // Admin má oprávnění >= 100
        }

        public static bool CanEdit(HttpContext context)
        {
            return GetUserPerms(context) >= 1; // Minimální oprávnění pro editaci
        }

        public static bool CanDelete(HttpContext context)
        {
            return GetUserPerms(context) >= 10; // Vyšší oprávnění pro mazání
        }
    }
}

