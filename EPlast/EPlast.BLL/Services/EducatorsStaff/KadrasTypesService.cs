using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.EducatorsStaff
{
    public class KadrasTypesService:IKadrasTypeService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
       

        public KadrasTypesService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            
        }

        public async Task<KadraVykhovnykivTypesDTO> CreateKVType(KadraVykhovnykivTypesDTO kvTypeDTO)
        {
            var newKVType = _mapper.Map<KadraVykhovnykivTypesDTO,KadraVykhovnykivTypes>(kvTypeDTO);
            await _repositoryWrapper.KVTypes.CreateAsync(newKVType);
            await _repositoryWrapper.SaveAsync();
            return _mapper.Map<KadraVykhovnykivTypes, KadraVykhovnykivTypesDTO>(newKVType);
        }

        public async Task<IEnumerable<KadraVykhovnykivTypesDTO>> GetAllKVTypesAsync()
        {
            return _mapper.Map<IEnumerable<KadraVykhovnykivTypes>, IEnumerable<KadraVykhovnykivTypesDTO>>(
               await _repositoryWrapper.KVTypes.GetAllAsync());
        }


       

        public async Task<IEnumerable<KadraVykhovnykivDTO>> GetKadrasWithSuchType(KadraVykhovnykivTypesDTO kvTypeDTO)
        {
            
            var KVs = _mapper.Map<IEnumerable<KadraVykhovnykiv>, IEnumerable<KadraVykhovnykivDTO>>(await _repositoryWrapper.KVs.GetAllAsync(c => c.KadraVykhovnykivType.ID == kvTypeDTO.ID));
            return KVs;
        }

        public async Task<KadraVykhovnykivTypesDTO> GetKVsTypeByIdAsync(int KV_id)
        {
            var KV = _mapper.Map<KadraVykhovnykiv, KadraVykhovnykivDTO>(await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(c => c.ID == KV_id));
            
                var Type = await _repositoryWrapper.KVTypes.GetFirstOrDefaultAsync(a => a.ID == KV_id);
                return _mapper.Map<KadraVykhovnykivTypes,KadraVykhovnykivTypesDTO>(Type);
        }

        public async Task<int> GetIdWithKVType(string detectname )
        {
            var KV = _mapper.Map<KadraVykhovnykivTypes, KadraVykhovnykivTypesDTO>(await _repositoryWrapper.KVTypes.GetFirstOrDefaultAsync(c => c.Name == detectname));


            return KV.ID;
        }


    }
}
