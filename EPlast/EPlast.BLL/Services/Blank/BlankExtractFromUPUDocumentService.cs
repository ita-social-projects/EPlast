using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Blank;
using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Repositories;

namespace EPlast.BLL.Services.Blank
{
    public class BlankExtractFromUpuDocumentService : IBlankExtractFromUpuDocumentService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IBlankExtractFromUpuBlobStorageRepository _blankFilesBlobStorage;

        public BlankExtractFromUpuDocumentService(IRepositoryWrapper repositoryWrapper,
           IMapper mapper,
           IBlankExtractFromUpuBlobStorageRepository blankFilesBlobStorage)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blankFilesBlobStorage = blankFilesBlobStorage;
        }


        public async Task<ExtractFromUpuDocumentsDto> AddDocumentAsync(ExtractFromUpuDocumentsDto extractFromUPUDocumentsDTO)
        {
            var fileBase64 = extractFromUPUDocumentsDTO.BlobName.Split(',')[1];
            var extension = "." + extractFromUPUDocumentsDTO.FileName.Split('.').LastOrDefault();
            var fileName = Guid.NewGuid() + extension;
            await _blankFilesBlobStorage.UploadBlobForBase64Async(fileBase64, fileName);
            extractFromUPUDocumentsDTO.BlobName = fileName;

            var document = _mapper.Map<ExtractFromUpuDocumentsDto, ExtractFromUpuDocuments>(extractFromUPUDocumentsDTO);
            _repositoryWrapper.ExtractFromUPUDocumentsRepository.Attach(document);
            await _repositoryWrapper.ExtractFromUPUDocumentsRepository.CreateAsync(document);
            await _repositoryWrapper.SaveAsync();

            return extractFromUPUDocumentsDTO;
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

        public async Task<ExtractFromUpuDocumentsDto> GetDocumentByUserId(string userid)
        {
            return _mapper.Map<ExtractFromUpuDocuments, ExtractFromUpuDocumentsDto>(
               await _repositoryWrapper.ExtractFromUPUDocumentsRepository.GetFirstOrDefaultAsync(i => i.UserId == userid));
        }
    }
}
