using EPlast.BLL.DTO.EducatorsStaff;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EducatorsStaff
{
    public interface IKVsTypeService
    {
        Task<KVTypeDTO> GetKVsTypeByIdAsync(int KV_id);

        Task<IEnumerable<KVTypeDTO>> GetAllKVTypesAsync();

        Task<KVTypeDTO> CreateKVType(KVTypeDTO kvTypeDTO);

        Task<IEnumerable<KadrasDTO>> GetKadrasWithSuchType(KVTypeDTO kvTypeDTO);
    }
}
