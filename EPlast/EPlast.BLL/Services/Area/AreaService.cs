using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Repositories;
using EPlast.DataAccess.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IMapper _mapper;

        public AreaService(IRepositoryWrapper repoWrapper,
            IMapper mapper)
        {
            _areaRepository = repoWrapper.AreaRepository;
            _mapper = mapper;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<AreaDTO>> GetAllAsync()
        {
            var areas = await _areaRepository.GetAllAsync();
            var areasDTOs = _mapper.Map<IEnumerable<AreaDTO>>(areas);
            return areasDTOs;
        }

        ///<inheritdoc/>
        public async Task<AreaDTO> GetByIdAsync(int id)
        {
            var area = await _areaRepository.GetFirstOrDefaultAsync(
                predicate: a => a.Id == id
            );

            if (area == null)
            {
                throw new ArgumentNullException("Area is not exist");
            }

            var areaDTO = _mapper.Map<AreaDTO>(area);
            return areaDTO;
        }
    }
}
