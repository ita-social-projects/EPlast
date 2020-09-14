using EPlast.BLL.DTO.EducatorsStaff;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EducatorsStaff
{
    public interface IEducatorsStaffTypesService
    {
        Task<EducatorsStaffTypesDTO> GetKVsTypeByIdAsync(int KV_id);

        Task<IEnumerable<EducatorsStaffTypesDTO>> GetAllKVTypesAsync();

        Task<EducatorsStaffTypesDTO> CreateKVType(EducatorsStaffTypesDTO kvTypeDTO);

        Task<IEnumerable<EducatorsStaffDTO>> GetKadrasWithSuchType(EducatorsStaffTypesDTO kvTypeDTO);
    }
}
