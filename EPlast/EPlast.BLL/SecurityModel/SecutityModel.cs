using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Services.Interfaces;

namespace EPlast.BLL.SecurityModel
{
    public class SecurityModel : ISecurityModel
    {
        private Dictionary<string, Dictionary<string, bool>> _accessDictionary;
        private readonly IUserManagerService _userManagerService;
        private const string SourceUrl = @"Properties\JSONAccessSettingFiles\";

        public SecurityModel(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;
        }

        public async Task<Dictionary<string, bool>> GetUserAccessAsync(string userId, IEnumerable<string> userRoles = null)
        {
            if (userRoles == null)
            {
                var user = await _userManagerService.FindByIdAsync(userId);
                userRoles = await _userManagerService.GetRolesAsync(user);
            }

            var userAccesses = _accessDictionary.Where(userAccess => userRoles.Contains(userAccess.Key));
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

        public void SetSettingsFile(string jsonFileName)
        {
            string jsonPath = SourceUrl + jsonFileName;

            if (File.Exists(jsonPath))
            {
                _accessDictionary =
                    JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, bool>>>(File.ReadAllText(jsonPath));
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
    }
}
