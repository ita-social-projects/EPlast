using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;

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
        public WorkDTO GetById(int? workId)
        {
            return _mapper.Map<Work, WorkDTO>(_repoWrapper.Work.FindByCondition(x => x.ID == workId)?.FirstOrDefault());
        }
    }
}
