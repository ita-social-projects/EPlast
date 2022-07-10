using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;

namespace EPlast.BLL.Interfaces.EducatorsStaff
{
    public interface IEducatorsStaffService
    {
        Task<IEnumerable<EducatorsStaffDto>> GetAllKVsAsync();

        Task<IEnumerable<EducatorsStaffDto>> GetKVsWithKVType(int kvTypeId);

        Task<IEnumerable<EducatorsStaffDto>> GetKVsOfGivenUser(string UserId);

        Task<EducatorsStaffDto> CreateKadra(EducatorsStaffDto kadrasDTO);

        Task<EducatorsStaffDto> GetKadraById(int KadraID);

        Task<EducatorsStaffDto> GetKadraByRegisterNumber(int KadrasRegisterNumber);

        Task<bool> StaffWithRegisternumberExists(int registerNumber);
        Task<bool> DoesUserHaveSuchStaff(string UserId, int kadraId);


        Task<bool> StaffWithRegisternumberExistsEdit(int kadraId, int numberInRegister);
        Task<bool> UserHasSuchStaffEdit(string UserId, int kadraId);

        Task UpdateKadra(EducatorsStaffDto kadrasDTO);

        Task<string> GetUserByEduStaff(int EduStaffId);
        Task DeleteKadra(int kadra_id);

        Task<Tuple<IEnumerable<EducatorsStaffTableObject>, int>> GetEducatorsStaffTableAsync(int kadraType, IEnumerable<string> sortByOrder, string searchedData, int page, int pageSize);
        Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> GetOrderByUserName();
        Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> GetOrderByNumberInRegister();
        Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> GetOrderByDateOfGranting();
        Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> GetOrderByID();
    }
}
