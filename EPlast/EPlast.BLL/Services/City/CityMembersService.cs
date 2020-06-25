using AutoMapper;
using EPlast.BLL.DTO.City;
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

        public async Task<IEnumerable<CityMembersDTO>> GetCurrentByCityIdAsync(int cityId)
        {
            var cityMembers = await _repositoryWrapper.CityMembers.GetAllAsync(
                    predicate: c => c.CityId == cityId && c.EndDate == null,
                    include: source => source
                        .Include(c => c.User));
            return _mapper.Map<IEnumerable<CityMembers>, IEnumerable<CityMembersDTO>>(cityMembers);
        }
    }
}