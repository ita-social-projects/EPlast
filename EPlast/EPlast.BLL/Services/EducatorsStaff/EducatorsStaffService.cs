using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public class EducatorsStaffService:IEducatorsStaffService
    {
        
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
       

        public EducatorsStaffService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            
        }

        public async Task<EducatorsStaffDTO> CreateKadra(EducatorsStaffDTO kadrasDTO)
        {
            var newKV = _mapper.Map<EducatorsStaffDTO, EducatorsStaff>(kadrasDTO);
            await _repositoryWrapper.KVs.CreateAsync(newKV);
            await _repositoryWrapper.SaveAsync();
            return _mapper.Map<EducatorsStaff,EducatorsStaffDTO>(newKV); 
        }

        public async Task DeleteKadra(int kadra_id)
        {

                var deletedKadra = (await _repositoryWrapper.KVs.GetFirstAsync(d => d.ID == kadra_id));
           
                _repositoryWrapper.KVs.Delete(deletedKadra);
                await _repositoryWrapper.SaveAsync();
            
            
                   
        }

        public async Task<IEnumerable<EducatorsStaffDTO>> GetAllKVsAsync()
        {
            return _mapper.Map<IEnumerable<EducatorsStaff>, IEnumerable<EducatorsStaffDTO>>(
                 await _repositoryWrapper.KVs.GetAllAsync());
          
        }

        public async Task<EducatorsStaffDTO> GetKadraById(int KadraID)
        {
            var KV = _mapper.Map<EducatorsStaff, EducatorsStaffDTO>(await _repositoryWrapper.KVs.GetFirstAsync(c => c.ID == KadraID, 
                include:
                source=>source.Include(c=>c.User)));
            return KV;
        }

        public async Task<EducatorsStaffDTO> GetKadraByRegisterNumber(int KadrasRegisterNumber)
        {
            var KV = _mapper.Map<EducatorsStaff, EducatorsStaffDTO>(await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(c => c.NumberInRegister == KadrasRegisterNumber));
            return KV;
        }

        public async Task<IEnumerable<EducatorsStaffDTO>> GetKVsOfGivenUser(string UserId)
        {
            var kadra = await _repositoryWrapper.KVs.GetAllAsync(c => c.UserId == UserId);
            var Kadras = _mapper.Map<IEnumerable<EducatorsStaff>,IEnumerable<EducatorsStaffDTO>>(kadra);
            return Kadras;
        }

        public async Task<IEnumerable<EducatorsStaffDTO>> GetKVsWithKVType(int kvTypeId)
        {
           
            var KVs = _mapper.Map<IEnumerable<EducatorsStaff>, IEnumerable<EducatorsStaffDTO>>(await _repositoryWrapper.KVs.GetAllAsync(c => c.KadraVykhovnykivType.ID == kvTypeId,
                include:
                source=>source.Include(c=>c.User)));
            return KVs;
        }

        public async Task<bool> DoesUserHaveSuchStaff(string UserId ,int kadraId)
        {
            var edustaff = (await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(x => x.KadraVykhovnykivTypeId == kadraId && x.UserId == UserId));
            return edustaff != null;
            
        }

        public async Task UpdateKadra(EducatorsStaffDTO kadrasDTO)
        {
           var  editedKadra = await _repositoryWrapper.KVs.GetFirstAsync(x => x.ID == kadrasDTO.ID);
           
                editedKadra.NumberInRegister = kadrasDTO.NumberInRegister;
                editedKadra.Link = kadrasDTO.Link;
                editedKadra.BasisOfGranting = kadrasDTO.BasisOfGranting;
                editedKadra.DateOfGranting = kadrasDTO.DateOfGranting;

                _repositoryWrapper.KVs.Update(editedKadra);
                await _repositoryWrapper.SaveAsync();
            
        }

        public async Task<bool> StaffWithRegisternumberExists(int registerNumber)
        {
            var staffwithnum = (await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(x => x.NumberInRegister == registerNumber));
            return staffwithnum != null;
        }




        public async Task<bool> StaffWithRegisternumberExistsEdit(int kadraId, int numberInRegister)
        {
            var staffwithnum = (await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(x => x.NumberInRegister == numberInRegister && x.ID != kadraId));
            return staffwithnum != null;
        }


        public async Task<bool> UserHasSuchStaffEdit(string UserId, int kadraId)
        {
            var edustaff = (await _repositoryWrapper.KVs.GetAllAsync(x => x.KadraVykhovnykivTypeId == kadraId && x.UserId == UserId));
            
                return edustaff.Any();
        }

        public async Task<string> GetUserByEduStaff(int EduStaffId)
        {
            var eduStaff= (await _repositoryWrapper.KVs.GetFirstAsync(x => x.ID == EduStaffId));
            return eduStaff.UserId;
        }
    }
}
