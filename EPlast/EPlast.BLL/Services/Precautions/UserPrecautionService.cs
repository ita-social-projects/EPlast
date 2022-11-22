using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.PrecautionsDTO;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Queries.Precaution;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        private async Task<bool> CanUserAddPrecaution(UserPrecautionDto userPrecautionDto, User user)
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

        public async Task<bool> AddUserPrecautionAsync(UserPrecautionDto userPrecautionDTO, User user)
        {
            bool canUserAddPrecaution = await CanUserAddPrecaution(userPrecautionDTO, user);

            if (!canUserAddPrecaution)
            {
                return false;
            }

            bool existNumber = await DoesPrecautionExistAsync(userPrecautionDTO.Id);
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
            };
            await _repoWrapper.UserPrecaution.CreateAsync(userPrecaution);
            await _repoWrapper.SaveAsync();
            return true;

        }


        private async Task<bool> CanUserChangePrecautionAsync(int precautionId, User user)
        {
            var userPrecaution = await _repoWrapper
                .UserPrecaution
                .GetFirstOrDefaultAsync
                (
                    predicate: d => d.Id == precautionId,
                    include: up => up.Include(p => p.Precaution)
                );

            if (userPrecaution == null)
            {
                return false;
            }

            bool isCurrentUserGoverningBodyAdmin = await _userManager.IsInRoleAsync(user, Roles.GoverningBodyAdmin);
            if (isCurrentUserGoverningBodyAdmin && !userPrecaution.IsActive)
            {
                return false;
            }

            return true;

        }

        public async Task<bool> ChangeUserPrecautionAsync(UserPrecautionDto userPrecautionDTO, User user)
        {
            bool canUserChangePrecautionAsync = await CanUserChangePrecautionAsync(userPrecautionDTO.Id, user);
            if (!canUserChangePrecautionAsync)
            {
                return false;
            }

            bool existRegisterNumber = await DoesPrecautionExistAsync(userPrecautionDTO.Id);
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
            };

            _repoWrapper.UserPrecaution.Update(userPrecaution);
            await _repoWrapper.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteUserPrecautionAsync(int id, User user)
        {
            var userPrecaution = await _repoWrapper
                .UserPrecaution
                .GetFirstOrDefaultAsync
                (
                    predicate: d => d.Id == id,
                    include: up => up.Include(p => p.Precaution)
                );
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

        public async Task<IEnumerable<UserPrecautionDto>> GetAllUsersPrecautionAsync()
        {
            var userPrecautions = await _repoWrapper.UserPrecaution.GetAllAsync(include:
                source => source
                .Include(c => c.User)
                .Include(d => d.Precaution)
                );

            return _mapper.Map<IEnumerable<UserPrecaution>, IEnumerable<UserPrecautionDto>>(userPrecautions);
        }

        public async Task<UserPrecautionDto> GetUserPrecautionAsync(int id)
        {
            var userPrecaution = await _repoWrapper.UserPrecaution.GetFirstOrDefaultAsync(d => d.Id == id, include:
                source => source
                .Include(c => c.User)
                .Include(d => d.Precaution));
            return _mapper.Map<UserPrecaution, UserPrecautionDto>(userPrecaution);
        }

        public async Task<IEnumerable<UserPrecautionDto>> GetUserPrecautionsOfUserAsync(string UserId)
        {
            var userPrecautions = await _repoWrapper.UserPrecaution.GetAllAsync(u => u.UserId == UserId,
                include: source => source
                .Include(c => c.User)
                .Include(d => d.Precaution));
            return _mapper.Map<IEnumerable<UserPrecaution>, IEnumerable<UserPrecautionDto>>(userPrecautions);
        }

        public async Task<bool> DoesPrecautionExistAsync(int id)
        {
            var userPrecaution = await _repoWrapper
                .UserPrecaution
                .GetFirstOrDefaultAsync
                (
                    predicate: d => d.Id == id,
                    include: up => up.Include(p => p.Precaution)
                );
            if (userPrecaution == null)
            {
                return false;
            }

            return userPrecaution.Id != id;
        }

        public async Task<bool> DoesNumberExistAsync(int number)
        {
            var userPrecaution = await _repoWrapper
                .UserPrecaution
                .GetFirstOrDefaultAsync
                (
                    predicate: up => up.Number == number
                );
            return userPrecaution != null;
        }

        public async Task<IEnumerable<ShortUserInformationDto>> UsersTableWithoutPrecautionAsync()
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

        public async Task<UserPrecautionDto> GetUserActivePrecaution(string userId, string type)
        {
            return (await GetUserPrecautionsOfUserAsync(userId)).FirstOrDefault(
                x => x.Date < DateTime.Now && DateTime.Now < x.Date.AddMonths(x.Precaution.MonthsPeriod)
                && x.Precaution.Name.Equals(type)
                && x.Status != UserPrecautionStatus.Cancelled
            );
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
                    suggestedUser.IsAvailable = !isInLowerRole && !roles.Contains(Roles.GoverningBodyAdmin) &&
                                                !roles.Contains(Roles.Admin);
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
