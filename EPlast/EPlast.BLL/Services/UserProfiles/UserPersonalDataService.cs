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
    public class UserPersonalDataService : IUserPersonalDataService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public UserPersonalDataService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DegreeDTO>> GetAllDegreesAsync()
        {
            return _mapper.Map<IEnumerable<Degree>, IEnumerable<DegreeDTO>>(await _repoWrapper.Degree.GetAllAsync());
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EducationDTO>> GetAllEducationsGroupByPlaceAsync()
        {
            return _mapper.Map<IEnumerable<Education>, IEnumerable<EducationDTO>>(
                (await _repoWrapper.Education.GetAllAsync()).
                    GroupBy(x => x.PlaceOfStudy).
                    Select(x => x.FirstOrDefault())
                );
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EducationDTO>> GetAllEducationsGroupBySpecialityAsync()
        {
            return _mapper.Map<IEnumerable<Education>, IEnumerable<EducationDTO>>(
                (await _repoWrapper.Education.GetAllAsync()).
                    GroupBy(x => x.Speciality).
                    Select(x => x.FirstOrDefault())
                );
        }

        /// <inheritdoc />
        public async Task<EducationDTO> GetEducationsByIdAsync(int? educationId)
        {
            return _mapper.Map<Education, EducationDTO>(
                await _repoWrapper.Education.GetFirstOrDefaultAsync(x => x.ID == educationId));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GenderDTO>> GetAllGendersAsync()
        {
            return _mapper.Map<IEnumerable<Gender>, IEnumerable<GenderDTO>>(await _repoWrapper.Gender.GetAllAsync());
        }

        /// <inheritdoc />
        public async Task<IEnumerable<NationalityDTO>> GetAllNationalityAsync()
        {
            return _mapper.Map<IEnumerable<Nationality>, IEnumerable<NationalityDTO>>(await _repoWrapper.Nationality.GetAllAsync());
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ReligionDTO>> GetAllReligionsAsync()
        {
            return _mapper.Map<IEnumerable<Religion>, IEnumerable<ReligionDTO>>(await _repoWrapper.Religion.GetAllAsync());
        }

        /// <inheritdoc />
        public async Task<IEnumerable<WorkDTO>> GetAllWorkGroupByPlaceAsync()
        {
            return _mapper.Map<IEnumerable<Work>, IEnumerable<WorkDTO>>(
                (await _repoWrapper.Work.GetAllAsync()).
                    GroupBy(x => x.PlaceOfwork).
                    Select(x => x.FirstOrDefault())
                );
        }

        /// <inheritdoc />
        public async Task<IEnumerable<WorkDTO>> GetAllWorkGroupByPositionAsync()
        {
            return _mapper.Map<IEnumerable<Work>, IEnumerable<WorkDTO>>(
                (await _repoWrapper.Work.GetAllAsync()).
                    GroupBy(x => x.Position).
                    Select(x => x.FirstOrDefault())
                );
        }

        /// <inheritdoc />
        public async Task<WorkDTO> GetWorkByIdAsync(int? workId)
        {
            return _mapper.Map<Work, WorkDTO>(
                await _repoWrapper.Work.GetFirstOrDefaultAsync(x => x.ID == workId));
        }
    }
}
