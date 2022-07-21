using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.Blank;
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

        public async Task<IEnumerable<CourseDTO>> GetCourseByIdAsync(string userid)
        {
            var result = (await _repositoryWrapper.UserCourse
                .GetAllAsync(
                    predicate: uc => uc.UserId == userid && !uc.StatusPassedCourse ,
                    include: a => a.Include(uc => uc.Сourse)))
                .Select(uc => new CourseDTO
                { 
                    ID = uc.Сourse.ID,
                    Link = uc.Сourse.Link,
                    Name = uc.Сourse.Name                    
                });

            return result;        
        }

        public  async Task ChangeCourseStatus(string userid , int courseId)
        {
            var result = await _repositoryWrapper.UserCourse.GetFirstOrDefaultAsync(predicate: uc => uc.UserId == userid && uc.CourseId == courseId, null);
            if (result != null )
            {
                if (  result.StatusPassedCourse == false)
                {
                    result.StatusPassedCourse = true;
                    _repositoryWrapper.UserCourse.Update(result);
                    await _repositoryWrapper.SaveAsync();
                }
                else
                {
                    result.StatusPassedCourse = false;
                    _repositoryWrapper.UserCourse.Update(result);
                    await _repositoryWrapper.SaveAsync();
                }
             
            }

        }
      
    }
}
