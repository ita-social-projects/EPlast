using EPlast.BLL.DTO.EducatorsStaff;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities.EducatorsStaff;
using System;

namespace EPlast.BLL.Interfaces.EducatorsStaff
{
    public interface IEducatorsStaffService
    {
        Task<IEnumerable<EducatorsStaffDTO>> GetAllKVsAsync();

        Task<IEnumerable<EducatorsStaffDTO>> GetKVsWithKVType(int kvTypeId);

        Task<IEnumerable<EducatorsStaffDTO>> GetKVsOfGivenUser(string UserId);

        Task<EducatorsStaffDTO> CreateKadra(EducatorsStaffDTO kadrasDTO);

        Task<EducatorsStaffDTO> GetKadraById(int KadraID);

        Task<EducatorsStaffDTO> GetKadraByRegisterNumber(int KadrasRegisterNumber);

        Task<bool> StaffWithRegisternumberExists(int registerNumber);
        Task<bool> DoesUserHaveSuchStaff(string UserId , int kadraId);


        Task<bool> StaffWithRegisternumberExistsEdit(int kadraId, int numberInRegister);
        Task<bool> UserHasSuchStaffEdit(string UserId, int kadraId);

        Task UpdateKadra( EducatorsStaffDTO kadrasDTO);

        Task<string> GetUserByEduStaff(int EduStaffId);
        Task DeleteKadra(int kadra_id);

        /// <summary>
        /// Returns EducatorsStaff with params
        /// </summary>
        /// <param name="kadraType">Type of Kadra</param>
        /// <param name="searchedData">Search string</param>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>EducatorsStaff</returns>
        IEnumerable<EducatorsStaffTableObject> GetEducatorsStaffTableObject(int kadraType, string searchedData, int page, int pageSize);
        Task<Tuple<IEnumerable<EducatorsStaffTableObject>, int>> GetEducatorsStaffTableAsync(int kadraType, IEnumerable<string> sortByOrder, string searchedData, int page, int pageSize);
    }
}
