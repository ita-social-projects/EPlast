using EPlast.BLL.DTO;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces
{
    public interface IHomeService
    {
        Task SendEmailAdmin(ContactDTO contactDTO);
    }
}
