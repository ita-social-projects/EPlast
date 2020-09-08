using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.EducatorsStaff
{
    public class KadraService:IKadraService
    {
        
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
       

        public KadraService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            
        }

        public async Task<KadraVykhovnykivDTO> CreateKadra(KadraVykhovnykivDTO kadrasDTO)
        {
            var newKV = _mapper.Map<KadraVykhovnykivDTO, KadraVykhovnykiv>(kadrasDTO);
            await _repositoryWrapper.KVs.CreateAsync(newKV);
            await _repositoryWrapper.SaveAsync();
            return _mapper.Map<KadraVykhovnykiv,KadraVykhovnykivDTO>(newKV); 
        }

        public async Task DeleteKadra(int kadra_id)
        {

                var deletedKadra = (await _repositoryWrapper.KVs.GetFirstAsync(d => d.ID == kadra_id));
           
                _repositoryWrapper.KVs.Delete(deletedKadra);
                await _repositoryWrapper.SaveAsync();
            
            
                   
        }

        public async Task<IEnumerable<KadraVykhovnykivDTO>> GetAllKVsAsync()
        {
            return _mapper.Map<IEnumerable<KadraVykhovnykiv>, IEnumerable<KadraVykhovnykivDTO>>(
                 await _repositoryWrapper.KVs.GetAllAsync());
          
        }

        public async Task<KadraVykhovnykivDTO> GetKadraById(int KadraID)
        {
            var KV = _mapper.Map<KadraVykhovnykiv, KadraVykhovnykivDTO>(await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(c => c.ID == KadraID));
            return KV;
        }

        public async Task<KadraVykhovnykivDTO> GetKadraByRegisterNumber(int KadrasRegisterNumber)
        {
            var KV = _mapper.Map<KadraVykhovnykiv, KadraVykhovnykivDTO>(await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(c => c.NumberInRegister == KadrasRegisterNumber));
            return KV;
        }

        public async Task<IEnumerable<KadraVykhovnykivDTO>> GetKVsOfGivenUser(string UserId)
        {
            var Kadras = _mapper.Map<IEnumerable<KadraVykhovnykiv>,IEnumerable<KadraVykhovnykivDTO>>(await _repositoryWrapper.KVs.GetAllAsync(c => c.UserId == UserId));
            return Kadras;
        }

        public async Task<IEnumerable<KadraVykhovnykivDTO>> GetKVsWithKVType(int kvType_Id)
        {
           
            var KVs = _mapper.Map<IEnumerable<KadraVykhovnykiv>, IEnumerable<KadraVykhovnykivDTO>>(await _repositoryWrapper.KVs.GetAllAsync(c => c.KadraVykhovnykivType.ID == kvType_Id,
                include:
                source=>source.Include(c=>c.User)));
            return KVs;
        }

        public async Task UpdateKadra(KadraVykhovnykivDTO kadrasDTO)
        {
           var  editedKadra = await _repositoryWrapper.KVs.GetFirstAsync(x => x.ID == kadrasDTO.ID);
           
                editedKadra.NumberInRegister = kadrasDTO.NumberInRegister;
                editedKadra.Link = kadrasDTO.Link;
            editedKadra.KadraVykhovnykivTypeId = kadrasDTO.KadraVykhovnykivTypeId;
                editedKadra.BasisOfGranting = kadrasDTO.BasisOfGranting;
                editedKadra.DateOfGranting = kadrasDTO.DateOfGranting;

                _repositoryWrapper.KVs.Update(editedKadra);
                await _repositoryWrapper.SaveAsync();
            
        }
    }
}
