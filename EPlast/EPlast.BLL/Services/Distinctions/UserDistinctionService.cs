using AutoMapper;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Distinctions
{
    public class UserDistinctionService : IUserDistinctionService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;

        public UserDistinctionService(IMapper mapper, IRepositoryWrapper repoWrapper)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }
        public async Task AddUserDistinctionAsync(UserDistinctionDTO userDistinctionDTO, ClaimsPrincipal user)
        {
            if (!user.IsInRole("Admin"))
                throw new UnauthorizedAccessException();
            var userDistinction = new UserDistinction()
            {
                UserId = userDistinctionDTO.UserId,
                DistinctionId = userDistinctionDTO.DistinctionId,
                Date = userDistinctionDTO.Date,
                Reason = userDistinctionDTO.Reason,
                Reporter = userDistinctionDTO.Reporter
            };
            await _repoWrapper.UserDistinction.CreateAsync(userDistinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task ChangeUserDistinctionAsync(UserDistinctionDTO userDistinctionDTO, ClaimsPrincipal user)
        {
            if (!user.IsInRole("Admin"))
                throw new UnauthorizedAccessException();
            var userDistinction = new UserDistinction()
            {
                Id = userDistinctionDTO.Id,
                UserId = userDistinctionDTO.UserId,
                DistinctionId = userDistinctionDTO.DistinctionId,
                Date = userDistinctionDTO.Date,
                Reason = userDistinctionDTO.Reason,
                Reporter = userDistinctionDTO.Reporter
            };
            _repoWrapper.UserDistinction.Update(userDistinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task DeleteUserDistinctionAsync(int id, ClaimsPrincipal user)
        {
            if (!user.IsInRole("Admin"))
                throw new UnauthorizedAccessException();
            var userDistinction = await _repoWrapper.UserDistinction.GetFirstOrDefaultAsync(d => d.Id == id);
            if (userDistinction == null)
                throw new NotImplementedException();
            _repoWrapper.UserDistinction.Delete(userDistinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<UserDistinctionDTO>> GetAllUsersDistinctionAsync()
        {
            var userDistinctions = await _repoWrapper.UserDistinction.GetAllAsync(include:
                source => source
                .Include(c => c.User)
                .Include(d => d.Distinction)
                );
            return _mapper.Map<IEnumerable<UserDistinction>, IEnumerable<UserDistinctionDTO>>(userDistinctions);
        }

        public async Task<UserDistinctionDTO> GetUserDistinctionAsync(int id)
        {
            var userDistinction = await _repoWrapper.UserDistinction.GetFirstOrDefaultAsync(d => d.Id == id, include:
                source => source
                .Include(c => c.User)
                .Include(d => d.Distinction));
            return _mapper.Map<UserDistinction, UserDistinctionDTO>(userDistinction);
        }

        public async Task<IEnumerable<UserDistinctionDTO>> GetUserDistinctionsOfUserAsync(string UserId)
        {
            var userDistinctions = await _repoWrapper.UserDistinction.GetAllAsync(u => u.UserId == UserId, 
                include: source => source
                .Include(c => c.User)
                .Include(d => d.Distinction));
            return _mapper.Map<IEnumerable<UserDistinction>, IEnumerable<UserDistinctionDTO>>(userDistinctions);
        }
    }
}
