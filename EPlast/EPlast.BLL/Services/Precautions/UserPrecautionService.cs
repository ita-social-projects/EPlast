using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using EPlast.Resources;
using System.Threading.Tasks;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.Services.Precautions
{
    public class UserPrecautionService : IUserPrecautionService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IAdminService _adminService;

        public UserPrecautionService(IMapper mapper, IRepositoryWrapper repoWrapper, UserManager<User> userManager, IAdminService adminService)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _adminService = adminService;
        }

        public async Task AddUserPrecautionAsync(UserPrecautionDTO userPrecautionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var userPrecaution = new UserPrecaution()
            {
                UserId = userPrecautionDTO.UserId,
                PrecautionId = userPrecautionDTO.PrecautionId,
                Date = userPrecautionDTO.Date,
                Reason = userPrecautionDTO.Reason,
                Reporter = userPrecautionDTO.Reporter,
                Number = userPrecautionDTO.Number,
                Status = userPrecautionDTO.Status,
                EndDate = GetPrecautionEndDate(userPrecautionDTO.PrecautionId, userPrecautionDTO.Date),
                IsActive = userPrecautionDTO.IsActive
            };
            await _repoWrapper.UserPrecaution.CreateAsync(userPrecaution);
            await _repoWrapper.SaveAsync();
        }

        private DateTime GetPrecautionEndDate(int precautionId, DateTime startDate)
        {
            if (precautionId == 1) { return startDate.AddMonths(3); }
            return precautionId == 2 ? startDate.AddMonths(6) : startDate.AddMonths(12);
        }

        public async Task ChangeUserPrecautionAsync(UserPrecautionDTO userPrecautionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var userPrecaution = new UserPrecaution()
            {
                Id = userPrecautionDTO.Id,
                UserId = userPrecautionDTO.UserId,
                PrecautionId = userPrecautionDTO.PrecautionId,
                Date = userPrecautionDTO.Date,
                Reason = userPrecautionDTO.Reason,
                Reporter = userPrecautionDTO.Reporter,
                Number = userPrecautionDTO.Number,
                Status = userPrecautionDTO.Status,
                EndDate = userPrecautionDTO.EndDate,
                IsActive = userPrecautionDTO.IsActive
            };
            _repoWrapper.UserPrecaution.Update(userPrecaution);
            await _repoWrapper.SaveAsync();
        }

        public async Task DeleteUserPrecautionAsync(int id, User user)
        {
            await CheckIfAdminAsync(user);
            var userPrecaution = await _repoWrapper.UserPrecaution.GetFirstOrDefaultAsync(d => d.Id == id);
            if (userPrecaution == null)
                throw new NotImplementedException();
            _repoWrapper.UserPrecaution.Delete(userPrecaution);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<UserPrecautionDTO>> GetAllUsersPrecautionAsync()
        {
            var userPrecautions = await _repoWrapper.UserPrecaution.GetAllAsync(include:
                source => source
                .Include(c => c.User)
                .Include(d => d.Precaution)
                );
            var precautions = await CheckEndDateAsync(userPrecautions);
            return _mapper.Map<IEnumerable<UserPrecaution>, IEnumerable<UserPrecautionDTO>>(precautions);
        }

        public async Task<UserPrecautionDTO> GetUserPrecautionAsync(int id)
        {
            var userPrecaution = await _repoWrapper.UserPrecaution.GetFirstOrDefaultAsync(d => d.Id == id, include:
                source => source
                .Include(c => c.User)
                .Include(d => d.Precaution));
            return _mapper.Map<UserPrecaution, UserPrecautionDTO>(userPrecaution);
        }

        public async Task<IEnumerable<UserPrecautionDTO>> GetUserPrecautionsOfUserAsync(string UserId)
        {
            var userPrecautions = await _repoWrapper.UserPrecaution.GetAllAsync(u => u.UserId == UserId,
                include: source => source
                .Include(c => c.User)
                .Include(d => d.Precaution));
            return _mapper.Map<IEnumerable<UserPrecaution>, IEnumerable<UserPrecautionDTO>>(userPrecautions);
        }

        public async Task<bool> IsNumberExistAsync(int number)
        {
            var distNum = await _repoWrapper.UserPrecaution.GetFirstOrDefaultAsync(x => x.Number == number);
            return distNum != null;
        }

        public async Task CheckIfAdminAsync(User user)
        {
            if (!(await _userManager.GetRolesAsync(user)).Contains(Roles.Admin))
                throw new UnauthorizedAccessException();
        }

        private async Task<IEnumerable<UserPrecaution>> CheckEndDateAsync(IEnumerable<UserPrecaution> userPrecaution)
        {
            if (userPrecaution != null)
            {
                foreach (var item in userPrecaution)
                {
                    if (item.EndDate < DateTime.Now && item.IsActive)
                    {
                        item.IsActive = false;
                        _repoWrapper.UserPrecaution.Update(item);
                        await _repoWrapper.SaveAsync();
                    }
                }
            }
            return userPrecaution;
        }

        public async Task<IEnumerable<ShortUserInformationDTO>> UsersTableWithoutPrecautionAsync()
        {
            var usersWithoutPrecautions = await _adminService.GetUsersAsync();
            foreach (var user in usersWithoutPrecautions)
            {
                user.IsInLowerRole = !user.IsInLowerRole ? await CheckUserPrecautions(user.ID) : user.IsInLowerRole;
            }
            return usersWithoutPrecautions;
        }

        private async Task<bool> CheckUserPrecautions(string userId)
        {
            return (await GetUserPrecautionsOfUserAsync(userId)).Any(x => x.IsActive);
        }
    }
}
