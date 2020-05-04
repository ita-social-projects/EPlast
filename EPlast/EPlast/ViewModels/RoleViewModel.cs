using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels
{
    public class RoleViewModel
    {
        public string UserID { get; set; }
        public string UserEmail { get; set; }
        public IEnumerable<IdentityRole> AllRoles { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
        public RoleViewModel()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
