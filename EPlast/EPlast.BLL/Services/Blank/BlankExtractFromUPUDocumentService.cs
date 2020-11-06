using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Blank;
using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Blank
{
    public class BlankExtractFromUPUDocumentService : IBlankExtractFromUPUDocumentService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IBlankExtractFromUPUBlobStorageRepository _blankFilesBlobStorage;

        public BlankExtractFromUPUDocumentService(IRepositoryWrapper repositoryWrapper,
           IMapper mapper,
           IBlankExtractFromUPUBlobStorageRepository blankFilesBlobStorage)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blankFilesBlobStorage = blankFilesBlobStorage;
        }


        public async Task<ExtractFromUPUDocumentsDTO> AddDocumentAsync(ExtractFromUPUDocumentsDTO biographyDocumentDTO)
        {
            var fileBase64 = biographyDocumentDTO.BlobName.Split(',')[1];
            var extension = "." + biographyDocumentDTO.FileName.Split('.').LastOrDefault();
            var fileName = Guid.NewGuid() + extension;
            await _blankFilesBlobStorage.UploadBlobForBase64Async(fileBase64, fileName);
            biographyDocumentDTO.BlobName = fileName;

            var document = _mapper.Map<ExtractFromUPUDocumentsDTO, ExtractFromUPUDocuments>(biographyDocumentDTO);
            _repositoryWrapper.ExtractFromUPUDocumentsRepository.Attach(document);
            await _repositoryWrapper.ExtractFromUPUDocumentsRepository.CreateAsync(document);
            await _repositoryWrapper.SaveAsync();

            return biographyDocumentDTO;
        }

        public async Task DeleteFileAsync(int documentId)
        {
            var document = await _repositoryWrapper.ExtractFromUPUDocumentsRepository
               .GetFirstOrDefaultAsync(d => d.ID == documentId);

            await _blankFilesBlobStorage.DeleteBlobAsync(document.BlobName);

            _repositoryWrapper.ExtractFromUPUDocumentsRepository.Delete(document);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task<string> DownloadFileAsync(string fileName)
        {
            return await _blankFilesBlobStorage.GetBlobBase64Async(fileName);
        }

        public async Task<ExtractFromUPUDocumentsDTO> GetDocumentByUserId(string userid)
        {
            return _mapper.Map<ExtractFromUPUDocuments, ExtractFromUPUDocumentsDTO>(
               await _repositoryWrapper.ExtractFromUPUDocumentsRepository.GetFirstOrDefaultAsync(i => i.UserId == userid));
        }
    }
}
