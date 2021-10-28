﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.Interfaces.Events
{
    /// <summary>
    ///  Implements  operations for work with event sections.
    /// </summary>
    public interface IEventSectionManager
    {
        /// <summary>
        /// Get all event sections.
        /// </summary>
        /// <returns>List of all event sections.</returns>
        Task<IEnumerable<EventSectionDTO>> GetEventSectionsDTOAsync();
    }
}