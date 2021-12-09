using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.FormerMember
{
    public interface IFormerMemberService
    {
        /// <summary>
        /// Changes user status to Former member 
        /// </summary>
        /// <param name="userId">User id</param>
        public Task MakeUserFormerMeberAsync(string userId);
    }
}
