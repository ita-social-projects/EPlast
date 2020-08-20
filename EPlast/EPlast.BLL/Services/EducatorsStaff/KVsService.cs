using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.EducatorsStaff
{
    public class KVsService:IKVService
    {
        
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
       

        public KVsService(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService<KVsService> loggerService)
        {
            
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            
        }

        public async Task<KadrasDTO> CreateKadra(KadrasDTO kadrasDTO)
        {
            var newKV = _mapper.Map<KadrasDTO, KVs>(kadrasDTO);
            await _repositoryWrapper.KVs.CreateAsync(newKV);
            await _repositoryWrapper.SaveAsync();
            return _mapper.Map<KVs,KadrasDTO>(newKV); 
        }

        public async Task DeleteKadra(int kadra_id)
        {

                var deletedKadra = (await _repositoryWrapper.KVs.GetFirstAsync(d => d.ID == kadra_id));
            if (deletedKadra != null)
            {
                _repositoryWrapper.KVs.Delete(deletedKadra);
                await _repositoryWrapper.SaveAsync();
            }
            else {
                throw new InvalidOperationException();
            }
                   
        }

        public async Task<IEnumerable<KadrasDTO>> GetAllKVsAsync()
        {
            return _mapper.Map<IEnumerable<KVs>, IEnumerable<KadrasDTO>>(
                 await _repositoryWrapper.KVs.GetAllAsync());
          
        }

        public async Task<KadrasDTO> GetKadraById(int KadraID)
        {
            var KV = _mapper.Map<KVs, KadrasDTO>(await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(c => c.ID == KadraID));
            return KV;
        }

        public async Task<KadrasDTO> GetKadraByRegisterNumber(int KadrasRegisterNumber)
        {
            var KV = _mapper.Map<KVs, KadrasDTO>(await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(c => c.NumberInRegister == KadrasRegisterNumber));
            return KV;
        }

        public async Task<IEnumerable<KadrasDTO>> GetKVsOfGivenUser(string userId)
        {
            var Kadras = _mapper.Map<IEnumerable<KVs>,IEnumerable<KadrasDTO>>(await _repositoryWrapper.KVs.GetAllAsync(c => c.UserId == userId));
            return Kadras;
        }

        public async Task<IEnumerable<KadrasDTO>> GetKVsWithKVType(int kvType_id)
        {
            var kvType = await _repositoryWrapper.KVTypes.GetFirstAsync(x => x.ID == kvType_id);
            if (kvType == null)
            {
                throw new InvalidOperationException();
            }
            var KVs = _mapper.Map<IEnumerable<KVs>, IEnumerable<KadrasDTO>>(await _repositoryWrapper.KVs.GetAllAsync(c => c.KVTypesID == kvType_id));
            return KVs;
        }

        public async Task UpdateKadra(KadrasDTO kadrasDTO)
        {
           var  editedKadra = await _repositoryWrapper.KVs.GetFirstAsync(x => x.ID == kadrasDTO.ID);
           
                editedKadra.NumberInRegister = kadrasDTO.NumberInRegister;
                editedKadra.Link = kadrasDTO.Link;
                editedKadra.UserId = kadrasDTO.UserId;
                editedKadra.BasisOfGranting = kadrasDTO.BasisOfGranting;
                editedKadra.DateOfGranting = kadrasDTO.DateOfGranting;

                _repositoryWrapper.KVs.Update(editedKadra);
                await _repositoryWrapper.SaveAsync();
            
        }
    }
}
