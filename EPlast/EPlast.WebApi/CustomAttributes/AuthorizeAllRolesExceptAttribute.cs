using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace EPlast.WebApi.CustomAttributes
{
    public class AuthorizeAllRolesExceptAttribute : AuthorizeAttribute
    {
        public AuthorizeAllRolesExceptAttribute(params string[] blockedRoles) : base()
        {
            Roles = string.Join(",", Resources.Roles.ListOfRoles.Where(r=>!blockedRoles.Contains(r)));
        }

    }
}
