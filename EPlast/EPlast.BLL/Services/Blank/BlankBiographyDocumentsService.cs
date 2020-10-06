using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Blank;
using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Blank
{
    class BlankBiographyDocumentsService : IBlankBiographyDocumentService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IBlankFilesBlobStorageRepository _blankFilesBlobStorage;

        public BlankBiographyDocumentsService(IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IBlankFilesBlobStorageRepository blankFilesBlobStorage)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blankFilesBlobStorage = blankFilesBlobStorage;
        }
        public async Task<BlankBiographyDocumentsDTO> AddDocumentAsync(BlankBiographyDocumentsDTO biographyDocumentDTO)
        {
            var fileBase64 = biographyDocumentDTO.BlobName.Split(',')[1];
            var extension = "." + biographyDocumentDTO.FileName.Split('.').LastOrDefault();
            var fileName = Guid.NewGuid() + extension;
            await _blankFilesBlobStorage.UploadBlobForBase64Async(fileBase64, fileName);
            biographyDocumentDTO.BlobName = fileName;

            var documentTypes = await GetAllBlankDocumentTypesAsync();
            biographyDocumentDTO.BlankBiographyDocumentsTypeDTO = documentTypes
                .FirstOrDefault(dt => dt.Name == biographyDocumentDTO.BlankBiographyDocumentsTypeDTO.Name);
            biographyDocumentDTO.BlankDocumentTypeId = biographyDocumentDTO.BlankBiographyDocumentsTypeDTO.ID;

            var document = _mapper.Map<BlankBiographyDocumentsDTO, BlankBiographyDocuments>(biographyDocumentDTO);
            _repositoryWrapper.BiographyDocumentsRepository.Attach(document);
            await _repositoryWrapper.BiographyDocumentsRepository.CreateAsync(document);
            await _repositoryWrapper.SaveAsync();

            return biographyDocumentDTO;
        }

        public async Task DeleteFileAsync(int documentId)
        {
            var document = await _repositoryWrapper.BiographyDocumentsRepository
                .GetFirstOrDefaultAsync(d => d.ID == documentId);

            await _blankFilesBlobStorage.DeleteBlobAsync(document.BlobName);

            _repositoryWrapper.BiographyDocumentsRepository.Delete(document);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task<string> DownloadFileAsync(string fileName)
        {
            var fileBase64 = await _blankFilesBlobStorage.GetBlobBase64Async(fileName);

            return fileBase64;
        }

        public async Task<IEnumerable<BlankBiographyDocumentsTypeDTO>> GetAllBlankDocumentTypesAsync()
        {
            var documentsTypes = _mapper.Map<IEnumerable<BlankBiographyDocumentsType>, 
                IEnumerable<BlankBiographyDocumentsTypeDTO>>(
                await _repositoryWrapper.BiographyDocumentsTypeRepository.GetAllAsync()
                );

            return documentsTypes;
        }
    }
}
