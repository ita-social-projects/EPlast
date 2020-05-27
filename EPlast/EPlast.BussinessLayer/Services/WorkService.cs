﻿using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BussinessLayer.Services
{
    public class WorkService : IWorkService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public WorkService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public IEnumerable<WorkDTO> GetAllGroupByPlace()
        {
            var result = _repoWrapper.Work.FindAll().GroupBy(x => x.PlaceOfwork).Select(x => x.FirstOrDefault()).ToList();
            return _mapper.Map<IEnumerable<Work>, IEnumerable<WorkDTO>>(result);
        }

        public IEnumerable<WorkDTO> GetAllGroupByPosition()
        {
            var result = _repoWrapper.Work.FindAll().GroupBy(x => x.Position).Select(x => x.FirstOrDefault()).ToList();
            return _mapper.Map<IEnumerable<Work>, IEnumerable<WorkDTO>>(result);
        }
    }
}
