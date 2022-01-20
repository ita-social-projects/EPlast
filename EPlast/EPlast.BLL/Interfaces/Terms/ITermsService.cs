using EPlast.BLL.DTO.Terms;
using EPlast.DataAccess.Entities;
using System;
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
        [Obsolete("Use refactored method via mediator query/handler GetFirstRecord")]
        Task<TermsDTO> GetFirstRecordAsync();

        /// <summary>
        /// Get all user Id
        /// </summary>
        /// <param name="user">Authorized user</param>
        /// <returns>All users Id</returns>
        [Obsolete("Use refactored method via mediator query/handler GetAllUsersIdWithoutSender")]
        Task<IEnumerable<string>> GetAllUsersIdWithoutAdminIdAsync(User user);

        /// <summary>
        /// Edit terms of use by Id
        /// </summary>
        /// <param name="termsDTO">Terms model(dto)</param>
        /// <param name="user">Authorized user</param>
        [Obsolete("Use refactored method via mediator query/handler ChangeTerms")]
        Task ChangeTermsAsync(TermsDTO termsDTO, User user);

        /// <summary>
        /// Add terms of use by Id
        /// </summary>
        /// <param name="termsDTO">Terms model(dto)</param>
        /// <param name="user">Authorized user</param>
        [Obsolete("Use refactored method via mediator query/handler AddTerms")]
        Task AddTermsAsync(TermsDTO termsDTO, User user);
    }
}