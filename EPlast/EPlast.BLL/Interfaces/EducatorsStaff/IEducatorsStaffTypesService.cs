using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.EducatorsStaff;

namespace EPlast.BLL.Interfaces.EducatorsStaff
{
    public interface IEducatorsStaffTypesService
    {
        Task<EducatorsStaffTypesDto> GetKVsTypeByIdAsync(int KV_id);

        Task<IEnumerable<EducatorsStaffTypesDto>> GetAllKVTypesAsync();

        Task<EducatorsStaffTypesDto> CreateKVType(EducatorsStaffTypesDto kvTypeDTO);

        Task<IEnumerable<EducatorsStaffDto>> GetKadrasWithSuchType(EducatorsStaffTypesDto kvTypeDTO);
    }
}
