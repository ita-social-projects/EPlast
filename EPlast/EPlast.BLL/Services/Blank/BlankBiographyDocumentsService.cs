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
   public class BlankBiographyDocumentsService : IBlankBiographyDocumentService
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
        public async Task<BlankBiographyDocumentsDto> AddDocumentAsync(BlankBiographyDocumentsDto biographyDocumentDTO)
        {
            var fileBase64 = biographyDocumentDTO.BlobName.Split(',')[1];
            var extension = "." + biographyDocumentDTO.FileName.Split('.').LastOrDefault();
            var fileName = Guid.NewGuid() + extension;
            await _blankFilesBlobStorage.UploadBlobForBase64Async(fileBase64, fileName);
            biographyDocumentDTO.BlobName = fileName;

            var document = _mapper.Map<BlankBiographyDocumentsDto, BlankBiographyDocuments>(biographyDocumentDTO);
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
            return await _blankFilesBlobStorage.GetBlobBase64Async(fileName);
        }


        public async Task<BlankBiographyDocumentsDto> GetDocumentByUserId(string userid)
        {
            return _mapper.Map<BlankBiographyDocuments, BlankBiographyDocumentsDto>(
                await _repositoryWrapper.BiographyDocumentsRepository.GetFirstOrDefaultAsync(i => i.UserId == userid));
        }
    }
}
