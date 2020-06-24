using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Interfaces.Admin;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;

namespace EPlast.BussinessLayer.Services.Admin
{
    public class AdminTypeService : IAdminTypeService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public AdminTypeService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        public async Task<AdminTypeDTO> GetAdminTypeByNameAsync(string name)
        {
            return _mapper.Map<AdminType, AdminTypeDTO>(await _repoWrapper.AdminType
                .GetFirstOrDefaultAsync(i => i.AdminTypeName == name));
        }

        public async Task<AdminTypeDTO> CreateAsync(AdminTypeDTO adminTypeDto)
        {
            var newAdminType = _mapper.Map<AdminTypeDTO, AdminType>(adminTypeDto);
            await _repoWrapper.AdminType.CreateAsync(newAdminType);
            await _repoWrapper.SaveAsync();

            return _mapper.Map<AdminType, AdminTypeDTO>(newAdminType);
        }
    }
}