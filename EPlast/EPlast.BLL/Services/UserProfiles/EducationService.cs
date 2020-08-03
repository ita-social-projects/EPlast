using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.UserProfiles
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

        /// <inheritdoc />
        public async Task<IEnumerable<EducationDTO>> GetAllGroupByPlaceAsync()
        {
            return _mapper.Map<IEnumerable<Education>, IEnumerable<EducationDTO>>(
                (await _repoWrapper.Education.GetAllAsync()).
                    GroupBy(x => x.PlaceOfStudy).
                    Select(x => x.FirstOrDefault())
                );
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EducationDTO>> GetAllGroupBySpecialityAsync()
        {
            return _mapper.Map<IEnumerable<Education>, IEnumerable<EducationDTO>>(
                (await _repoWrapper.Education.GetAllAsync()).
                    GroupBy(x => x.Speciality).
                    Select(x => x.FirstOrDefault())
                );
        }

        /// <inheritdoc />
        public async Task<EducationDTO> GetByIdAsync(int? educationId)
        {
            return _mapper.Map<Education, EducationDTO>(
                await _repoWrapper.Education.GetFirstOrDefaultAsync(x => x.ID == educationId));
        }
    }
}
