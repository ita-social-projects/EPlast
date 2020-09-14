using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public class EducatorsStaffTypesService:IEducatorsStaffTypesService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
       

        public EducatorsStaffTypesService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            
        }

        public async Task<EducatorsStaffTypesDTO> CreateKVType(EducatorsStaffTypesDTO kvTypeDTO)
        {
            var newKVType = _mapper.Map<EducatorsStaffTypesDTO,EducatorsStaffTypes>(kvTypeDTO);
            await _repositoryWrapper.KVTypes.CreateAsync(newKVType);
            await _repositoryWrapper.SaveAsync();
            return _mapper.Map<EducatorsStaffTypes, EducatorsStaffTypesDTO>(newKVType);
        }

        public async Task<IEnumerable<EducatorsStaffTypesDTO>> GetAllKVTypesAsync()
        {
            return _mapper.Map<IEnumerable<EducatorsStaffTypes>, IEnumerable<EducatorsStaffTypesDTO>>(
               await _repositoryWrapper.KVTypes.GetAllAsync());
        }

        public async Task<IEnumerable<EducatorsStaffDTO>> GetKadrasWithSuchType(EducatorsStaffTypesDTO kvTypeDTO)
        {
            
            var KVs = _mapper.Map<IEnumerable<EducatorsStaff>, IEnumerable<EducatorsStaffDTO>>(await _repositoryWrapper.KVs.GetAllAsync(c => c.KadraVykhovnykivTypeId == kvTypeDTO.ID));
            return KVs;
        }

        public async Task<EducatorsStaffTypesDTO> GetKVsTypeByIdAsync(int KV_id)
        {
            var KV = _mapper.Map<EducatorsStaff, EducatorsStaffDTO>(await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(c => c.ID == KV_id));
            
                var Type = await _repositoryWrapper.KVTypes.GetFirstOrDefaultAsync(a => a.ID == KV.KadraVykhovnykivTypeId);
                return _mapper.Map<EducatorsStaffTypes,EducatorsStaffTypesDTO>(Type);
        }

       
    }
}
