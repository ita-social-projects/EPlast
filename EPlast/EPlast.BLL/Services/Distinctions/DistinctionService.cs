using AutoMapper;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public class DistinctionService : IDistinctionService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;


        public DistinctionService(IMapper mapper, IRepositoryWrapper repoWrapper)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }
        public async Task AddDistinction(DistinctionDTO distinctionDto, ClaimsPrincipal user)
        {
            if (!user.IsInRole("Admin"))
                throw new UnauthorizedAccessException();
            var distinction = _mapper.Map<DistinctionDTO, Distinction>(distinctionDto);
            await _repoWrapper.Distinction.CreateAsync(distinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task ChangeDistinction(DistinctionDTO distinctionDTO, ClaimsPrincipal user)
        {
            if (!user.IsInRole("Admin"))
                throw new UnauthorizedAccessException();
            var distinction = await _repoWrapper.Distinction.GetFirstAsync(x => x.Id == distinctionDTO.Id);
            distinction.Name = distinctionDTO.Name;
            _repoWrapper.Distinction.Update(distinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task DeleteDistinction(int id, ClaimsPrincipal user)
        {
            if (!user.IsInRole("Admin"))
                throw new UnauthorizedAccessException();
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
    }
}
