using System;
using Microsoft.AspNetCore.Authorization;

namespace EPlast.WebApi.CustomAttributes
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles) : base()
        {
            Roles = String.Join(',', roles);
        }
    }
}
