﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EPlast.BLL.Services.Distinctions
{
    public class UserDistinctionService : IUserDistinctionService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper; 
        private readonly UserManager<User> _userManager;

        public UserDistinctionService(IMapper mapper, IRepositoryWrapper repoWrapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _userManager = userManager;
        }
        public async Task AddUserDistinctionAsync(UserDistinctionDto userDistinctionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var userDistinction = new UserDistinction()
            {
                UserId = userDistinctionDTO.UserId,
                DistinctionId = userDistinctionDTO.DistinctionId,
                Date = userDistinctionDTO.Date,
                Reason = userDistinctionDTO.Reason,
                Reporter = userDistinctionDTO.Reporter,
                Number = userDistinctionDTO.Number,
            };
            await _repoWrapper.UserDistinction.CreateAsync(userDistinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task ChangeUserDistinctionAsync(UserDistinctionDto userDistinctionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var userDistinction = new UserDistinction()
            {
                Id = userDistinctionDTO.Id,
                UserId = userDistinctionDTO.UserId,
                DistinctionId = userDistinctionDTO.DistinctionId,
                Date = userDistinctionDTO.Date,
                Reason = userDistinctionDTO.Reason,
                Reporter = userDistinctionDTO.Reporter,
                Number = userDistinctionDTO.Number
            };
            _repoWrapper.UserDistinction.Update(userDistinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task DeleteUserDistinctionAsync(int id, User user)
        {
            await CheckIfAdminAsync(user);
            var userDistinction = await _repoWrapper.UserDistinction.GetFirstOrDefaultAsync(d => d.Id == id);
            if (userDistinction == null)
                throw new NotImplementedException();
            _repoWrapper.UserDistinction.Delete(userDistinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<UserDistinctionDto>> GetAllUsersDistinctionAsync()
        {
            var userDistinctions = await _repoWrapper.UserDistinction.GetAllAsync(include:
                source => source
                .Include(c => c.User)
                .Include(d => d.Distinction)
                );
            return _mapper.Map<IEnumerable<UserDistinction>, IEnumerable<UserDistinctionDto>>(userDistinctions);
        }

        public async Task<UserDistinctionDto> GetUserDistinctionAsync(int id)
        {
            var userDistinction = await _repoWrapper.UserDistinction.GetFirstOrDefaultAsync(d => d.Id == id, include:
                source => source
                .Include(c => c.User)
                .Include(d => d.Distinction));
            return _mapper.Map<UserDistinction, UserDistinctionDto>(userDistinction);
        }

        public async Task<IEnumerable<UserDistinctionDto>> GetUserDistinctionsOfUserAsync(string UserId)
        {
            var userDistinctions = await _repoWrapper.UserDistinction.GetAllAsync(u => u.UserId == UserId,
                include: source => source
                .Include(c => c.User)
                .Include(d => d.Distinction));
            return _mapper.Map<IEnumerable<UserDistinction>, IEnumerable<UserDistinctionDto>>(userDistinctions);
        }
        public async Task<bool> IsNumberExistAsync(int number) 
        {
            var distNum = await _repoWrapper.UserDistinction.GetFirstOrDefaultAsync(x => x.Number == number);
            return distNum != null;
        }

        public async Task CheckIfAdminAsync(User user)
        {
            if (!(await _userManager.GetRolesAsync(user)).Contains(Roles.Admin) &&
                !(await _userManager.GetRolesAsync(user)).Contains(Roles.GoverningBodyAdmin))
                throw new UnauthorizedAccessException();
        }

    }
}
