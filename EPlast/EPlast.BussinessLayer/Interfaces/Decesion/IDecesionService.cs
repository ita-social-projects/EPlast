using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public interface IDecesionService
    {
        Task<DecisionDto> GetDecesion();

        List<DecisionDto> GetDecesionsList();

        Task<bool> CreateDecesion();

        Task<bool> ChangeDecesion();

        Task<bool> SaveDecesionAsync();

        Task<byte[]> DownloadDecesion();
    }
}