using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.FormerMember
{
    public interface IFormerMemberAdminService
    {
        /// <summary>
        /// Removes user from his admin roles when he becomes former member
        /// </summary>
        /// <param name="userId">The id of the user</param>
        Task RemoveFromAdminRolesAsync(string userId);
    }
}
