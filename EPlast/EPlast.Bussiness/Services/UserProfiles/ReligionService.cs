using AutoMapper;
using EPlast.Bussiness.DTO.UserProfiles;
using EPlast.Bussiness.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Bussiness.Services.UserProfiles
{
    public class ReligionService : IReligionService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public ReligionService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReligionDTO>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<Religion>, IEnumerable<ReligionDTO>>(await _repoWrapper.Religion.GetAllAsync());
        }
    }
}
