using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Repositories;

namespace EPlast.BLL.Services.GoverningBodies
{
    public class GoverningBodyDocumentsService : IGoverningBodyDocumentsService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IGoverningBodyFilesBlobStorageRepository _governingBodyFilesBlobStorage;

        public GoverningBodyDocumentsService(IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IGoverningBodyFilesBlobStorageRepository governingBodyFilesBlobStorage
        )
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _governingBodyFilesBlobStorage = governingBodyFilesBlobStorage;
        }

        private async Task<IEnumerable<GoverningBodyDocumentType>> GetAllGoverningBodyDocumentTypeEntities()
        {
            var documentTypes = await _repositoryWrapper.GoverningBodyDocumentType.GetAllAsync();

            return documentTypes;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GoverningBodyDocumentTypeDTO>> GetAllGoverningBodyDocumentTypesAsync()
        {
            var documentTypes = await GetAllGoverningBodyDocumentTypeEntities();

            return _mapper.Map<IEnumerable<GoverningBodyDocumentType>, IEnumerable<GoverningBodyDocumentTypeDTO>>(documentTypes);
        }

        /// <inheritdoc />
        public async Task<GoverningBodyDocumentsDTO> AddGoverningBodyDocumentAsync(GoverningBodyDocumentsDTO documentDto)
        {
            var fileBase64 = documentDto.BlobName.Split(',')[1];
            var extension = $".{documentDto.FileName.Split('.').LastOrDefault()}";
            var fileName = $"{Guid.NewGuid()}{extension}";

            await _governingBodyFilesBlobStorage.UploadBlobForBase64Async(fileBase64, fileName);
            documentDto.BlobName = fileName;

            var documentTypes = await GetAllGoverningBodyDocumentTypesAsync();
            documentDto.GoverningBodyDocumentType = documentTypes
                .FirstOrDefault(dt => dt.Name == documentDto.GoverningBodyDocumentType.Name);
            documentDto.GoverningBodyDocumentTypeId = documentDto.GoverningBodyDocumentType.Id;

            var document = _mapper.Map<GoverningBodyDocumentsDTO, GoverningBodyDocuments>(documentDto);
            _repositoryWrapper.GoverningBodyDocuments.Attach(document);
            await _repositoryWrapper.GoverningBodyDocuments.CreateAsync(document);
            await _repositoryWrapper.SaveAsync();

            return documentDto;
        }

        public async Task<string> DownloadGoverningBodyDocumentAsync(string documentName)
        {
            var fileBase64 = await _governingBodyFilesBlobStorage.GetBlobBase64Async(documentName);

            return fileBase64;
        }

        public async Task DeleteGoverningBodyDocumentAsync(int documentId)
        {
            var document = await _repositoryWrapper.GoverningBodyDocuments
                .GetFirstOrDefaultAsync(d => d.Id == documentId);

            await _governingBodyFilesBlobStorage.DeleteBlobAsync(document.BlobName);

            _repositoryWrapper.GoverningBodyDocuments.Delete(document);
            await _repositoryWrapper.SaveAsync();
        }
    }
}
