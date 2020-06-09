using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.UserProfiles
{
    public class GenderService : IGenderService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GenderService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GenderDTO>> GetAllAsync()
        {
            var result = await _repoWrapper.Gender.FindAll().
                ToListAsync();
            return _mapper.Map<IEnumerable<Gender>, IEnumerable<GenderDTO>>(result);
        }
    }
}
