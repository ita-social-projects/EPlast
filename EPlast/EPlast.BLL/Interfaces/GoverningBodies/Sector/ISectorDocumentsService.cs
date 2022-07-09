using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.GoverningBody.Sector;

namespace EPlast.BLL.Interfaces.GoverningBodies.Sector
{
    public interface ISectorDocumentsService
    {
        Task<SectorDocumentsDto> AddSectorDocumentAsync(SectorDocumentsDto documentDto);

        Task<string> DownloadSectorDocumentAsync(string documentName);

        Task<IEnumerable<SectorDocumentTypeDto>> GetAllSectorDocumentTypesAsync();

        Task DeleteSectorDocumentAsync(int documentId);
    }
}
