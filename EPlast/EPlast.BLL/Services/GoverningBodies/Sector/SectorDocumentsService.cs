using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.DataAccess.Entities.GoverningBody.Sector;
using EPlast.DataAccess.Repositories;

namespace EPlast.BLL.Services.GoverningBodies.Sector
{
    public class SectorDocumentsService : ISectorDocumentsService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IGoverningBodySectorFilesBlobStorageRepository _sectorFilesBlobStorage;

        public SectorDocumentsService(IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IGoverningBodySectorFilesBlobStorageRepository sectorFilesBlobStorage
        )
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _sectorFilesBlobStorage = sectorFilesBlobStorage;
        }

        private async Task<IEnumerable<SectorDocumentType>> GetAllSectorDocumentTypeEntities()
        {
            var documentTypes = await _repositoryWrapper.GoverningBodySectorDocumentType.GetAllAsync();
            return documentTypes;
        }

        public async Task<IEnumerable<SectorDocumentTypeDto>> GetAllSectorDocumentTypesAsync()
        {
            var documentTypes = await GetAllSectorDocumentTypeEntities();
            return _mapper.Map<IEnumerable<SectorDocumentType>, IEnumerable<SectorDocumentTypeDto>>(documentTypes);
        }

        public async Task<SectorDocumentsDto> AddSectorDocumentAsync(SectorDocumentsDto documentDto)
        {
            var fileBase64 = documentDto.BlobName.Split(',')[1];
            var extension = $".{documentDto.FileName.Split('.').LastOrDefault()}";
            var fileName = $"{Guid.NewGuid()}{extension}";

            await _sectorFilesBlobStorage.UploadBlobForBase64Async(fileBase64, fileName);
            documentDto.BlobName = fileName;

            var documentTypes = await GetAllSectorDocumentTypesAsync();
            documentDto.SectorDocumentType = documentTypes
                .FirstOrDefault(dt => dt.Name == documentDto.SectorDocumentType.Name);
            documentDto.SectorDocumentTypeId = documentDto.SectorDocumentType.Id;

            var document = _mapper.Map<SectorDocumentsDto, SectorDocuments>(documentDto);
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