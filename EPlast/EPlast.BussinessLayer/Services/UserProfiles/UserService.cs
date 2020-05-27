using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.UserProfiles
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _env;
        public UserService(IRepositoryWrapper repoWrapper, UserManager<User> userManager, IMapper mapper, IHostingEnvironment env)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _mapper = mapper;
            _env = env;
        }
        public UserDTO GetUser(string userId)
        {
            var user = _repoWrapper.User.
                FindByCondition(q => q.Id == userId).
                Include(i => i.UserProfile).
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
                Include(x => x.ConfirmedUsers).
                        ThenInclude(q => (q as ConfirmedUser).Approver).
                        ThenInclude(q => q.User).
                    FirstOrDefault();
            var model = _mapper.Map<User, UserDTO>(user);
            return model;
        }

        public IEnumerable<ConfirmedUserDTO> GetConfirmedUsers(UserDTO user)
        {
            var result = user.ConfirmedUsers.Where(x => x.isCityAdmin == false && x.isClubAdmin == false);
            return result;
        }

        public ConfirmedUserDTO GetClubAdminConfirmedUser(UserDTO user)
        {
            var result = user.ConfirmedUsers.FirstOrDefault(x => x.isClubAdmin == true);
            return result;
        }

        public ConfirmedUserDTO GetCityAdminConfirmedUser(UserDTO user)
        {
            var result = user.ConfirmedUsers.FirstOrDefault(x => x.isCityAdmin == true);
            return result;
        }
        public bool CanApprove(IEnumerable<ConfirmedUserDTO> confUsers, string userId, ClaimsPrincipal user)
        {
            var currentUserId = _userManager.GetUserId(user);

            var canApprove = confUsers.Count() < 3
                    && !confUsers.Any(x => x.Approver.UserID == currentUserId)
                    && !(currentUserId == userId);
            return canApprove;
        }
        public async Task<TimeSpan> CheckOrAddPlastunRole(string userId, DateTime registeredOn)
        {
            try
            {
                var timeToJoinPlast = registeredOn.AddYears(1) - DateTime.Now;
                if (timeToJoinPlast <= TimeSpan.Zero)
                {
                    var us = await _userManager.FindByIdAsync(userId);
                    await _userManager.AddToRoleAsync(us, "Пластун");
                    return TimeSpan.Zero;
                }
                return timeToJoinPlast;
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }
        public void Update(UserDTO user, IFormFile file, int? placeOfStudyId, int? specialityId, int? placeOfWorkId, int? positionId)
        {

            UploadPhoto(ref user, file);
            user.UserProfile.Nationality = CheckFieldForNull(user.UserProfile.NationalityId, user.UserProfile.Nationality.Name, user.UserProfile.Nationality);
            user.UserProfile.Religion = CheckFieldForNull(user.UserProfile.ReligionId, user.UserProfile.Religion.Name, user.UserProfile.Religion);
            user.UserProfile.Degree = CheckFieldForNull(user.UserProfile.DegreeId, user.UserProfile.Degree.Name, user.UserProfile.Degree);
            user.UserProfile.EducationId = CheckEducationFields(user.UserProfile.Education.PlaceOfStudy, user.UserProfile.Education.Speciality, placeOfStudyId, specialityId);
            user.UserProfile.Education = CheckFieldForNull(user.UserProfile.EducationId, user.UserProfile.Education.PlaceOfStudy, user.UserProfile.Education.Speciality, user.UserProfile.Education);
            user.UserProfile.WorkId = CheckWorkFields(user.UserProfile.Work.PlaceOfwork, user.UserProfile.Work.Position, placeOfWorkId, positionId);
            user.UserProfile.Work = CheckFieldForNull(user.UserProfile.WorkId, user.UserProfile.Work.PlaceOfwork, user.UserProfile.Work.Position, user.UserProfile.Work);
            var userForUpdate = _mapper.Map<UserDTO, User>(user);
            _repoWrapper.User.Update(userForUpdate);
            _repoWrapper.UserProfile.Update(userForUpdate.UserProfile);
            _repoWrapper.Save();
        }

        private int? CheckEducationFields(string firstName, string secondName, int? firstId, int? secondId)
        {

            var spec = _repoWrapper.Education.FindByCondition(x => x.ID == secondId).FirstOrDefault();
            var placeStudy = _repoWrapper.Education.FindByCondition(x => x.ID == firstId).FirstOrDefault();
            if (secondId == firstId)
            {
                return secondId;
            }
            else
            {
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

        private int? CheckWorkFields(string firstName, string secondName, int? firstId, int? secondId)
        {
            var placefWork = _repoWrapper.Work.FindByCondition(x => x.ID == firstId).FirstOrDefault();
            var position = _repoWrapper.Work.FindByCondition(x => x.ID == secondId).FirstOrDefault();
            if (secondId == firstId)
            {
                return secondId;
            }
            else
            {
                if (position != null && position.PlaceOfwork == firstName)
                {
                    return position.ID;
                }
                else if (placefWork != null && placefWork.Position == secondName)
                {
                    return placefWork.ID;
                }
                else
                {
                    return null;
                }
            }
        }

        private T CheckFieldForNull<T>(int? id, string name, T model)
        {
            if (!(id == null) || string.IsNullOrEmpty(name))
            {
                return default(T);
            }
            return model;
        }

        private T CheckFieldForNull<T>(int? id, string firstField, string secondField, T model)
        {
            if (!(id == null) || (string.IsNullOrEmpty(firstField) && string.IsNullOrEmpty(secondField)))
            {
                return default(T);
            }
            return model;
        }

        private void UploadPhoto(ref UserDTO user, IFormFile file)
        {
            var userId = user.Id;
            var oldImageName = _repoWrapper.User.FindByCondition(i => i.Id == userId).FirstOrDefault().ImagePath;
            if (file != null && file.Length > 0)
            {
                var img = Image.FromStream(file.OpenReadStream());
                var uploads = Path.Combine(_env.WebRootPath, "images\\Users");
                if (!string.IsNullOrEmpty(oldImageName) && !string.Equals(oldImageName, "default.png"))
                {
                    var oldPath = Path.Combine(uploads, oldImageName);
                    if (File.Exists(oldPath))
                    {
                        File.Delete(oldPath);
                    }
                }

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, fileName);
                img.Save(filePath);
                user.ImagePath = fileName;
            }
            else
            {
                user.ImagePath = oldImageName;
            }
        }
    }
}
