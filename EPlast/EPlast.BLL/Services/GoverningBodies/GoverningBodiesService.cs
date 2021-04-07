using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.GoverningBodies
{
    public class GoverningBodiesService: IGoverningBodiesService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public GoverningBodiesService(IRepositoryWrapper repoWrapper, IMapper mapper) 
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrganizationDTO>> GetOrganizationListAsync()
        {
            return _mapper.Map<IEnumerable<OrganizationDTO>>((await _repoWrapper.Organization.GetAllAsync()));
        }
    }
}
