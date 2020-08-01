using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.UserProfiles
{
    public class DegreeService : IDegreeService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public DegreeService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DegreeDTO>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<Degree>, IEnumerable<DegreeDTO>>(await _repoWrapper.Degree.GetAllAsync());
        }
    }
}
