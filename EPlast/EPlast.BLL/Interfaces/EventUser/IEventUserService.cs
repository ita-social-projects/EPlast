using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL.DTO.EventUser;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.EventUser
{
    public interface IEventUserService
    {
        /// <summary>
        /// Get all created, planned, visited events for user by id
        /// </summary>
        /// <returns>Array of all created, planned, visited events for user</returns>
        /// /// <param name="userId"></param>
        /// /// <param name="user"></param>
        Task<EventUserDto> EventUserAsync(string userId, User user);
    }
}
