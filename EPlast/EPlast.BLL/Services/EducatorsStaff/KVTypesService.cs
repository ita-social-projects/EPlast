using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.EducatorsStaff
{
    public class KVTypesService:IKVsTypeService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
       

        public KVTypesService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IKVsTypeService adminTypeService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            
        }

        public async Task<KVTypeDTO> CreateKVType(KVTypeDTO kvTypeDTO)
        {
            var newKVType = _mapper.Map<KVTypeDTO,KVTypes>(kvTypeDTO);
            await _repositoryWrapper.KVTypes.CreateAsync(newKVType);
            await _repositoryWrapper.SaveAsync();
            return _mapper.Map<KVTypes, KVTypeDTO>(newKVType);
        }

        public async Task<IEnumerable<KVTypeDTO>> GetAllKVTypesAsync()
        {
            return _mapper.Map<IEnumerable<KVTypes>, IEnumerable<KVTypeDTO>>(
               await _repositoryWrapper.KVTypes.GetAllAsync());
        }

        public async Task<IEnumerable<KadrasDTO>> GetKadrasWithSuchType(KVTypeDTO kvTypeDTO)
        {
            
            var KVs = _mapper.Map<IEnumerable<KVs>, IEnumerable<KadrasDTO>>(await _repositoryWrapper.KVs.GetAllAsync(c => c.KVType.ID == kvTypeDTO.ID));
            return KVs;
        }

        public async Task<KVTypeDTO> GetKVsTypeByIdAsync(int KV_id)
        {
            var KV = _mapper.Map<KVs, KadrasDTO>(await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(c => c.ID == KV_id));
            
                var Type = await _repositoryWrapper.KVTypes.GetFirstOrDefaultAsync(a => a.ID == KV.KVType.ID);
                return _mapper.Map<KVTypes,KVTypeDTO>(Type);
        }

       
    }
}
