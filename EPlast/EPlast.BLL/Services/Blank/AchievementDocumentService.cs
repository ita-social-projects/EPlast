using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Blank;
using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Blank
{
    public class AchievementDocumentService : IBlankAchievementDocumentService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IBlankAchievementBlobStorageRepository _blobStorageRepo;

        public AchievementDocumentService(IRepositoryWrapper repositoryWrapper,
           IMapper mapper,
           IBlankAchievementBlobStorageRepository blobStorageRepo)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobStorageRepo = blobStorageRepo;
        }


        public async Task<IEnumerable<AchievementDocumentsDTO>> AddDocumentAsync(IEnumerable<AchievementDocumentsDTO> achievementDocumentsDTO)
        {
            foreach (AchievementDocumentsDTO achievementDocumentDTO in achievementDocumentsDTO)
            {
                var fileBase64 = achievementDocumentDTO.BlobName.Split(',')[1];
                var extension = "." + achievementDocumentDTO.FileName.Split('.').LastOrDefault();
                var fileName = Guid.NewGuid() + extension;
                await _blobStorageRepo.UploadBlobForBase64Async(fileBase64, fileName);
                achievementDocumentDTO.BlobName = fileName;

                var document = _mapper.Map<AchievementDocumentsDTO, AchievementDocuments>(achievementDocumentDTO);
                _repositoryWrapper.AchievementDocumentsRepository.Attach(document);
                await _repositoryWrapper.AchievementDocumentsRepository.CreateAsync(document);
                await _repositoryWrapper.SaveAsync();
            }

            return achievementDocumentsDTO;
        }

        public async Task DeleteFileAsync(int documentId)
        {
            var document = await _repositoryWrapper.AchievementDocumentsRepository
                .GetFirstOrDefaultAsync(d => d.ID == documentId);

            await _blobStorageRepo.DeleteBlobAsync(document.BlobName);

            _repositoryWrapper.AchievementDocumentsRepository.Delete(document);
            await _repositoryWrapper.SaveAsync();

        }

        public async Task<string> DownloadFileAsync(string fileName)
        {
            return await _blobStorageRepo.GetBlobBase64Async(fileName);
        }

        public async Task<IEnumerable<AchievementDocumentsDTO>> GetDocumentsByUserId(string userid)
        {
            return _mapper.Map<IEnumerable<AchievementDocuments>,IEnumerable<AchievementDocumentsDTO>>(
                await _repositoryWrapper.AchievementDocumentsRepository.FindByCondition(i => i.UserId == userid)
                .ToListAsync());
        }

        public async Task<IEnumerable<AchievementDocumentsDTO>> GetPartOfAchievement(int pageNumber, int pageSize, string userid)
        {
            var partOfAchievements = await _repositoryWrapper.AchievementDocumentsRepository.FindByCondition(i => i.UserId == userid).Skip(pageSize * pageNumber).Take(pageSize).ToListAsync();

            return _mapper.Map<IEnumerable<AchievementDocuments>, IEnumerable<AchievementDocumentsDTO>>(partOfAchievements);
            
        }
    }
}
