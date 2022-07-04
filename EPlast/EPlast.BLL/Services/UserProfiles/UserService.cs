using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.Notification;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Services.UserProfiles
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IUserManagerService _userManagerService;
        private readonly IUserPersonalDataService _userPersonalDataService;
        private readonly IWebHostEnvironment _env;
        private readonly IUserBlobStorageRepository _userBlobStorage;
        private readonly INotificationService _notificationService;
        private readonly UserManager<User> _userManager;

        public UserService(IRepositoryWrapper repoWrapper,
            IMapper mapper,
            IUserPersonalDataService userPersonalDataService,
            IUserBlobStorageRepository userBlobStorage,
            IWebHostEnvironment env,
            IUserManagerService userManagerService,
            INotificationService notificationService,
            UserManager<User> userManager
        )
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _userPersonalDataService = userPersonalDataService;
            _userBlobStorage = userBlobStorage;
            _userManagerService = userManagerService;
            _env = env;
            _notificationService = notificationService;
            _userManager = userManager;
        }

        /// <inheritdoc />
        public async Task<UserDto> GetUserAsync(string userId)
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
                        ThenInclude(g => g.Region).
                    Include(g => g.ClubMembers).
                        ThenInclude(g => g.Club).
                    Include(g => g.UserProfile).
                        ThenInclude(g => g.UpuDegree).
                    Include(x => x.ConfirmedUsers).
                        ThenInclude(q => (q as ConfirmedUser).Approver).
                            ThenInclude(q => q.User));
            var model = _mapper.Map<User, UserDto>(user);

            return model;
        }

        /// <inheritdoc />
        public IEnumerable<ConfirmedUserDto> GetConfirmedUsers(UserDto user)
        {
            var result = user.ConfirmedUsers.
                Where(x => !x.IsCityAdmin && !x.IsClubAdmin);
            return result;
        }

        /// <inheritdoc />
        public ConfirmedUserDto GetClubAdminConfirmedUser(UserDto user)
        {
            var result = user.ConfirmedUsers.
                FirstOrDefault(x => x.IsClubAdmin);

            return result;
        }

        /// <inheritdoc />
        public ConfirmedUserDto GetCityAdminConfirmedUser(UserDto user)
        {
            var result = user.ConfirmedUsers.
                FirstOrDefault(x => x.IsCityAdmin);

            return result;
        }

        /// <inheritdoc />
        public bool CanApprove(IEnumerable<ConfirmedUserDto> confUsers, string userId, string currentUserId, bool isAdmin = false)
        {
            return confUsers.Count() < 3 && !confUsers.Any(x => x.Approver.UserID == currentUserId)
                                         && (currentUserId != userId || isAdmin);
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

        public async Task UpdateAsyncForFile(UserDto user, IFormFile file, int? placeOfStudyId, int? specialityId, int? placeOfWorkId, int? positionId)
        {
            user.ImagePath = await UploadPhotoAsyncInFolder(user.Id, file);
            await UpdateAsync(user, placeOfStudyId, specialityId, placeOfWorkId, positionId);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsyncForBase64(UserDto user, string base64, int? placeOfStudyId, int? specialityId, int? placeOfWorkId, int? positionId)
        {
            user = SaveCorrectLinks(user);
            user.ImagePath ??= await UploadPhotoAsyncFromBase64(user.Id, base64);
            await UpdateAsync(user, placeOfStudyId, specialityId, placeOfWorkId, positionId);
            await _repoWrapper.SaveAsync();
        }

        /// <inheritdoc />
        public async Task UpdatePhotoAsyncForBase64(UserDto user, string photoBase64)
        {
            user.ImagePath = await UploadPhotoAsyncFromBase64(user.Id, photoBase64);
            var userForUpdate = _mapper.Map<UserDto, User>(user);
            _repoWrapper.User.Update(userForUpdate);
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
                return default;
            }
            return model;
        }

        private T CheckFieldForNull<T>(int? id, string firstField, string secondField, T model)
        {
            if (id != null || (string.IsNullOrEmpty(firstField) && string.IsNullOrEmpty(secondField)))
            {
                return default;
            }
            return model;
        }

        private async Task<string> UploadPhotoAsyncInFolder(string userId, IFormFile file)
        {
            var oldImageName = (await _repoWrapper.User.GetFirstOrDefaultAsync(x => x.Id == userId)).ImagePath;
            if (file != null && file.Length > 0)
            {
                using var img = System.Drawing.Image.FromStream(file.OpenReadStream());
                var uploads = Path.Combine(_env.WebRootPath, "images\\Users");
                if (!string.IsNullOrEmpty(oldImageName) && !string.Equals(oldImageName, "default.png"))
                {
                    var oldPath = Path.Combine(uploads, oldImageName);
                    if (File.Exists(oldPath))
                    {
                        File.Delete(oldPath);
                    }
                }
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploads, fileName);
                img.Save(filePath);
                return fileName;
            }
            else
            {
                return oldImageName;
            }
        }

        private async Task<string> UploadPhotoAsyncFromBase64(string userId, string imageBase64)
        {
            var oldImageName = (await _repoWrapper.User.GetFirstOrDefaultAsync(x => x.Id == userId)).ImagePath;
            if (!string.IsNullOrWhiteSpace(imageBase64))
            {
                var base64Parts = imageBase64.Split(',');
                var ext = base64Parts[0].Split(new[] { '/', ';' }, 3)[1];
                var fileName = $"{Guid.NewGuid()}.{ext}";
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
        private async Task UpdateAsync(UserDto user, int? placeOfStudyId, int? specialityId, int? placeOfWorkId, int? positionId)
        {
            user.UserProfile.Nationality = CheckFieldForNull(user.UserProfile.NationalityId, user.UserProfile.Nationality.Name, user.UserProfile.Nationality);
            user.UserProfile.Religion = CheckFieldForNull(user.UserProfile.ReligionId, user.UserProfile.Religion.Name, user.UserProfile.Religion);
            user.UserProfile.Degree = CheckFieldForNull(user.UserProfile.DegreeId, user.UserProfile.Degree.Name, user.UserProfile.Degree);
            user.UserProfile.EducationId = await CheckEducationFieldsAsync(user.UserProfile.Education.PlaceOfStudy, user.UserProfile.Education.Speciality, placeOfStudyId, specialityId);
            user.UserProfile.Education = CheckFieldForNull(user.UserProfile.EducationId, user.UserProfile.Education.PlaceOfStudy, user.UserProfile.Education.Speciality, user.UserProfile.Education);
            user.UserProfile.WorkId = await CheckWorkFieldsAsync(user.UserProfile.Work.PlaceOfwork, user.UserProfile.Work.Position, placeOfWorkId, positionId);
            user.UserProfile.Work = CheckFieldForNull(user.UserProfile.WorkId, user.UserProfile.Work.PlaceOfwork, user.UserProfile.Work.Position, user.UserProfile.Work);
            var userForUpdate = _mapper.Map<UserDto, User>(user);
            _repoWrapper.User.Update(userForUpdate);
            _repoWrapper.UserProfile.Update(userForUpdate.UserProfile);
            await _repoWrapper.SaveAsync();
        }

        private UserDto SaveCorrectLinks(UserDto user)
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
                        link = link[8..];
                    }
                    link = link[(socialMediaName.Length + 9)..];
                }
                else if (link.Contains($"{socialMediaName}.com/"))
                {
                    if (link.Contains("https://"))
                    {
                        link = link[8..];
                    }
                    link = link[(socialMediaName.Length + 5)..];
                }
                return link;
            }
            return link;
        }

        public async Task<bool> IsApprovedCityMember(string userId)
        {
            var cityMember = await _repoWrapper.CityMembers
                 .GetFirstOrDefaultAsync(u => u.UserId == userId, m => m.Include(u => u.User));
            return cityMember != null && cityMember.IsApproved;
        }

        public async Task<bool> IsApprovedCLubMember(string userId)
        {
            var clubMember = await _repoWrapper.ClubMembers
                .GetFirstOrDefaultAsync(u => u.UserId == userId, m => m.Include(u => u.User));
            return clubMember != null && clubMember.IsApproved;
        }

        public async Task<string> GetUserGenderAsync(string userId)
        {
            var user = await _repoWrapper.User.GetFirstAsync(
                i => i.Id == userId,
                i =>
                    i.Include(g => g.UserProfile).ThenInclude(x => x.Gender));
            return user.UserProfile.Gender == null ? UserGenders.Undefined : user.UserProfile.Gender.Name;
        }

        public bool IsUserSameCity(UserDto currentUser, UserDto focusUser)
        {
            return currentUser.CityMembers.FirstOrDefault()?.CityId
                       .Equals(focusUser.CityMembers.FirstOrDefault()?.CityId)
                   == true;
        }

        public bool IsUserSameClub(UserDto currentUser, UserDto focusUser)
        {
            return currentUser.ClubMembers.FirstOrDefault()?.ClubId
                       .Equals(focusUser.ClubMembers.FirstOrDefault()?.ClubId)
                   == true;
        }

        public bool IsUserSameRegion(UserDto currentUser, UserDto focusUser)
        {
            return currentUser.RegionAdministrations.FirstOrDefault()?.RegionId
                       .Equals(focusUser.RegionAdministrations.FirstOrDefault()?.RegionId) == true
                   || currentUser.CityMembers.FirstOrDefault()?.City.RegionId
                       .Equals(focusUser.CityMembers.FirstOrDefault()?.City.RegionId) == true;
        }

        public async Task<bool> IsUserInClubAsync(UserDto currentUser, UserDto focusUser)
        {
            var isUserHeadOfClub = await _userManagerService.IsInRoleAsync(currentUser, Roles.KurinHead);
            var isUserHeadDeputyOfClub = await _userManagerService.IsInRoleAsync(currentUser, Roles.KurinHeadDeputy);
            var isFocusUserPlastun = await _userManagerService.IsInRoleAsync(focusUser, Roles.PlastMember)
                                     || !(await IsApprovedCLubMember(focusUser.Id));
            bool sameClub = IsUserSameClub(currentUser, focusUser);
            return ((isUserHeadDeputyOfClub && sameClub) || (isUserHeadOfClub && sameClub) || (isFocusUserPlastun && sameClub));
        }

        public async Task<bool> IsUserInCityAsync(UserDto currentUser, UserDto focusUser)
        {
            var isUserHeadOfCity = await _userManagerService.IsInRoleAsync(currentUser, Roles.CityHead);
            var isUserHeadDeputyOfCity = await _userManagerService.IsInRoleAsync(currentUser, Roles.CityHeadDeputy);
            var isFocusUserPlastun = await _userManagerService.IsInRoleAsync(focusUser, Roles.PlastMember)
                                     || !(await IsApprovedCityMember(focusUser.Id));
            bool sameCity = IsUserSameCity(currentUser, focusUser);
            return ((isUserHeadDeputyOfCity && sameCity) || (isUserHeadOfCity && sameCity) || (isFocusUserPlastun && sameCity));
        }

        public async Task<bool> IsUserInRegionAsync(UserDto currentUser, UserDto focusUser)
        {
            var isUserHeadOfRegion = await _userManagerService.IsInRoleAsync(currentUser, Roles.OkrugaHead);
            var isUserHeadDeputyOfRegion = await _userManagerService.IsInRoleAsync(currentUser, Roles.OkrugaHeadDeputy);
            bool sameRegion = IsUserSameRegion(currentUser, focusUser);
            return ((isUserHeadDeputyOfRegion && sameRegion) || (isUserHeadOfRegion && sameRegion));
        }

        public async Task<bool> IsUserInSameCellAsync(UserDto currentUser, UserDto focusUser, CellType cellType)
        {
            return cellType switch
            {
                CellType.City => IsUserSameCity(currentUser, focusUser) && await IsApprovedCityMember(focusUser.Id),
                CellType.Region => IsUserSameRegion(currentUser, focusUser) && await IsApprovedCityMember(focusUser.Id),
                CellType.Club => IsUserSameClub(currentUser, focusUser) && await IsApprovedCLubMember(focusUser.Id),
                _ => false
            };
        }

        public async Task CheckRegisteredUsersAsync()
        {
            var users = await _repoWrapper.User.GetAllAsync(
               predicate: x =>
               x.CityMembers.FirstOrDefault(y => y.UserId == x.Id) == null,
               include: x => x.Include(y => y.CityMembers)
           );
            var filteredUsers = users.Where(x => (DateTime.Now - x.RegistredOn).Days >= 7).ToList();

            if (filteredUsers.Any())
            {
                List<UserNotificationDto> userNotificationsDTO = new List<UserNotificationDto>();
                var governingBodyAdmins = await _userManager.GetUsersInRoleAsync(Roles.GoverningBodyAdmin);
                foreach (var userToCheck in filteredUsers)
                {
                    foreach (var u in governingBodyAdmins)
                    {
                        userNotificationsDTO.Add(new UserNotificationDto
                        {
                            Message = $"Користувачу {userToCheck.FirstName} {userToCheck.LastName} не обрали станицю уже 7 днів! ",
                            NotificationTypeId = 1,
                            OwnerUserId = u.Id,
                            SenderLink = $"/user/table?search={userToCheck.FirstName} {userToCheck.LastName}",
                            SenderName = "Переглянути"
                        });
                    }
                }
                await _notificationService.AddListUserNotificationAsync(userNotificationsDTO);
            }
        }

        public async Task CheckRegisteredWithoutCityUsersAsync()
        {
            var users = await _repoWrapper.User.GetAllAsync(
                predicate: x => !x.CityMembers.FirstOrDefault(y => y.UserId == x.Id).IsApproved,
                include: x => x.Include(y => y.CityMembers)
            );
            var filteredUsers = users.Where(x => (DateTime.Now - x.RegistredOn).Days >= 7).ToList();

            if (filteredUsers.Any())
            {
                List<UserNotificationDto> userNotificationsDTO = new List<UserNotificationDto>();
                foreach (var userToCheck in filteredUsers)
                {
                    var cityId = userToCheck.CityMembers.FirstOrDefault().CityId;
                    var userCity = await _repoWrapper.City.GetFirstOrDefaultAsync(x => x.ID == cityId);
                    var userRegion = await _repoWrapper.City.GetFirstOrDefaultAsync(x => x.Name == userCity.Name);
                    var regionAdministration = await _repoWrapper.RegionAdministration
               .GetAllAsync(i => i.RegionId == userRegion.RegionId,
                   i => i
                       .Include(c => c.AdminType)
                       .Include(a => a.User));

                    var regionHead = regionAdministration.FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.OkrugaHead
                                                                          && (DateTime.Now < a.EndDate || a.EndDate == null));
                    var regionHeadDeputy = regionAdministration.FirstOrDefault(a => a.AdminType.AdminTypeName == Roles.OkrugaHeadDeputy
                                                                         && (DateTime.Now < a.EndDate || a.EndDate == null));

                    if (regionHead != null)
                    {
                        userNotificationsDTO.Add(new UserNotificationDto
                        {
                            Message = $"Користувача {userToCheck.FirstName} {userToCheck.LastName} не додають в станицю {userCity.Name} уже 7 днів! ",
                            NotificationTypeId = 1,
                            OwnerUserId = regionHead.UserId,
                            SenderLink = $"/user/table?search={userToCheck.FirstName} {userToCheck.LastName}",
                            SenderName = "Переглянути"
                        });
                    }
                    if (regionHeadDeputy != null)
                    {
                        userNotificationsDTO.Add(new UserNotificationDto
                        {
                            Message = $"Користувача {userToCheck.FirstName} {userToCheck.LastName} не додають в станицю {userCity.Name} уже 7 днів! ",
                            NotificationTypeId = 1,
                            OwnerUserId = regionHeadDeputy.UserId,
                            SenderLink = $"/user/table?search={userToCheck.FirstName} {userToCheck.LastName}",
                            SenderName = "Переглянути"
                        });
                    }
                }
                await _notificationService.AddListUserNotificationAsync(userNotificationsDTO);
            }
        }
    }
}