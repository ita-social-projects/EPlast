using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.DTO.Course;
using EPlast.BLL.Interfaces.Blank;
using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Entities.Course;
using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Blank
{
    public class CourseService : ICourseService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        public CourseService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseDto>> GetAllAsync()
        {
            var result = (await _repositoryWrapper.Course.GetAllAsync()).Select(c => _mapper.Map<CourseDto>(c));
           

            return result;
        }

        public async Task<CourseDto> AddCourseAsync(CourseDto courseDto)
        {
            var course =  _mapper.Map<Course>(courseDto);
            await _repositoryWrapper.Course.CreateAsync(course);
            await _repositoryWrapper.SaveAsync();

            return courseDto;
        }
    }
}
