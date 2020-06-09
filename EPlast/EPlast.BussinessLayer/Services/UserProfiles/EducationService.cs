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
    public class EducationService : IEducationService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public EducationService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EducationDTO>> GetAllGroupByPlaceAsync()
        {
            var result = await _repoWrapper.Education.FindAll().
                GroupBy(x => x.PlaceOfStudy).
                Select(x => x.FirstOrDefault()).
                ToListAsync();
            return _mapper.Map<IEnumerable<Education>, IEnumerable<EducationDTO>>(result);
        }

        public async Task<IEnumerable<EducationDTO>> GetAllGroupBySpecialityAsync()
        {
            var result = await _repoWrapper.Education.FindAll().
                GroupBy(x => x.Speciality).
                Select(x => x.FirstOrDefault()).
                ToListAsync();
            return _mapper.Map<IEnumerable<Education>, IEnumerable<EducationDTO>>(result);
        }
        public async Task<EducationDTO> GetByIdAsync(int? educationId)
        {
            return _mapper.Map<Education, EducationDTO>(
                await _repoWrapper.Education.FindByCondition(x => x.ID == educationId).
                    FirstOrDefaultAsync()
                );
        }
    }
}
