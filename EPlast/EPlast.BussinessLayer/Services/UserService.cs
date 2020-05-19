using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BussinessLayer.Services
{
    public class UserService:IUserService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(IRepositoryWrapper repoWrapper, UserManager<User> userManager, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _mapper = mapper;
        }

        public void Delete(string user)
        {
            throw new NotImplementedException();
        }

        public UserDTO GetUserProfile(string userId)
        {
            var user = _repoWrapper.User.
                FindByCondition(q => q.Id == userId).
                Include(i => i.UserProfile).
                ThenInclude(x => x.Nationality).
                Include(g => g.UserProfile).
                ThenInclude(g => g.Gender).
                Include(g => g.UserProfile).
                ThenInclude(g => g.Education).
                Include(g => g.UserProfile).
                ThenInclude(g => g.Degree).
                Include(g => g.UserProfile).
                ThenInclude(g => g.Religion).
                Include(g => g.UserProfile).
                ThenInclude(g => g.Work).
                FirstOrDefault();
            var model = _mapper.Map<User, UserDTO>(user);
            return model;
        }

        public async Task<TimeSpan> CheckOrAddPlastunRole(string userId,DateTime registeredOn)
        {
            try
            {
                var timeToJoinPlast = registeredOn.AddYears(1) - DateTime.Now;
                if (timeToJoinPlast <= TimeSpan.Zero)
                {
                    var us = await _userManager.FindByIdAsync(userId);
                    await _userManager.AddToRoleAsync(us, "Пластун");
                    return TimeSpan.Zero;
                }
                return timeToJoinPlast;
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }

        public IEnumerable<UserProfile> GetUserProfiles()
        {
            throw new NotImplementedException();
        }

        public void Update(UserProfile user)
        {
            throw new NotImplementedException();
        }
    }
}
