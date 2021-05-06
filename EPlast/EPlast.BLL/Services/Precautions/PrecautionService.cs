using AutoMapper;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services
{
    public class PrecautionService : IPrecautionService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;

        public PrecautionService(IMapper mapper, IRepositoryWrapper repoWrapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _userManager = userManager;
        }

        public async Task AddPrecautionAsync(PrecautionDTO precautionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var Precaution = _mapper.Map<PrecautionDTO, Precaution>(precautionDTO);
            await _repoWrapper.Precaution.CreateAsync(Precaution);
            await _repoWrapper.SaveAsync();
        }

        public async Task ChangePrecautionAsync(PrecautionDTO precautionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var Precaution = await _repoWrapper.Precaution.GetFirstAsync(x => x.Id == precautionDTO.Id);
            Precaution.Name = precautionDTO.Name;
            _repoWrapper.Precaution.Update(Precaution);
            await _repoWrapper.SaveAsync();
        }

        public async Task DeletePrecautionAsync(int id, User user)
        {
            await CheckIfAdminAsync(user);
            var Precaution = (await _repoWrapper.Precaution.GetFirstAsync(d => d.Id == id));
            if (Precaution == null)
                throw new ArgumentNullException($"Precaution with {id} not found");
            _repoWrapper.Precaution.Delete(Precaution);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<PrecautionDTO>> GetAllPrecautionAsync()
        {
            return _mapper.Map<IEnumerable<Precaution>, IEnumerable<PrecautionDTO>>(await _repoWrapper.Precaution.GetAllAsync());
        }

        public async Task<PrecautionDTO> GetPrecautionAsync(int id)
        {
            var Precaution = _mapper.Map<PrecautionDTO>(await _repoWrapper.Precaution.GetFirstAsync(d => d.Id == id));
            return Precaution;
        }

        public async Task CheckIfAdminAsync(User user)
        {
            if (!(await _userManager.GetRolesAsync(user)).Contains(Roles.Admin))
                throw new UnauthorizedAccessException();
        }

        public IEnumerable<UserPrecautionsTableObject> GetUsersPrecautionsForTable(string searchedData, int page, int pageSize)
        {
            return _repoWrapper.UserPrecaution.GetUsersPrecautions(searchedData, page, pageSize);
        }
    }
}
