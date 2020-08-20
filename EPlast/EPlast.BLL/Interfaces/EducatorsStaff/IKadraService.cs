using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EducatorsStaff
{
    public interface IKadraService
    {
        Task<IEnumerable<KadraVykhovnykivDTO>> GetAllKVsAsync();

        Task<IEnumerable<KadraVykhovnykivDTO>> GetKVsWithKVType(int kvTypeId);

        Task<IEnumerable<KadraVykhovnykivDTO>> GetKVsOfGivenUser(string UserId);

        Task<KadraVykhovnykivDTO> CreateKadra(KadraVykhovnykivDTO kadrasDTO);

        Task<KadraVykhovnykivDTO> GetKadraById(int KadraID);

        Task<KadraVykhovnykivDTO> GetKadraByRegisterNumber(int KadrasRegisterNumber);

        Task UpdateKadra( KadraVykhovnykivDTO kadrasDTO);

        Task DeleteKadra(int kadra_id);
    }
}
