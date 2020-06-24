using System.Threading.Tasks;
using EPlast.BussinessLayer.DTO;

namespace EPlast.BussinessLayer.Interfaces.Admin
{
    public interface IAdminTypeService
    {
        Task<AdminTypeDTO> GetAdminTypeByNameAsync(string name);
        Task<AdminTypeDTO> CreateByNameAsync(string adminTypeName);
        Task<AdminTypeDTO> CreateAsync(AdminTypeDTO adminTypeDto);
    }
}