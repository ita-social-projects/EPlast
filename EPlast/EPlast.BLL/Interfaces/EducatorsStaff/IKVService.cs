using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EducatorsStaff
{
    public interface IKVService
    {
        Task<IEnumerable<KadrasDTO>> GetAllKVsAsync();

        Task<IEnumerable<KadrasDTO>> GetKVsWithKVType(int kvTypeId);

        Task<IEnumerable<KadrasDTO>> GetKVsOfGivenUser(string UserId);

        Task<KadrasDTO> CreateKadra(KadrasDTO kadrasDTO);

        Task<KadrasDTO> GetKadraById(int KadraID);

        Task<KadrasDTO> GetKadraByRegisterNumber(int KadrasRegisterNumber);

        Task UpdateKadra( KadrasDTO kadrasDTO);

        Task DeleteKadra(int kadra_id);
    }
}
