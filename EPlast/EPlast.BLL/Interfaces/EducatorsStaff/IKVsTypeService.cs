using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EducatorsStaff
{
    public interface IKVsTypeService
    {
        Task<KVTypeDTO> GetKVsTypeByIdAsync(int KV_id);

        Task<IEnumerable<KVTypeDTO>> GetAllKVTypesAsync();

        Task<KVTypeDTO> GetTypeOfConcreteKVByIdAsync(int KV_id);

        void CreateKVType(KVTypeDTO kvTypeDTO); 
    }
}
