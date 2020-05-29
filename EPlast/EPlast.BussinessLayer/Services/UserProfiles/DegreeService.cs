using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BussinessLayer.Services.UserProfiles
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

        public IEnumerable<DegreeDTO> GetAll()
        {
            var result = _repoWrapper.Degree.FindAll().ToList();
            return _mapper.Map<IEnumerable<Degree>, IEnumerable<DegreeDTO>>(result);
        }
    }
}
