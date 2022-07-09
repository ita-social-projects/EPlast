using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Services.ActiveMembership
{
    /// <inheritdoc />
    public class PlastDegreeService : IPlastDegreeService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IUserManagerService _userManagerService;
        public PlastDegreeService(IMapper mapper, IRepositoryWrapper repoWrapper, IUserManagerService userManagerService)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _userManagerService = userManagerService;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PlastDegreeDto>> GetDergeesAsync()
        {
            var degrees = await _repoWrapper.PlastDegrees.GetAllAsync();

            return _mapper.Map<IEnumerable<PlastDegreeDto>>(degrees);
        }

        /// <inheritdoc />
        public async Task<bool> CheckDegreeAsync(int degreeId, List<string> appropriateDegrees)
        {
            var degree = await _repoWrapper.PlastDegrees
                .GetFirstAsync(predicate: p => p.Id == degreeId);

            return appropriateDegrees.Contains(degree.Name);
        }

        /// <inheritdoc />
        public async Task<DateTime> GetDateOfEntryAsync(string userId)
        {
            var userDTO = await _userManagerService.FindByIdAsync(userId);

            return userDTO.RegistredOn;
        }

        /// <inheritdoc />
        public async Task<UserPlastDegreeDto> GetUserPlastDegreeAsync(string userId)
        {
            var userPlastDegree = await _repoWrapper.UserPlastDegree.GetFirstOrDefaultAsync(upd => upd.UserId == userId, include: pd => pd.Include(d => d.PlastDegree));

            return _mapper.Map<UserPlastDegreeDto>(userPlastDegree);
        }
        /// <inheritdoc />
        public async Task<bool> AddPlastDegreeForUserAsync(UserPlastDegreePostDto userPlastDegreePostDTO)
        {
            bool isAdded = false;
            var userDto = await _userManagerService.FindByIdAsync(userPlastDegreePostDTO.UserId);
            var previousDegreeUserPlastDegree = await _repoWrapper.UserPlastDegree
                .GetFirstOrDefaultAsync(i => i.UserId == userPlastDegreePostDTO.UserId);

            if (previousDegreeUserPlastDegree != null)
            {
                UserPlastDegree userPlastDegree = _mapper.Map<UserPlastDegree>(userDto.UserPlastDegrees);
                if (userPlastDegree != null && userPlastDegree.PlastDegreeId == userPlastDegreePostDTO.PlastDegreeId)
                {
                    return isAdded;
                }
                previousDegreeUserPlastDegree.UserId = userPlastDegreePostDTO.UserId;
                previousDegreeUserPlastDegree.PlastDegreeId = userPlastDegreePostDTO.PlastDegreeId;
                previousDegreeUserPlastDegree.DateStart = userPlastDegreePostDTO.DateStart;
                    
                _repoWrapper.UserPlastDegree.Update(previousDegreeUserPlastDegree);
                await _repoWrapper.SaveAsync();

                return isAdded;
            }

            if (userDto != null)
            {
                UserPlastDegree userPlastDegree = _mapper.Map<UserPlastDegree>(userPlastDegreePostDTO);
                PlastDegree plastDegree =
                    await _repoWrapper.PlastDegrees.GetFirstOrDefaultAsync(pd =>
                        pd.Id == userPlastDegreePostDTO.PlastDegreeId);
                if (plastDegree != null)
                {
                    userPlastDegree.PlastDegree = plastDegree;
                    _repoWrapper.UserPlastDegree.Attach(userPlastDegree);
                    _repoWrapper.UserPlastDegree.Create(userPlastDegree);
                    await _repoWrapper.SaveAsync();
                    isAdded = true;
                }
            }
            return isAdded;
        }

        /// <inheritdoc />
        public async Task<bool> DeletePlastDegreeForUserAsync(string userId, int plastDegreeId)
        {
            bool isDeleted = false;
            UserPlastDegree userPlastDegree = await _repoWrapper.UserPlastDegree
                .GetFirstOrDefaultAsync(upd => upd.PlastDegreeId == plastDegreeId && upd.UserId == userId);
            if (userPlastDegree != null)
            {
                _repoWrapper.UserPlastDegree.Delete(userPlastDegree);
                await _repoWrapper.SaveAsync();
                isDeleted = true;
            }

            return isDeleted;
        }
    }
}
