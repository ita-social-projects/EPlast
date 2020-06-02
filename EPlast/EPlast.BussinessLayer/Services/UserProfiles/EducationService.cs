using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BussinessLayer.Services.UserProfiles
{
    public class EducationService : IEducationService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public EducationService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public IEnumerable<EducationDTO> GetAllGroupByPlace()
        {
            var result = _repoWrapper.Education.FindAll().GroupBy(x => x.PlaceOfStudy).Select(x => x.FirstOrDefault()).ToList();
            return _mapper.Map<IEnumerable<Education>, IEnumerable<EducationDTO>>(result);
        }

        public IEnumerable<EducationDTO> GetAllGroupBySpeciality()
        {
            var result = _repoWrapper.Education.FindAll().GroupBy(x => x.Speciality).Select(x => x.FirstOrDefault()).ToList();
            return _mapper.Map<IEnumerable<Education>, IEnumerable<EducationDTO>>(result);
        }
        public EducationDTO GetById(int? educationId)
        {
            return _mapper.Map<Education, EducationDTO>(_repoWrapper.Education.FindByCondition(x => x.ID == educationId).FirstOrDefault());
        }
    }
}
