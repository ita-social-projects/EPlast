using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;

namespace EPlast.BLL.Services.Club
{
    public class ClubDocumentsService: IClubDocumentsService
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IClubFilesBlobStorageRepository _ClubFilesBlobStorage;

        public ClubDocumentsService(IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            IClubFilesBlobStorageRepository ClubFilesBlobStorage
        )
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _ClubFilesBlobStorage = ClubFilesBlobStorage;
        }

        private async Task<IEnumerable<ClubDocumentType>> GetAllClubDocumentTypeEntities()
        {
            var documentTypes = await _repositoryWrapper.ClubDocumentType.GetAllAsync();

            return documentTypes;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ClubDocumentTypeDto>> GetAllClubDocumentTypesAsync()
        {
            var documentTypes = await GetAllClubDocumentTypeEntities();

            return _mapper.Map<IEnumerable<ClubDocumentType>, IEnumerable<ClubDocumentTypeDto>>(documentTypes);
        }

        /// <inheritdoc />
        public async Task<ClubDocumentsDto> AddDocumentAsync(ClubDocumentsDto documentsDTO)
        {
            var fileBase64 = documentsDTO.BlobName.Split(',')[1];
            var extension = $".{documentsDTO.FileName.Split('.').LastOrDefault()}";
            var fileName = $"{Guid.NewGuid()}{extension}";
            await _ClubFilesBlobStorage.UploadBlobForBase64Async(fileBase64, fileName);
            documentsDTO.BlobName = fileName;

            var documentTypes = await GetAllClubDocumentTypesAsync();
            documentsDTO.ClubDocumentType = documentTypes
                .FirstOrDefault(dt => dt.Name == documentsDTO.ClubDocumentType.Name);
            documentsDTO.ClubDocumentTypeId = documentsDTO.ClubDocumentType.ID;

            var document = _mapper.Map<ClubDocumentsDto, ClubDocuments>(documentsDTO);
            _repositoryWrapper.ClubDocuments.Attach(document);
            await _repositoryWrapper.ClubDocuments.CreateAsync(document);
            await _repositoryWrapper.SaveAsync();
            return documentsDTO;
        }

        public async Task<string> DownloadFileAsync(string fileName)
        {
            var fileBase64 = await _ClubFilesBlobStorage.GetBlobBase64Async(fileName);

            return fileBase64;
        }

        public async Task DeleteFileAsync(int documentId)
        {
            var document = await _repositoryWrapper.ClubDocuments
                .GetFirstOrDefaultAsync(d => d.ID == documentId);

            await _ClubFilesBlobStorage.DeleteBlobAsync(document.BlobName);

            _repositoryWrapper.ClubDocuments.Delete(document);
            await _repositoryWrapper.SaveAsync();
        }
    }
}
