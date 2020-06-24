﻿using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.UserProfiles;
using EPlast.BusinessLogicLayer.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BusinessLogicLayer.Services.UserProfiles
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

        public async Task<IEnumerable<WorkDTO>> GetAllGroupByPlaceAsync()
        {
            return _mapper.Map<IEnumerable<Work>, IEnumerable<WorkDTO>>(
                (await _repoWrapper.Work.GetAllAsync()).
                    GroupBy(x => x.PlaceOfwork).
                    Select(x => x.FirstOrDefault())
                );
        }

        public async Task<IEnumerable<WorkDTO>> GetAllGroupByPositionAsync()
        {
            return _mapper.Map<IEnumerable<Work>, IEnumerable<WorkDTO>>(
                (await _repoWrapper.Work.GetAllAsync()).
                    GroupBy(x => x.Position).
                    Select(x => x.FirstOrDefault())
                );
        }
        public async Task<WorkDTO> GetByIdAsync(int? workId)
        {
            return _mapper.Map<Work, WorkDTO>(
                await _repoWrapper.Work.GetFirstOrDefaultAsync(x => x.ID == workId));
        }
    }
}
