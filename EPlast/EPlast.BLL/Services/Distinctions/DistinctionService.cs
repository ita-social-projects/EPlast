using AutoMapper;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using EPlast.Resources;

namespace EPlast.BLL
{
    public class DistinctionService : IDistinctionService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;


        public DistinctionService(IMapper mapper, IRepositoryWrapper repoWrapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _userManager = userManager;
        }
        public async Task AddDistinctionAsync(DistinctionDTO distinctionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var distinction = _mapper.Map<DistinctionDTO, Distinction>(distinctionDTO);
            await _repoWrapper.Distinction.CreateAsync(distinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task ChangeDistinctionAsync(DistinctionDTO distinctionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var distinction = await _repoWrapper.Distinction.GetFirstAsync(x => x.Id == distinctionDTO.Id);
            distinction.Name = distinctionDTO.Name;
            _repoWrapper.Distinction.Update(distinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task DeleteDistinctionAsync(int id, User user)
        {
            await CheckIfAdminAsync(user);
            var distinction = (await _repoWrapper.Distinction.GetFirstAsync(d => d.Id == id));
            if (distinction == null)
                throw new ArgumentNullException($"Distinction with {id} not found");
            _repoWrapper.Distinction.Delete(distinction);
            await _repoWrapper.SaveAsync();
        }
        public async Task<IEnumerable<DistinctionDTO>> GetAllDistinctionAsync()
        {
            return _mapper.Map<IEnumerable<Distinction>, IEnumerable<DistinctionDTO>>(await _repoWrapper.Distinction.GetAllAsync());
        }

        public async Task<DistinctionDTO> GetDistinctionAsync(int id)
        {
            var distinction = _mapper.Map<DistinctionDTO>(await _repoWrapper.Distinction.GetFirstAsync(d => d.Id == id));
            return distinction;
        }

        public async Task CheckIfAdminAsync(User user)
        {
            if(!(await _userManager.GetRolesAsync(user)).Contains(Roles.admin))
                throw new UnauthorizedAccessException();
        }
    }
}
