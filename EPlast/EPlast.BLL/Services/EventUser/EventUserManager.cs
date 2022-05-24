using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.EventUser
{
    /// <inheritdoc/>
    public class EventUserManager : IEventUserManager
    {
        private readonly IRepositoryWrapper repoWrapper;
        private readonly IMapper mapper;
        private readonly IEventCategoryManager eventCategoryManager;
        private readonly IEventStatusManager eventStatusManager;
        private readonly IEventAdministrationTypeManager eventAdministrationTypeManager;
        private readonly UserManager<User> _userManager;


        public EventUserManager(IRepositoryWrapper repoWrapper, IMapper mapper,
            IEventCategoryManager eventCategoryManager, IEventStatusManager eventStatusManager,
            IEventAdministrationTypeManager eventAdministrationTypeManager, UserManager<User> _userManager)
        {
            this.repoWrapper = repoWrapper;
            this.mapper = mapper;
            this.eventCategoryManager = eventCategoryManager;
            this.eventStatusManager = eventStatusManager;
            this.eventAdministrationTypeManager = eventAdministrationTypeManager;
            this._userManager = _userManager;
        }

        private int commandantTypeId;
        private int alternateTypeId;
        private int bunchuzhnyiTypeID;
        private int pysarTypeId;

        public async Task<EventCreateDTO> InitializeEventCreateDTOAsync()
        {
            var allUsers = await repoWrapper.User.GetAllAsync();

            return new EventCreateDTO()
            {
                Users = mapper.Map<IEnumerable<User>, IEnumerable<UserInfoDTO>>(allUsers.Where(u =>
                    (_userManager.GetRolesAsync(u).Result).Intersect(AllowedRolesToBeAdminOfEvent.roles).Any())),
                EventTypes = mapper.Map<IEnumerable<EventType>, IEnumerable<EventTypeDTO>>(await repoWrapper.EventType.GetAllAsync()),
                EventCategories = await eventCategoryManager.GetDTOAsync()
            };
        }

        public async Task<int> CreateEventAsync(EventCreateDTO model)
        {
            await GetAdministrationTypeId();
            model.Event.EventStatusID = await eventStatusManager.GetStatusIdAsync("Не затверджено");

            if (model.Event.EventDateStart >= model.Event.EventDateEnd)
            {
                throw new InvalidOperationException();
            }

            var eventToCreate = mapper.Map<EventCreationDTO, Event>(model.Event);

            var administrationList = new List<EventAdministration>
            {
                new EventAdministration
                {
                    UserID = model.Сommandant.UserId,
                    EventAdministrationTypeID = commandantTypeId,
                    EventID = eventToCreate.ID,
                },
                 new EventAdministration
                  {
                    UserID = model.Bunchuzhnyi.UserId,
                    EventAdministrationTypeID = bunchuzhnyiTypeID,
                    EventID = eventToCreate.ID,
                  },
                   new EventAdministration
                   {
                    UserID = model.Pysar.UserId,
                    EventAdministrationTypeID = pysarTypeId,
                    EventID = eventToCreate.ID,
                   },
            };
            if (model.Alternate.UserId != null)
            {
                administrationList.Add(new EventAdministration
                {
                    UserID = model.Alternate.UserId,
                    EventAdministrationTypeID = alternateTypeId,
                    EventID = eventToCreate.ID,
                });
            }
            eventToCreate.EventAdministrations = administrationList;

            await repoWrapper.Event.CreateAsync(eventToCreate);
            await repoWrapper.SaveAsync();

            return eventToCreate.ID;
        }

        public async Task<EventCreateDTO> InitializeEventEditDTOAsync(int eventId)
        {
            await GetAdministrationTypeId();

            return new EventCreateDTO()
            {
                Event = mapper.Map<Event, EventCreationDTO>(await repoWrapper.Event.GetFirstAsync(predicate: i => i.ID == eventId,
                        include: source => source.Include(i => i.EventAdministrations))),

                Users = mapper.Map<IEnumerable<User>, IEnumerable<UserInfoDTO>>(await repoWrapper.User.GetAllAsync()),

                EventTypes = mapper.Map<IEnumerable<EventType>, IEnumerable<EventTypeDTO>>(await repoWrapper.EventType.GetAllAsync()),

                EventCategories = await eventCategoryManager.GetDTOAsync(),

                Сommandant = mapper.Map<EventAdministration, EventAdministrationDTO>(await repoWrapper.EventAdministration.
                             GetFirstAsync(predicate: i => i.EventAdministrationTypeID == commandantTypeId
                             && i.EventID == eventId, include: source => source.Include(q => q.User))),

                Alternate = mapper.Map<EventAdministration, EventAdministrationDTO>(await repoWrapper.EventAdministration.
                            GetFirstOrDefaultAsync(predicate: i => i.EventAdministrationTypeID == alternateTypeId
                            && i.EventID == eventId, include: source => source.Include(q => q.User))),

                Bunchuzhnyi = mapper.Map<EventAdministration, EventAdministrationDTO>(await repoWrapper.EventAdministration.
                              GetFirstAsync(predicate: i => i.EventAdministrationTypeID == bunchuzhnyiTypeID
                              && i.EventID == eventId, include: source => source.Include(q => q.User))),

                Pysar = mapper.Map<EventAdministration, EventAdministrationDTO>(await repoWrapper.EventAdministration.
                        GetFirstAsync(predicate: i => i.EventAdministrationTypeID == pysarTypeId
                        && i.EventID == eventId, include: source => source.Include(q => q.User)))
            };
        }

        public async Task EditEventAsync(EventCreateDTO model)
        {
            await GetAdministrationTypeId();
            var eventToEdit = mapper.Map<EventCreationDTO, Event>(model.Event);
            List<EventAdministration> newAdmins = new List<EventAdministration> {
                new EventAdministration
                {
                    UserID = model.Сommandant.UserId,
                    EventAdministrationTypeID = commandantTypeId,
                    EventID = eventToEdit.ID,
                    ID = (await repoWrapper.EventAdministration.GetFirstAsync(predicate: i => i.EventAdministrationTypeID == commandantTypeId
                         && i.EventID == eventToEdit.ID, include: source => source.Include(q => q.User))).ID
                },
                  new EventAdministration
                  {
                    UserID = model.Bunchuzhnyi.UserId,
                    EventAdministrationTypeID = bunchuzhnyiTypeID,
                    EventID = eventToEdit.ID,
                    ID = (await repoWrapper.EventAdministration.GetFirstAsync(predicate: i => i.EventAdministrationTypeID == bunchuzhnyiTypeID
                         && i.EventID == eventToEdit.ID, include: source => source.Include(q => q.User))).ID
                  },
                   new EventAdministration
                   {
                    UserID = model.Pysar.UserId,
                    EventAdministrationTypeID = pysarTypeId,
                    EventID = eventToEdit.ID,
                    ID = (await repoWrapper.EventAdministration.GetFirstAsync(predicate: i => i.EventAdministrationTypeID == pysarTypeId
                         && i.EventID == eventToEdit.ID, include: source => source.Include(q => q.User))).ID
                   }
            };

            var tempAlter = await repoWrapper.EventAdministration
                  .GetFirstOrDefaultAsync(predicate: i => i.EventAdministrationTypeID == alternateTypeId
                  && i.EventID == eventToEdit.ID, include: source => source.Include(q => q.User));

            if (tempAlter != null)  repoWrapper.EventAdministration.Delete(tempAlter); 

            if (model.Alternate.UserId != null)
            {
                newAdmins.Add(new EventAdministration
                {
                    UserID = model.Alternate.UserId,
                    EventAdministrationTypeID = alternateTypeId,
                    EventID = eventToEdit.ID,
                });
            };
            
            eventToEdit.EventAdministrations = newAdmins;
            repoWrapper.Event.Update(eventToEdit);
            await repoWrapper.SaveAsync();
        }

        public async Task<int> ApproveEventAsync(int id)
        {
            try
            {
                var eventToApprove = await repoWrapper.Event.GetFirstAsync(r => r.ID == id);
                eventToApprove.EventStatusID = await eventStatusManager.GetStatusIdAsync("Затверджено");

                repoWrapper.Event.Update(eventToApprove);
                await repoWrapper.SaveAsync();

                return StatusCodes.Status200OK;
            }
            catch
            {
                return StatusCodes.Status400BadRequest;
            }
        }

        private async Task GetAdministrationTypeId()
        {
            commandantTypeId = await eventAdministrationTypeManager.GetTypeIdAsync("Комендант");
            alternateTypeId = await eventAdministrationTypeManager.GetTypeIdAsync("Заступник коменданта");
            bunchuzhnyiTypeID = await eventAdministrationTypeManager.GetTypeIdAsync("Бунчужний");
            pysarTypeId = await eventAdministrationTypeManager.GetTypeIdAsync("Писар");
        }
    }
}
