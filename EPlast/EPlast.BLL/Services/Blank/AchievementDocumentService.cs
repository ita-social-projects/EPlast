using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Blank;
using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        public async Task<List<AchievementDocumentsDTO>> AddDocumentAsync(List<AchievementDocumentsDTO> listAchievementDocumentsDTO)
        {
            foreach (AchievementDocumentsDTO achievementDocumentsDTO in listAchievementDocumentsDTO)
            {
                var fileBase64 = achievementDocumentsDTO.BlobName.Split(',')[1];
                var extension = "." + achievementDocumentsDTO.FileName.Split('.').LastOrDefault();
                var fileName = Guid.NewGuid() + extension;
                await _blobStorageRepo.UploadBlobForBase64Async(fileBase64, fileName);
                achievementDocumentsDTO.BlobName = fileName;

                var document = _mapper.Map<AchievementDocumentsDTO, AchievementDocuments>(achievementDocumentsDTO);
                _repositoryWrapper.AchievementDocumentsRepository.Attach(document);
                await _repositoryWrapper.AchievementDocumentsRepository.CreateAsync(document);
                await _repositoryWrapper.SaveAsync();
            }

            return listAchievementDocumentsDTO;
        }

        public async Task<int> DeleteFileAsync(int documentId)
        {
            var document = await _repositoryWrapper.AchievementDocumentsRepository
                .GetFirstOrDefaultAsync(d => d.ID == documentId);

            await _blobStorageRepo.DeleteBlobAsync(document.BlobName);

            _repositoryWrapper.AchievementDocumentsRepository.Delete(document);
            await _repositoryWrapper.SaveAsync();

            return StatusCodes.Status204NoContent;
        }

        public async Task<string> DownloadFileAsync(string fileName)
        {
            return await _blobStorageRepo.GetBlobBase64Async(fileName);
        }

        public async Task<List<AchievementDocumentsDTO>> GetDocumentsByUserId(string userid)
        {
            var document = _mapper.Map<IEnumerable<AchievementDocuments>, List<AchievementDocumentsDTO>>(
                await _repositoryWrapper.AchievementDocumentsRepository.FindByCondition(i => i.UserId == userid)
                .ToListAsync());

            return document;
        }

        public async Task<List<AchievementDocumentsDTO>> GetPartOfAchievement(int pageNumber, int pageSize, string userid)
        {
            var partOfAchievements = await _repositoryWrapper.AchievementDocumentsRepository.FindByCondition(i => i.UserId == userid).Skip(pageSize * pageNumber).Take(pageSize).ToListAsync();
            var mappedAchievements = _mapper.Map<List<AchievementDocuments>, List<AchievementDocumentsDTO>>(partOfAchievements);

            return mappedAchievements;
        }
    }
}
