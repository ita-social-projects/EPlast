using AutoMapper;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public class DistinctionService : IDistinctionService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ILogger _logger;

        public DistinctionService(IMapper mapper, IRepositoryWrapper repoWrapper, ILogger logger)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _logger = logger;
        }
        public DistinctionDTO AddDistinction()
        {
            DistinctionDTO distinction = null;
            try
            {
                distinction = new DistinctionDTO();
            }
            catch(Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }
            return distinction;
        }

        public async Task<bool> ChangeDistinction(DistinctionDTO distinctionDTO)
        {
            Distinction distinction = null;
            try
            {
                distinction = await _repoWrapper.Distinction.GetFirstAsync(x => x.Id == distinctionDTO.Id);
                distinction.Name = distinctionDTO.Name;
                _repoWrapper.Distinction.Update(distinction);
                await _repoWrapper.SaveAsync();
            }
            catch(Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }
            return distinction != null;
        }

        public async Task<bool> DeleteDistinction(int id)
        {
            var success = false;
            try
            {
                var distinction = (await _repoWrapper.Distinction.GetFirstAsync(d => d.Id == id));
                if (distinction == null)
                    throw new ArgumentNullException($"Distinction with {id}");
                success = true;
                _repoWrapper.Distinction.Delete(distinction);
                await _repoWrapper.SaveAsync();
            }
            catch(Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }
            return success;
        }

        public async Task<IEnumerable<DistinctionDTO>> GetAllDistinctionAsync()
        {
            return _mapper.Map<IEnumerable<Distinction>, IEnumerable<DistinctionDTO>>(await _repoWrapper.Distinction.GetAllAsync());
        }

        public async Task<DistinctionDTO> GetDistinctionAsync(int id)
        {
            DistinctionDTO distinction = null;
            try
            {
                distinction = _mapper.Map<DistinctionDTO>(await _repoWrapper.Distinction.GetFirstAsync(d => d.Id == id));
            }
            catch(Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }
            return distinction;
        }
    }
}
