using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Entities.GoverningBody.Sector;
using EPlast.DataAccess.Repositories;

namespace EPlast.BLL.Services.GoverningBodies.Sector
{
    public class SectorDocumentsService : ISectorDocumentsService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IGoverningBodySectorFilesBlobStorageRepository _sectorFilesBlobStorage;
        private readonly IUniqueIdService _uniqueId;

        public SectorDocumentsService(IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IGoverningBodySectorFilesBlobStorageRepository sectorFilesBlobStorage,
            IUniqueIdService uniqueId)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _sectorFilesBlobStorage = sectorFilesBlobStorage;
            _uniqueId = uniqueId;
        }

        private async Task<IEnumerable<SectorDocumentType>> GetAllSectorDocumentTypeEntities()
        {
            var documentTypes = await _repositoryWrapper.GoverningBodySectorDocumentType.GetAllAsync();
            return documentTypes;
        }

        public async Task<IEnumerable<SectorDocumentTypeDTO>> GetAllSectorDocumentTypesAsync()
        {
            var documentTypes = await GetAllSectorDocumentTypeEntities();
            return _mapper.Map<IEnumerable<SectorDocumentType>, IEnumerable<SectorDocumentTypeDTO>>(documentTypes);
        }

        public async Task<SectorDocumentsDTO> AddSectorDocumentAsync(SectorDocumentsDTO documentDto)
        {
            var fileBase64 = documentDto.BlobName.Split(',')[1];
            var extension = $".{documentDto.FileName.Split('.').LastOrDefault()}";
            var fileName = $"{_uniqueId.GetUniqueId()}{extension}";

            await _sectorFilesBlobStorage.UploadBlobForBase64Async(fileBase64, fileName);
            documentDto.BlobName = fileName;

            var documentTypes = await GetAllSectorDocumentTypesAsync();
            documentDto.SectorDocumentType = documentTypes
                .FirstOrDefault(dt => dt.Name == documentDto.SectorDocumentType.Name);
            documentDto.SectorDocumentTypeId = documentDto.SectorDocumentType.Id;

            var document = _mapper.Map<SectorDocumentsDTO, SectorDocuments>(documentDto);
            _repositoryWrapper.GoverningBodySectorDocuments.Attach(document);
            await _repositoryWrapper.GoverningBodySectorDocuments.CreateAsync(document);
            await _repositoryWrapper.SaveAsync();

            return documentDto;
        }

        public async Task<string> DownloadSectorDocumentAsync(string documentName)
        {
            var fileBase64 = await _sectorFilesBlobStorage.GetBlobBase64Async(documentName);
            return fileBase64;
        }

        public async Task DeleteSectorDocumentAsync(int documentId)
        {
            var document = await _repositoryWrapper.GoverningBodySectorDocuments
                .GetFirstOrDefaultAsync(d => d.Id == documentId);

            await _sectorFilesBlobStorage.DeleteBlobAsync(document.BlobName);

            _repositoryWrapper.GoverningBodySectorDocuments.Delete(document);
            await _repositoryWrapper.SaveAsync();
        }
    }
}