using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.City
{
    public class CityMembersService : ICityMembersService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public CityMembersService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityMembersDTO>> GetMembersByCityIdAsync(int cityId)
        {
            var cityMembers = await _repositoryWrapper.CityMembers.GetAllAsync(
                    predicate: c => c.CityId == cityId && c.EndDate == null,
                    include: source => source
                        .Include(c => c.User));
            return _mapper.Map<IEnumerable<CityMembers>, IEnumerable<CityMembersDTO>>(cityMembers);
        }

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
                UserId = userId
            };

            await _repositoryWrapper.CityMembers.CreateAsync(cityMember);
            await _repositoryWrapper.SaveAsync();

            return _mapper.Map<CityMembers, CityMembersDTO>(cityMember);
        }

        public async Task<CityMembersDTO> ToggleApproveStatusAsync(string userId)
        {
            var cityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.UserId == userId);

            cityMember.IsApproved = !cityMember.IsApproved;

            _repositoryWrapper.CityMembers.Update(cityMember);
            await _repositoryWrapper.SaveAsync();

            return _mapper.Map<CityMembers, CityMembersDTO>(cityMember);
        }

        public async Task RemoveMemberAsync(string userId)
        {
            var cityMember = await _repositoryWrapper.CityMembers
                .GetFirstOrDefaultAsync(u => u.UserId == userId);

            _repositoryWrapper.CityMembers.Delete(cityMember);
            await _repositoryWrapper.SaveAsync();
        }
    }
}