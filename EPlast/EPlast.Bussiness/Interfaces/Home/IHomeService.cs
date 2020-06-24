using EPlast.Bussiness.DTO;
using System.Threading.Tasks;

namespace EPlast.Bussiness.Interfaces
{
    public interface IHomeService
    {
        Task SendEmailAdmin(ContactDTO contactDTO);
    }
}
