using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO.PrecautionsDTO;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.BLL.Queries.Precaution;
using EPlast.BLL.Services.UserAccess;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EPlast.BLL.Services.Precautions
{
    public class UserPrecautionService : IUserPrecautionService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IAdminService _adminService;
        private readonly IMediator _mediator;

        public UserPrecautionService(IMapper mapper, IRepositoryWrapper repoWrapper,
            UserManager<User> userManager, IAdminService adminService, IMediator mediator)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _adminService = adminService;
            _mediator = mediator;
        }

        private async Task<bool> CanUserAddPrecaution(UserPrecautionDTO userPrecautionDto, User user)
        {
            var precautionUser = await _userManager.FindByIdAsync(userPrecautionDto.UserId);

            bool isUserInPrecautionGoverningBodyAdmin =
                await _userManager.IsInRoleAsync(precautionUser, Roles.GoverningBodyAdmin);

            bool isCreatorGoverningBodyAdmin = await _userManager.IsInRoleAsync(user, Roles.GoverningBodyAdmin);

            if (isUserInPrecautionGoverningBodyAdmin && isCreatorGoverningBodyAdmin)
            {
                return false;
            }

            var roles = await _userManager.GetRolesAsync(precautionUser);
            var isInLowerRole = roles.Intersect(Roles.LowerRoles).Any();

            if (isInLowerRole)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddUserPrecautionAsync(UserPrecautionDTO userPrecautionDTO, User user)
        {
            bool canUserAddPrecaution = await CanUserAddPrecaution(userPrecautionDTO, user);

            if (!canUserAddPrecaution)
            {
                return false;
            }

            bool existNumber = await IsNumberExistAsync(userPrecautionDTO.Number, userPrecautionDTO.Id);
            if (existNumber)
            {
                return false;
            }

            bool isActive =
                await CheckUserPrecautionsType(userPrecautionDTO.UserId, userPrecautionDTO.Precaution.Name);
            if (isActive)
            {
                return false;
            }

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
            return true;

        }



        private DateTime GetPrecautionEndDate(int precautionId, DateTime startDate)
        {
            if (precautionId == 1) { return startDate.AddMonths(3); }
            return precautionId == 2 ? startDate.AddMonths(6) : startDate.AddMonths(12);
        }

        private async Task<bool> CanUserChangePrecautionAsync(int precautionId, User user)
        {
            var precaution = await _repoWrapper.UserPrecaution.GetFirstOrDefaultAsync(p => p.Id == precautionId);
            if (precaution == null)
            {
                return false;
            }

            var precautionUser = await _userManager.FindByIdAsync(precaution.UserId);
            bool isUserInPrecautionGoverningBodyAdmin =
                await _userManager.IsInRoleAsync(precautionUser, Roles.GoverningBodyAdmin);
            bool isCurrentUserGoverningBodyAdmin = await _userManager.IsInRoleAsync(user, Roles.GoverningBodyAdmin);

            if (isUserInPrecautionGoverningBodyAdmin && isCurrentUserGoverningBodyAdmin)
            {
                return false;
            }

            if (isCurrentUserGoverningBodyAdmin && !precaution.IsActive)
            {
                return false;
            }

            return true;

        }

        public async Task<bool> ChangeUserPrecautionAsync(UserPrecautionDTO userPrecautionDTO, User user)
        {
                bool canUserChangePrecautionAsync = await CanUserChangePrecautionAsync(userPrecautionDTO.Id, user);

                if (!canUserChangePrecautionAsync)
                {
                    return false;
                }

                bool existRegisterNumber = await IsNumberExistAsync(userPrecautionDTO.Number, userPrecautionDTO.Id);
                if (existRegisterNumber)
                {
                    return false;
                }

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
                return true;
            
        }

        public async Task<bool> DeleteUserPrecautionAsync(int id, User user)
        {
            var userPrecaution = await _repoWrapper.UserPrecaution.GetFirstOrDefaultAsync(d => d.Id == id);
            if (userPrecaution == null)
            {
                return false;
            }

            bool canUserDeletePrecaution = await CanUserChangePrecautionAsync(userPrecaution.Id, user);

            if (!canUserDeletePrecaution)
            {
                return false;
            }

            _repoWrapper.UserPrecaution.Delete(userPrecaution);
            await _repoWrapper.SaveAsync();
            return true;
        }

        public async Task<UserPrecautionsTableInfo> GetUserPrecautionsForTableAsync(PrecautionTableSettings tableSettings)
        {
            var query = new GetUsersPrecautionsForTableQuery(tableSettings);
            var precautionsTuple = await _mediator.Send(query);
            var allInfoPrecautions = precautionsTuple.Item1.ToList();

            var tableInfo = new UserPrecautionsTableInfo
            {
                TotalItems = precautionsTuple.Item2,
                UserPrecautions = allInfoPrecautions
            };

            return tableInfo;
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

        public async Task<bool> IsNumberExistAsync(int number, int? id = null)
        {                        
            var distNum = await _repoWrapper.UserPrecaution.GetFirstOrDefaultAsync(x => x.Number == number);
            if (distNum == null)
            {
                return false;
            }

            return distNum.Id != id;   
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
        public async Task<bool> CheckUserPrecautionsType(string userId, string type)
        {
            return (await GetUserPrecautionsOfUserAsync(userId)).Any(x => x.IsActive && x.Precaution.Name.Equals(type));
        }

        public async Task<UserPrecautionDTO> GetUserActivePrecaution(string userId, string type)
        {
            return (await GetUserPrecautionsOfUserAsync(userId)).FirstOrDefault(x => x.IsActive && x.Precaution.Name.Equals(type));
        }

        public async Task<IEnumerable<SuggestedUserDto>> GetUsersForPrecautionAsync(User currentUser)
        {
            bool isCreatorGoverningBodyAdmin = await _userManager.IsInRoleAsync(currentUser, Roles.GoverningBodyAdmin);
            var allUsers = await _repoWrapper.User.GetAllAsync();
            var suggestedUsers = new List<SuggestedUserDto>();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var isInLowerRole = roles.Intersect(Roles.LowerRoles).Any();
                
                var suggestedUser = _mapper.Map<User, SuggestedUserDto>(user);

                if (isCreatorGoverningBodyAdmin)
                {
                    suggestedUser.IsAvailable = !isInLowerRole && !roles.Contains(Roles.GoverningBodyAdmin);
                }
                else
                {
                    suggestedUser.IsAvailable = !isInLowerRole;
                }
               
                suggestedUsers.Add(suggestedUser);
            }

            suggestedUsers = suggestedUsers.OrderBy(u => !u.IsAvailable).ToList();

            return suggestedUsers;
        }

    }
}
