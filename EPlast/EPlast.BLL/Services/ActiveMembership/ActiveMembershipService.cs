using AutoMapper;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.ActiveMembership
{
    /// <inheritdoc />
    public class ActiveMembershipService : IActiveMembershipService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IUserManagerService _userManagerService;
        public ActiveMembershipService(IMapper mapper, IRepositoryWrapper repoWrapper, IUserManagerService userManagerService)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _userManagerService = userManagerService;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PlastDegreeDTO>> GetDergeesAsync()
        {
            var degrees = await _repoWrapper.PlastDegrees.GetAllAsync();

            return _mapper.Map<IEnumerable<PlastDegreeDTO>>(degrees);
        }

        /// <inheritdoc />
        public async Task<DateTime> GetDateOfEntryAsync(string userId)
        {
            var userDTO = await _userManagerService.FindByIdAsync(userId);

            return userDTO.RegistredOn;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserPlastDegreeDTO>> GetUserPlastDegreesAsync(string userId)
        {
            var userPlastDegrees = await _repoWrapper.UserPlastDegrees.GetAllAsync(upd => upd.UserId == userId);

            return _mapper.Map<IEnumerable<UserPlastDegreeDTO>>(userPlastDegrees);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<string>> GetUserAccessLevelsAsync(string userId)
        {
            List<string> accessLevels = new List<string>();
            var user = await _userManagerService.FindByIdAsync(userId);
            List<string> userRoles = (await _userManagerService.GetRolesAsync(user)).ToList();
            if (userRoles.Count == 1)
            {
                if (userRoles[0] == RolesForActiveMembershipTypeDTO.Plastun.GetDescription())
                {
                    accessLevels.Add(AccessLevelTypeDTO.Member.GetDescription());
                }
                else if (userRoles[0] == RolesForActiveMembershipTypeDTO.Supporter.GetDescription())
                {
                    accessLevels.Add(AccessLevelTypeDTO.Supporter.GetDescription());
                }
            }
            else if (userRoles.Count > 1)
            {
                accessLevels.AddRange(new List<string>
                {
                    AccessLevelTypeDTO.Member.GetDescription(),
                    AccessLevelTypeDTO.LeadershipMember.GetDescription()
                });
            }
            else
            {
                accessLevels.Add(AccessLevelTypeDTO.FormerMember.GetDescription());
            }

            return accessLevels.AsEnumerable();
        }

        /// <inheritdoc />
        public async Task<bool> AddPlastDegreeForUserAsync(UserPlastDegreePostDTO userPlastDegreePostDTO)
        {
            bool isAdded = false;
            var userDto = await _userManagerService.FindByIdAsync(userPlastDegreePostDTO.UserId);
            if (userDto != null)
            {
                List<UserPlastDegree> userPlastDegrees = userDto.UserPlastDegrees.ToList();
                if (!userPlastDegrees.Any(upd => upd.PlastDegree.Id == userPlastDegreePostDTO.PlastDegreeId))
                {
                    UserPlastDegree userPlastDegree = _mapper.Map<UserPlastDegree>(userPlastDegreePostDTO);
                    userPlastDegree.User = _mapper.Map<User>(userDto);
                    PlastDegree plastDegree = await _repoWrapper.PlastDegrees.GetFirstOrDefaultAsync(pd => pd.Id == userPlastDegreePostDTO.PlastDegreeId);
                    if (plastDegree != null)
                    {
                        userPlastDegree.PlastDegree = plastDegree;
                        _repoWrapper.UserPlastDegrees.Attach(userPlastDegree);
                        _repoWrapper.UserPlastDegrees.Create(userPlastDegree);
                        isAdded = true;
                    }

                }
            }

            return isAdded;
        }
    }
}
