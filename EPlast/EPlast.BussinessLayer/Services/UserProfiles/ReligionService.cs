using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BussinessLayer.Services.UserProfiles
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

        public IEnumerable<ReligionDTO> GetAll()
        {
            var result = _repoWrapper.Religion.FindAll().ToList();
            return _mapper.Map<IEnumerable<Religion>, IEnumerable<ReligionDTO>>(result);
        }
    }
}
