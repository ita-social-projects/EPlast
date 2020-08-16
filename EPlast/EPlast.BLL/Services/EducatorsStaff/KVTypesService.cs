using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.EducatorsStaff
{
    public class KVTypesService:IKVsTypeService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IKVsTypeService _kvTypeService;

        public KVTypesService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IKVsTypeService adminTypeService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _kvTypeService = adminTypeService;
        }

        public void CreateKVType(KVTypeDTO kvTypeDTO)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KVTypeDTO>> GetAllKVTypesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<KVTypeDTO> GetKVsTypeByIdAsync(int KV_id)
        {
            throw new NotImplementedException();
        }

        public Task<KVTypeDTO> GetTypeOfConcreteKVByIdAsync(int KV_id)
        {
            throw new NotImplementedException();
        }
    }
}
