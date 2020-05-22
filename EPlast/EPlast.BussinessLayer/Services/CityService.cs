using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Services
{
    public class CityService:ICityService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public CityService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public IEnumerable<City> GetAll()
        {
            return _repoWrapper.City.FindAll();
        }
        public IEnumerable<CityDTO> GetAllDTO()
        {
            return _mapper.Map<IEnumerable<City>, IEnumerable<CityDTO>>(GetAll());
        }
    }
}
