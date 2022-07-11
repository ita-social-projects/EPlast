using System.Threading.Tasks;
using EPlast.BLL.DTO.Admin;

namespace EPlast.BLL.Interfaces.Admin
{
    public interface IAdminTypeService
    {
        Task<AdminTypeDto> GetAdminTypeByNameAsync(string name);

        Task<AdminTypeDto> GetAdminTypeByIdAsync(int adminTypeId);

        Task<AdminTypeDto> CreateByNameAsync(string adminTypeName);

        Task<AdminTypeDto> CreateAsync(AdminTypeDto adminTypeDto);
    }
}
