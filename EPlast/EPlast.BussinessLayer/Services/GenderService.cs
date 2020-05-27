﻿using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BussinessLayer.Services
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

        public IEnumerable<GenderDTO> GetAll()
        {
            var result = _repoWrapper.Gender.FindAll().ToList();
            return _mapper.Map<IEnumerable<Gender>, IEnumerable<GenderDTO>>(result);
        }
    }
}
