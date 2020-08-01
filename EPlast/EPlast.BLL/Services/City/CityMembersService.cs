using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.City
{
    public class CityMembersService : ICityMembersService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CityMembersService(IRepositoryWrapper repositoryWrapper,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _userManager = userManager;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CityMembersDTO>> GetMembersByCityIdAsync(int cityId)
        {
            var cityMembers = await _repositoryWrapper.CityMembers.GetAllAsync(
                    predicate: c => c.CityId == cityId && c.EndDate == null,
                    include: source => source
                        .Include(c => c.User));

            return _mapper.Map<IEnumerable<CityMembers>, IEnumerable<CityMembersDTO>>(cityMembers);
        }

        /// <inheritdoc />
        public async Task<CityMembersDTO> AddFollowerAsync(int cityId, string userId)
        {
            var oldCityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(i => i.UserId == userId);

            if (oldCityMember != null)
            {
                _repositoryWrapper.CityMembers.Delete(oldCityMember);
                await _repositoryWrapper.SaveAsync();
            }

            var cityMember = new CityMembers()
            {
                CityId = cityId,
                IsApproved = false,
                UserId = userId,
                User = await _userManager.FindByIdAsync(userId)
            };

            await _repositoryWrapper.CityMembers.CreateAsync(cityMember);
            await _repositoryWrapper.SaveAsync();

            return _mapper.Map<CityMembers, CityMembersDTO>(cityMember);
        }

        /// <inheritdoc />
        public async Task<CityMembersDTO> AddFollowerAsync(int cityId, ClaimsPrincipal user)
        {
            var userId = _userManager.GetUserId(user);

            return await AddFollowerAsync(cityId, userId);
        }

        /// <inheritdoc />
        public async Task<CityMembersDTO> ToggleApproveStatusAsync(int memberId)
        {
            var cityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.ID == memberId, m => m.Include(u => u.User));

            cityMember.IsApproved = !cityMember.IsApproved;

            _repositoryWrapper.CityMembers.Update(cityMember);
            await _repositoryWrapper.SaveAsync();

            return _mapper.Map<CityMembers, CityMembersDTO>(cityMember);
        }

        /// <inheritdoc />
        public async Task RemoveFollowerAsync(int followerId)
        {
            var cityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.ID == followerId);

            _repositoryWrapper.CityMembers.Delete(cityMember);
            await _repositoryWrapper.SaveAsync();
        }
    }
}