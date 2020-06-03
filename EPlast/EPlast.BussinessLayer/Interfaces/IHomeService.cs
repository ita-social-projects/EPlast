using EPlast.BussinessLayer.DTO;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces
{
    public interface IHomeService
    {
        Task SendEmailAdmin(ContactDTO contactDTO);
    }
}
