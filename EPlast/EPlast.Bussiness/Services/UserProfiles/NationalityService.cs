using AutoMapper;
using EPlast.Bussiness.DTO.UserProfiles;
using EPlast.Bussiness.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Bussiness.Services.UserProfiles
{
    public class NationalityService : INationalityService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public NationalityService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NationalityDTO>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<Nationality>, IEnumerable<NationalityDTO>>(await _repoWrapper.Nationality.GetAllAsync());
        }
    }
}
