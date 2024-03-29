﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Blank;
using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Services.Blank
{
    public class AchievementDocumentService : IBlankAchievementDocumentService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IUserCourseService _usercourseService;
        private readonly IMapper _mapper;
        private readonly IBlankAchievementBlobStorageRepository _blobStorageRepo;

        public AchievementDocumentService(IRepositoryWrapper repositoryWrapper,
           IMapper mapper,
           IBlankAchievementBlobStorageRepository blobStorageRepo,
           IUserCourseService usercourseService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _blobStorageRepo = blobStorageRepo;
            _usercourseService = usercourseService;
        }

        public async Task<IEnumerable<AchievementDocumentsDto>> AddDocumentAsync(IEnumerable<AchievementDocumentsDto> achievementDocumentsDTO)
        {
            foreach (AchievementDocumentsDto achievementDocumentDTO in achievementDocumentsDTO)
            {
                var fileBase64 = achievementDocumentDTO.BlobName.Split(',')[1];
                var extension = "." + achievementDocumentDTO.FileName.Split('.').LastOrDefault();
                var fileName = Guid.NewGuid() + extension;
                await _blobStorageRepo.UploadBlobForBase64Async(fileBase64, fileName);
                achievementDocumentDTO.BlobName = fileName;

                var document = _mapper.Map<AchievementDocumentsDto, AchievementDocuments>(achievementDocumentDTO);
                _repositoryWrapper.AchievementDocumentsRepository.Attach(document);
                await _repositoryWrapper.AchievementDocumentsRepository.CreateAsync(document);
                await _repositoryWrapper.SaveAsync();
            }

            return achievementDocumentsDTO;
        }

        public async Task DeleteFileAsync(int documentId, string userId)
        {
            var document = await _repositoryWrapper.AchievementDocumentsRepository
                .GetFirstOrDefaultAsync(d => d.ID == documentId);
            if (document != null)
            {
                await _blobStorageRepo.DeleteBlobAsync(document.BlobName);
                _repositoryWrapper.AchievementDocumentsRepository.Delete(document);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task<string> DownloadFileAsync(string fileName)
        {
            return await _blobStorageRepo.GetBlobBase64Async(fileName);
        }

        public async Task<IEnumerable<AchievementDocumentsDto>> GetDocumentsByUserIdAsync(string userId)
        {
            return _mapper.Map<IEnumerable<AchievementDocuments>, IEnumerable<AchievementDocumentsDto>>(
                await _repositoryWrapper.AchievementDocumentsRepository.GetAllAsync(i => i.UserId == userId));
        }

        public async Task<IEnumerable<AchievementDocumentsDto>> GetPartOfAchievementByUserIdAsync(int pageNumber, int pageSize, string userId)
        {
            var achievements = await _repositoryWrapper.AchievementDocumentsRepository.GetAllAsync(i => i.UserId == userId);
            var partOfAchievements = achievements.Skip(pageSize * pageNumber).Take(pageSize);
            return _mapper.Map<IEnumerable<AchievementDocuments>, IEnumerable<AchievementDocumentsDto>>(partOfAchievements);
        }

        public async Task<IEnumerable<AchievementDocumentsDto>> GetPartOfAchievementByUserIdAndCourseIdAsync(int pageNumber, int pageSize, string userId, int courseId)
        {
            var achievements = await _repositoryWrapper.AchievementDocumentsRepository.GetAllAsync(i => i.UserId == userId && i.CourseId == courseId);
            var partOfAchievements = achievements.Skip(pageSize * pageNumber).Take(pageSize);
            return _mapper.Map<IEnumerable<AchievementDocuments>, IEnumerable<AchievementDocumentsDto>>(partOfAchievements);
        }
    }
}
