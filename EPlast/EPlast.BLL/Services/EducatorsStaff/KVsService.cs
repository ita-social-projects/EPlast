using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.EducatorsStaff
{
   public class KVsService:IKVService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IKVService _kvService;

        public KVsService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IKVService adminTypeService)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _kvService = adminTypeService;
        }

        public Task<KadrasDTO> CreateKadra(KadrasDTO kadrasDTO)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KadrasDTO>> GetAllKVsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<KadrasDTO> GetKadraById(int KadraID)
        {
            throw new NotImplementedException();
        }

        public Task<KadrasDTO> GetKadraByRegisterNumber(int KadrasRegisterNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KadrasDTO>> GetKVsOfGivenUser(UserDTO userDTO)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KadrasDTO>> GetKVsWithKVType(KVTypeDTO kvTypeDTO)
        {
            throw new NotImplementedException();
        }

        public Task<KadrasDTO> UpdateKadra(int KadraID, KadrasDTO kadrasDTO)
        {
            throw new NotImplementedException();
        }
    }
}
