using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.DTO.Course;
using EPlast.BLL.Interfaces.Blank;
using EPlast.DataAccess.Entities.Course;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Blank
{
    public class UserCourseService : IUserCourseService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public UserCourseService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseDto>> GetCourseByUserIdAsync(string userid)
        {
            //var result = (await _repositoryWrapper.UserCourse
            //    .GetAllAsync(
            //        predicate: uc => uc.UserId == userid && !uc.StatusPassedCourse ,
            //        include: a => a.Include(uc => uc.Сourse)))
            //    .Select(uc => new CourseDTO
            //    { 
            //        ID = uc.Сourse.ID,
            //        Link = uc.Сourse.Link,
            //        Name = uc.Сourse.Name                    
            //    });

            //return result;        

            var allcourse = (await _repositoryWrapper.Course
                .GetAllAsync(include: a=> a.Include(a =>a.AchievementDocuments))).Select(c => new CourseDto
                {
                    ID = c.ID,
                    Link = c.Link,
                    Name = c.Name,
                    IsFinishedByUser = c.AchievementDocuments.Any(uc => uc.UserId == userid )
                });

            return allcourse;

        }

        public  async Task ChangeCourseStatus(string userid , int? courseId)
        {
         
        }
    }
}
