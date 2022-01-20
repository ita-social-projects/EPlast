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
        /// <param name="user">Authorized user</param>
        /// <returns>All users Id without sender</returns>
        Task<IEnumerable<string>> GetAllUsersIdWithoutAdminIdAsync(User user);

        /// <summary>
        /// Edit terms of use by Id
        /// </summary>
        /// <param name="termsDTO">Terms model(dto)</param>
        /// <param name="user">Authorized user</param>
        Task ChangeTermsAsync(TermsDTO termsDTO, User user);

        /// <summary>
        /// Add terms of use by Id
        /// </summary>
        /// <param name="termsDTO">Terms model(dto)</param>
        /// <param name="user">Authorized user</param>
        Task AddTermsAsync(TermsDTO termsDTO, User user);
    }
}