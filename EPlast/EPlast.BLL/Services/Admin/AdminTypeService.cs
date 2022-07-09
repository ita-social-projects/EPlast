using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.Interfaces.Admin;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;

namespace EPlast.BLL.Services.Admin
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

        public async Task<AdminTypeDto> GetAdminTypeByNameAsync(string name)
        {
            var adminType = await _repoWrapper.AdminType.GetFirstOrDefaultAsync(i => i.AdminTypeName == name);
            return adminType != null
                ? _mapper.Map<AdminType, AdminTypeDto>(adminType)
                : await this.CreateByNameAsync(name);
        }

        public async Task<AdminTypeDto> GetAdminTypeByIdAsync(int adminTypeId)
        {
            var adminType = await _repoWrapper.AdminType.GetFirstOrDefaultAsync(i => i.ID == adminTypeId);
            return _mapper.Map<AdminType, AdminTypeDto>(adminType);
        }

        public async Task<AdminTypeDto> CreateByNameAsync(string adminTypeName)
        {
            return await this.CreateAsync(new AdminTypeDto() { AdminTypeName = adminTypeName });
        }

        public async Task<AdminTypeDto> CreateAsync(AdminTypeDto adminTypeDto)
        {
            var newAdminType = _mapper.Map<AdminTypeDto, AdminType>(adminTypeDto);
            await _repoWrapper.AdminType.CreateAsync(newAdminType);
            await _repoWrapper.SaveAsync();

            return _mapper.Map<AdminType, AdminTypeDto>(newAdminType);
        }
    }
}
