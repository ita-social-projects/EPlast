using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;

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
        public async Task AddUserDistinction(UserDistinctionDTO userDistinctionDTO, ClaimsPrincipal user)
        {
            userDistinctionDTO.CanChange = user.IsInRole("Admin");
            var userDistinction = _mapper.Map<UserDistinctionDTO, UserDistinction>(userDistinctionDTO);
            await _repoWrapper.UserDistinction.CreateAsync(userDistinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task ChangeUserDistinction(UserDistinctionDTO userDistinctionDTO, ClaimsPrincipal user)
        {
            var userDistinction  = await _repoWrapper.UserDistinction.GetFirstAsync(x => x.Id == userDistinctionDTO.Id);
                _repoWrapper.UserDistinction.Update(userDistinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task DeleteUserDistinction(int id, ClaimsPrincipal user)
        {
            var userDistinction = await _repoWrapper.UserDistinction.GetFirstOrDefaultAsync(d => d.Id == id);
            if (userDistinction == null)
                throw new NotImplementedException();
            _repoWrapper.UserDistinction.Delete(userDistinction);
            await _repoWrapper.SaveAsync();

        }

        public async Task<IEnumerable<UserDistinctionDTO>> GetAllUsersDistinctionAsync()
        {
            var userDistinctions = await _repoWrapper.UserDistinction.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDistinction>, IEnumerable<UserDistinctionDTO>>(userDistinctions);
        }

        public async Task<UserDistinctionDTO> GetUserDistinction(int id)
        {
            var userDistinction = await _repoWrapper.UserDistinction.GetFirstOrDefaultAsync(d => d.Id == id);
            return _mapper.Map<UserDistinction, UserDistinctionDTO>(userDistinction);
        }

        public async Task<IEnumerable<UserDistinctionDTO>> GetUserDistinctionOfGivenUser(string UserId)
        {
            var userDistinctions = await _repoWrapper.UserDistinction.GetAllAsync(u => u.UserId == UserId);
            return _mapper.Map<IEnumerable<UserDistinction>, IEnumerable<UserDistinctionDTO>>(userDistinctions);
        }
    }
}
