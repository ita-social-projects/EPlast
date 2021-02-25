using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.UserProfiles
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IUserPersonalDataService _userPersonalDataService;
        private readonly IWebHostEnvironment _env;
        private readonly IUserBlobStorageRepository _userBlobStorage;
        private readonly IUniqueIdService _uniqueId;

        public UserService(IRepositoryWrapper repoWrapper, 
            UserManager<User> userManager, 
            IMapper mapper,
            IUserPersonalDataService userPersonalDataService,
            IUserBlobStorageRepository userBlobStorage, 
            IWebHostEnvironment env, 
            IUniqueIdService uniqueId)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _userPersonalDataService = userPersonalDataService;
            _userBlobStorage = userBlobStorage;
            _env = env;
            _uniqueId = uniqueId;
        }

        /// <inheritdoc />
        public async Task<UserDTO> GetUserAsync(string userId)
        {
            var user = await _repoWrapper.User.GetFirstAsync(
                i => i.Id == userId,
                i =>
                    i.Include(g => g.UserProfile).
                        ThenInclude(x => x.Nationality).
                    Include(g => g.UserProfile).
                        ThenInclude(g => g.Gender).
                    Include(g => g.UserProfile).
                        ThenInclude(g => g.Education).
                    Include(g => g.UserProfile).
                        ThenInclude(g => g.Degree).
                    Include(g => g.UserProfile).
                        ThenInclude(g => g.Religion).
                    Include(g => g.UserProfile).
                        ThenInclude(g => g.Work).
                    Include(g => g.CityMembers).
                        ThenInclude(g => g.City).
                    Include(g => g.ClubMembers).
                        ThenInclude(g => g.Club).
                    Include(g => g.UserProfile).
                        ThenInclude(g => g.UpuDegree).
                    Include(x => x.ConfirmedUsers).
                        ThenInclude(q => (q as ConfirmedUser).Approver).
                            ThenInclude(q => q.User));
            var model = _mapper.Map<User, UserDTO>(user);

            return model;
        }

        /// <inheritdoc />
        public IEnumerable<ConfirmedUserDTO> GetConfirmedUsers(UserDTO user)
        {
            var result = user.ConfirmedUsers.
                Where(x => !x.isCityAdmin && !x.isClubAdmin);
            return result;
        }

        /// <inheritdoc />
        public ConfirmedUserDTO GetClubAdminConfirmedUser(UserDTO user)
        {
            var result = user.ConfirmedUsers.
                FirstOrDefault(x => x.isClubAdmin);

            return result;
        }

        /// <inheritdoc />
        public ConfirmedUserDTO GetCityAdminConfirmedUser(UserDTO user)
        {
            var result = user.ConfirmedUsers.
                FirstOrDefault(x => x.isCityAdmin);

            return result;
        }

        /// <inheritdoc />
        public bool CanApprove(IEnumerable<ConfirmedUserDTO> confUsers, string userId, User user)
        {
            var currentUserId = user.Id;

            var canApprove = confUsers.Count() < 3
                    && !confUsers.Any(x => x.Approver.UserID == currentUserId)
                    && currentUserId != userId;

            return canApprove;
        }

        /// <inheritdoc />
        public TimeSpan CheckOrAddPlastunRole(string userId, DateTime registeredOn)
        {
            try
            {
                var timeToJoinPlast = registeredOn.AddYears(1) - DateTime.Now;
                TimeSpan halfOfYear = new TimeSpan(182, 0, 0, 0);
                if (_repoWrapper.ConfirmedUser.FindByCondition(x => x.UserID == userId).Any(q => q.isClubAdmin))
                {
                    timeToJoinPlast = timeToJoinPlast.Subtract(halfOfYear);
                }
                if (timeToJoinPlast <= TimeSpan.Zero)
                    return TimeSpan.Zero;

                return timeToJoinPlast;
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }
        public async Task UpdateAsyncForFile(UserDTO user, IFormFile file, int? placeOfStudyId, int? specialityId, int? placeOfWorkId, int? positionId)
        {
            user.ImagePath = await UploadPhotoAsyncInFolder(user.Id, file);
            await UpdateAsync(user, placeOfStudyId, specialityId, placeOfWorkId, positionId);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsyncForBase64(UserDTO user, string base64, int? placeOfStudyId, int? specialityId, int? placeOfWorkId, int? positionId)
        {
            user = SaveCorrectLinks(user);
            user.ImagePath ??= await UploadPhotoAsyncFromBase64(user.Id, base64);
            await UpdateAsync(user, placeOfStudyId, specialityId, placeOfWorkId, positionId);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<string> GetImageBase64Async(string fileName)
        {
            return await _userBlobStorage.GetBlobBase64Async(fileName);

        }
        private async Task<int?> CheckEducationFieldsAsync(string firstName, string secondName, int? firstId, int? secondId)
        {
            if (secondId == firstId)
            {
                return secondId;
            }
            else
            {
                var spec = await _userPersonalDataService?.GetEducationsByIdAsync(secondId);
                var placeStudy = await _userPersonalDataService?.GetEducationsByIdAsync(firstId);
                if (spec != null && spec.PlaceOfStudy == firstName)
                {
                    return spec.ID;
                }
                else if (placeStudy != null && placeStudy.Speciality == secondName)
                {
                    return placeStudy.ID;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<int?> CheckWorkFieldsAsync(string firstName, string secondName, int? firstId, int? secondId)
        {
            if (secondId == firstId)
            {
                return secondId;
            }
            else
            {
                var placeOfWork = await _userPersonalDataService?.GetWorkByIdAsync(firstId);
                var position = await _userPersonalDataService?.GetWorkByIdAsync(secondId);
                if (position != null && position.PlaceOfwork == firstName)
                {
                    return position.ID;
                }
                else if (placeOfWork != null && placeOfWork.Position == secondName)
                {
                    return placeOfWork.ID;
                }
                else
                {
                    return null;
                }
            }
        }

        private T CheckFieldForNull<T>(int? id, string name, T model)
        {
            if (id != null || string.IsNullOrEmpty(name))
            {
                return default(T);
            }
            return model;
        }

        private T CheckFieldForNull<T>(int? id, string firstField, string secondField, T model)
        {
            if (id != null || (string.IsNullOrEmpty(firstField) && string.IsNullOrEmpty(secondField)))
            {
                return default(T);
            }
            return model;
        }
        private async Task<string> UploadPhotoAsyncInFolder(string userId, IFormFile file)
        {
            var oldImageName = (await _repoWrapper.User.GetFirstOrDefaultAsync(x => x.Id == userId)).ImagePath;
            if (file != null && file.Length > 0)
            {

                using (var img = System.Drawing.Image.FromStream(file.OpenReadStream()))
                {
                    var uploads = Path.Combine(_env.WebRootPath, "images\\Users");
                    if (!string.IsNullOrEmpty(oldImageName) && !string.Equals(oldImageName, "default.png"))
                    {
                        var oldPath = Path.Combine(uploads, oldImageName);
                        if (File.Exists(oldPath))
                        {
                            File.Delete(oldPath);
                        }
                    }
                    var fileName = $"{_uniqueId.GetUniqueId()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(uploads, fileName);
                    img.Save(filePath);
                    return fileName;
                }
            }
            else
            {
                return oldImageName;
            }
        }
        private async Task<string> UploadPhotoAsyncFromBase64(string userId, string imageBase64)
        {
            var oldImageName = (await _repoWrapper.User.GetFirstOrDefaultAsync(x => x.Id == userId)).ImagePath;
            if (!string.IsNullOrWhiteSpace(imageBase64) && imageBase64.Length > 0)
            {
                var base64Parts = imageBase64.Split(',');
                var ext = base64Parts[0].Split(new[] { '/', ';' }, 3)[1];
                var fileName = $"{_uniqueId.GetUniqueId()}.{ext}";
                await _userBlobStorage.UploadBlobForBase64Async(base64Parts[1], fileName);
                if (!string.IsNullOrEmpty(oldImageName) && !string.Equals(oldImageName, "default_user_image.png"))
                {
                    await _userBlobStorage.DeleteBlobAsync(oldImageName);
                }

                return fileName;
            }
            else
            {
                return oldImageName;
            }

        }
        /// <inheritdoc />
        private async Task UpdateAsync(UserDTO user, int? placeOfStudyId, int? specialityId, int? placeOfWorkId, int? positionId)
        {
            user.UserProfile.Nationality = CheckFieldForNull(user.UserProfile.NationalityId, user.UserProfile.Nationality.Name, user.UserProfile.Nationality);
            user.UserProfile.Religion = CheckFieldForNull(user.UserProfile.ReligionId, user.UserProfile.Religion.Name, user.UserProfile.Religion);
            user.UserProfile.Degree = CheckFieldForNull(user.UserProfile.DegreeId, user.UserProfile.Degree.Name, user.UserProfile.Degree);
            user.UserProfile.EducationId = await CheckEducationFieldsAsync(user.UserProfile.Education.PlaceOfStudy, user.UserProfile.Education.Speciality, placeOfStudyId, specialityId);
            user.UserProfile.Education = CheckFieldForNull(user.UserProfile.EducationId, user.UserProfile.Education.PlaceOfStudy, user.UserProfile.Education.Speciality, user.UserProfile.Education);
            user.UserProfile.WorkId = await CheckWorkFieldsAsync(user.UserProfile.Work.PlaceOfwork, user.UserProfile.Work.Position, placeOfWorkId, positionId);
            user.UserProfile.Work = CheckFieldForNull(user.UserProfile.WorkId, user.UserProfile.Work.PlaceOfwork, user.UserProfile.Work.Position, user.UserProfile.Work);
            var userForUpdate = _mapper.Map<UserDTO, User>(user);
            _repoWrapper.User.Update(userForUpdate);
            _repoWrapper.UserProfile.Update(userForUpdate.UserProfile);
            await _repoWrapper.SaveAsync();
        }

        private UserDTO SaveCorrectLinks(UserDTO user)
        {
            user.UserProfile.FacebookLink = SaveCorrectLink(user.UserProfile.FacebookLink, "facebook");
            user.UserProfile.TwitterLink = SaveCorrectLink(user.UserProfile.TwitterLink, "twitter");
            user.UserProfile.InstagramLink = SaveCorrectLink(user.UserProfile.InstagramLink, "instagram");

            return user;
        }
        private string SaveCorrectLink(string link, string socialMediaName)
        {
            if (link != null && link != "")
            {
                if (link.Contains($"www.{socialMediaName}.com/"))
                {
                    if (link.Contains("https://"))
                    {
                        link = link.Substring(8);
                    }
                    link = link.Substring(socialMediaName.Length + 9);
                }               
                else if (link.Contains($"{socialMediaName}.com/"))
                {
                    if (link.Contains("https://"))
                    {
                        link = link.Substring(8);
                    }
                    link = link.Substring(socialMediaName.Length + 5);
                }
                return link;
            }
            return link;
        }

        public async Task<bool> IsApprovedCityMember(string userId)
        {
            var cityMember = await _repoWrapper.CityMembers
                 .GetFirstOrDefaultAsync(u => u.UserId == userId, m => m.Include(u => u.User));
            return cityMember!=null && cityMember.IsApproved;
        }
    }
}
