using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.UserProfiles
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
            var result = await _repoWrapper.Work.FindAll().
                GroupBy(x => x.PlaceOfwork).
                Select(x => x.FirstOrDefault()).
                ToListAsync();
            return _mapper.Map<IEnumerable<Work>, IEnumerable<WorkDTO>>(result);
        }

        public async Task<IEnumerable<WorkDTO>> GetAllGroupByPositionAsync()
        {
            var result = await _repoWrapper.Work.FindAll().
                GroupBy(x => x.Position).
                Select(x => x.FirstOrDefault()).
                ToListAsync();
            return _mapper.Map<IEnumerable<Work>, IEnumerable<WorkDTO>>(result);
        }
        public async Task<WorkDTO> GetByIdAsync(int? workId)
        {
            return _mapper.Map<Work, WorkDTO>(
                await _repoWrapper.Work.FindByCondition(x => x.ID == workId).
                    FirstOrDefaultAsync()
                );
        }
    }
}
