using EPlast.BLL.DTO.EducatorsStaff;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities.EducatorsStaff;
using System;
using System.Linq.Expressions;

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

        Task<Tuple<IEnumerable<EducatorsStaffTableObject>, int>> GetEducatorsStaffTableAsync(int kadraType, IEnumerable<string> sortByOrder, string searchedData, int page, int pageSize);
        Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> GetOrderByUserName();
        Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> GetOrderByNumberInRegister();
        Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> GetOrderByDateOfGranting();
        Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> GetOrderByID();
    }
}
