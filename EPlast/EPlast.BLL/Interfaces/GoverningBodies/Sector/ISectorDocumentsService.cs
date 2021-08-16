using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.GoverningBody.Sector;

namespace EPlast.BLL.Interfaces.GoverningBodies.Sector
{
    public interface ISectorDocumentsService
    {
        Task<SectorDocumentsDTO> AddSectorDocumentAsync(SectorDocumentsDTO documentDto);

        Task<string> DownloadSectorDocumentAsync(string documentName);

        Task<IEnumerable<SectorDocumentTypeDTO>> GetAllSectorDocumentTypesAsync();

        Task DeleteSectorDocumentAsync(int documentId);
    }
}
