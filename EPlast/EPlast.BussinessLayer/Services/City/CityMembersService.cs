using AutoMapper;
using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.City
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

        public async Task<IEnumerable<CityMembersDTO>> GetCurrentByCityIdAsync(int cityId)
        {
            var cityMembers = await _repositoryWrapper.CityMembers
                .FindByCondition(cm => cm.CityId == cityId && cm.EndDate == null)
                .Include(cm => cm.User)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CityMembers>, IEnumerable<CityMembersDTO>>(cityMembers);
        }
    }
}