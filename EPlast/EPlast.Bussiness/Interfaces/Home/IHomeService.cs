using EPlast.BusinessLogicLayer.DTO;
using System.Threading.Tasks;

namespace EPlast.BusinessLogicLayer.Interfaces
{
    public interface IHomeService
    {
        Task SendEmailAdmin(ContactDTO contactDTO);
    }
}
