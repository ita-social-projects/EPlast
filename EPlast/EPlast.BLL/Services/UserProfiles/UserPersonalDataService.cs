using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;

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
        public async Task<IEnumerable<DegreeDto>> GetAllDegreesAsync()
        {
            return _mapper.Map<IEnumerable<Degree>, IEnumerable<DegreeDto>>(await _repoWrapper.Degree.GetAllAsync());
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EducationDto>> GetAllEducationsGroupByPlaceAsync()
        {
            return _mapper.Map<IEnumerable<Education>, IEnumerable<EducationDto>>(
                (await _repoWrapper.Education.GetAllAsync()).
                    GroupBy(x => x.PlaceOfStudy).
                    Select(x => x.FirstOrDefault())
                );
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EducationDto>> GetAllEducationsGroupBySpecialityAsync()
        {
            return _mapper.Map<IEnumerable<Education>, IEnumerable<EducationDto>>(
                (await _repoWrapper.Education.GetAllAsync()).
                    GroupBy(x => x.Speciality).
                    Select(x => x.FirstOrDefault())
                );
        }

        /// <inheritdoc />
        public async Task<EducationDto> GetEducationsByIdAsync(int? educationId)
        {
            return _mapper.Map<Education, EducationDto>(
                await _repoWrapper.Education.GetFirstOrDefaultAsync(x => x.ID == educationId));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GenderDto>> GetAllGendersAsync()
        {
            return _mapper.Map<IEnumerable<Gender>, IEnumerable<GenderDto>>(await _repoWrapper.Gender.GetAllAsync());
        }

        public async Task<IEnumerable<UpuDegreeDto>> GetAllUpuDegreesAsync()
        {
            return _mapper.Map<IEnumerable<UpuDegree>, IEnumerable<UpuDegreeDto>>(await _repoWrapper.UpuDegree.GetAllAsync());
        }

        /// <inheritdoc />
        public async Task<IEnumerable<NationalityDto>> GetAllNationalityAsync()
        {
            return _mapper.Map<IEnumerable<Nationality>, IEnumerable<NationalityDto>>(await _repoWrapper.Nationality.GetAllAsync());
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ReligionDto>> GetAllReligionsAsync()
        {
            return _mapper.Map<IEnumerable<Religion>, IEnumerable<ReligionDto>>(await _repoWrapper.Religion.GetAllAsync());
        }

        /// <inheritdoc />
        public async Task<IEnumerable<WorkDto>> GetAllWorkGroupByPlaceAsync()
        {
            return _mapper.Map<IEnumerable<Work>, IEnumerable<WorkDto>>(
                (await _repoWrapper.Work.GetAllAsync()).
                    GroupBy(x => x.PlaceOfwork).
                    Select(x => x.FirstOrDefault())
                );
        }

        /// <inheritdoc />
        public async Task<IEnumerable<WorkDto>> GetAllWorkGroupByPositionAsync()
        {
            return _mapper.Map<IEnumerable<Work>, IEnumerable<WorkDto>>(
                (await _repoWrapper.Work.GetAllAsync()).
                    GroupBy(x => x.Position).
                    Select(x => x.FirstOrDefault())
                );
        }

        /// <inheritdoc />
        public async Task<WorkDto> GetWorkByIdAsync(int? workId)
        {
            return _mapper.Map<Work, WorkDto>(
                await _repoWrapper.Work.GetFirstOrDefaultAsync(x => x.ID == workId));
        }
    }
}
