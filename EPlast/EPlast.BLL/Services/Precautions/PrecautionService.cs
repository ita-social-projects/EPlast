using AutoMapper;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace EPlast.BLL
{
    public class PrecautionService: IPrecautionService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;

        public PrecautionService(IMapper mapper, IRepositoryWrapper repoWrapper, UserManager<User> userManager) {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _userManager = userManager;
        }

        public async Task AddPrecautionAsync(PrecautionDTO PrecautionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var Precaution = _mapper.Map<PrecautionDTO, Precaution>(PrecautionDTO);
            await _repoWrapper.Precaution.CreateAsync(Precaution);
            await _repoWrapper.SaveAsync();
        }

        public async Task ChangePrecautionAsync(PrecautionDTO PrecautionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var Precaution = await _repoWrapper.Precaution.GetFirstAsync(x => x.Id == PrecautionDTO.Id);
            Precaution.Name = PrecautionDTO.Name;
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
            if (!(await _userManager.GetRolesAsync(user)).Contains("Admin"))
                throw new UnauthorizedAccessException();
        }
    }
}
