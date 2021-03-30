using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Repositories.Realizations.Blank;

namespace EPlast.BLL.SecurityModel
{
    public class SecurityModel : ISecurityModel
    {
        private Dictionary<string, Dictionary<string, bool>> accessDictionary;
        private IUserManagerService _userManagerService;
        public SecurityModel(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;
        }

        /*public Dictionary<string, bool> GetUserAccess(string userId)
        {
            var user = _userManagerService.FindByIdAsync(userId).Result;
            var userRoles = _userManagerService.GetRolesAsync(user).Result;
            var userAccesses = accessDictionary.Where(userAcces => userRoles.Contains(userAcces.Key));
            var accesses = new Dictionary<string, bool>();
            foreach (var uAccess in userAccesses)
            {
                foreach (var roleAccesses in uAccess.Value)
                {
                    if (!accesses.ContainsKey(roleAccesses.Key))
                    {
                        accesses.Add(roleAccesses.Key, roleAccesses.Value);
                    }
                    else
                    {
                        accesses[roleAccesses.Key] |= roleAccesses.Value;
                    }
                }
            }

            return accesses;
        }*/

        public Dictionary<string, bool> GetUserAccess(string userId, IEnumerable<string> userRoles = null)
        {
            var user = _userManagerService.FindByIdAsync(userId).Result;
            if (userRoles == null)
            {
                userRoles = _userManagerService.GetRolesAsync(user).Result;
            }

            var userAccesses = accessDictionary.Where(userAccess => userRoles.Contains(userAccess.Key));
            var accesses = new Dictionary<string, bool>();
            foreach (var uAccess in userAccesses)
            {
                foreach (var roleAccesses in uAccess.Value)
                {
                    if (!accesses.ContainsKey(roleAccesses.Key))
                    {
                        accesses.Add(roleAccesses.Key, roleAccesses.Value);
                    }
                    else
                    {
                        accesses[roleAccesses.Key] |= roleAccesses.Value;
                    }
                }
            }

            return accesses;
        }

        public void SetSettingsFile(string jsonPath)
        {
            if (File.Exists(jsonPath))
            {
                accessDictionary =
                    JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, bool>>>(File.ReadAllText(jsonPath));
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
    }
}
