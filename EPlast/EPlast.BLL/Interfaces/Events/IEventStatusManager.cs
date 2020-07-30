using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Events
{
    /// <summary>
    ///  Implements  operations for work with event statuses.
    /// </summary>
    public interface IEventStatusManager
    {
        /// <summary>
        /// Get Id of event status by status name.
        /// </summary>
        /// <returns>The Id of specific event status.</returns>
        /// <param name="statusName">The name of event status</param>
        Task<int> GetStatusIdAsync(string statusName);
    }
}
