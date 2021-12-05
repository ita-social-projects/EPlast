using EPlast.BLL.DTO.Terms;
using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Terms
{
    public interface ITermsService
    {
        /// <summary>
        /// Get first record from database
        /// </summary>
        /// <returns>First record</returns>
        Task<TermsDTO> GetFirstRecordAsync();

        /// <summary>
        /// Get all user Id
        /// </summary>
        /// <param name="user">The id of the user</param>
        /// <returns>All users Id</returns>
        Task<IEnumerable<string>> GetAllUsersIdAsync(User user);

        /// <summary>
        /// Edit new terms of use by Id
        /// </summary>
        /// <param name="termsDTO">An information about terms of use</param>
        /// <param name="user">The id of the user</param>
        /// <returns>Annual report model</returns>
        Task ChangeTermsAsync(TermsDTO termsDTO, User user);

        /// <summary>
        /// Add new terms of use by Id
        /// </summary>
        /// <param name="termsDTO">An information about terms of use</param>
        /// <param name="user">The id of the user</param>
        Task AddTermsAsync(TermsDTO termsDTO, User user);
    }
}