using EPlast.BLL.DTO.EducatorsStaff;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EducatorsStaff
{
    public interface IKadrasTypeService
    {
        Task<KadraVykhovnykivTypesDTO> GetKVsTypeByIdAsync(int KV_id);

        Task<IEnumerable<KadraVykhovnykivTypesDTO>> GetAllKVTypesAsync();

        Task<KadraVykhovnykivTypesDTO> CreateKVType(KadraVykhovnykivTypesDTO kvTypeDTO);

        Task<IEnumerable<KadraVykhovnykivDTO>> GetKadrasWithSuchType(KadraVykhovnykivTypesDTO kvTypeDTO);
        Task<int> GetIdWithKVType(string detectname);
    }
}
