using EPlast.BLL.DTO.Account;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces
{
    public interface IHomeService
    {
        Task SendEmailAdmin(ContactsDto contactDTO);
    }
}
