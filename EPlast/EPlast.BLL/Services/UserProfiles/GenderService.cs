using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.UserProfiles
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

        /// <inheritdoc />
        public async Task<IEnumerable<GenderDTO>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<Gender>, IEnumerable<GenderDTO>>(await _repoWrapper.Gender.GetAllAsync());
        }
    }
}
