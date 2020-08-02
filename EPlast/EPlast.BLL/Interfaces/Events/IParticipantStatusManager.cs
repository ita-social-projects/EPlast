using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Events
{
    /// <summary>
    ///  Implements  operations for work with participant statuses.
    /// </summary>
    public interface IParticipantStatusManager
    {
        /// <summary>
        /// Get Id of participant status by status name.
        /// </summary>
        /// <returns>The Id of specific participant status.</returns>
        /// <param name="statusName">The name of participant status</param>
        Task<int> GetStatusIdAsync(string statusName);
    }
}
