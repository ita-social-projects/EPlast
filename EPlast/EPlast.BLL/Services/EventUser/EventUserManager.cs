﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        private readonly IUserAccessService _userAccesses;



        public EventUserManager(IRepositoryWrapper repoWrapper, IMapper mapper,
            IEventCategoryManager eventCategoryManager, IEventStatusManager eventStatusManager,
            IEventAdministrationTypeManager eventAdministrationTypeManager, UserManager<User> _userManager, IUserAccessService _userAccesses)
        {
            this.repoWrapper = repoWrapper;
            this.mapper = mapper;
            this.eventCategoryManager = eventCategoryManager;
            this.eventStatusManager = eventStatusManager;
            this.eventAdministrationTypeManager = eventAdministrationTypeManager;
            this._userManager = _userManager;
            this._userAccesses = _userAccesses;
        }

        private int commandantTypeId;
        private int alternateTypeId;
        private int bunchuzhnyiTypeID;
        private int pysarTypeId;

        public async Task<EventCreateDto> InitializeEventCreateDTOAsync()
        {
            var allUsers = await repoWrapper.User.GetAllAsync();

            return new EventCreateDto()
            {
                Users = mapper.Map<IEnumerable<User>, IEnumerable<UserInfoDto>>(allUsers.Where(u =>
                    (_userManager.GetRolesAsync(u).Result).Intersect(AllowedRolesToBeAdminOfEvent.Roles).Any())),
                EventTypes = mapper.Map<IEnumerable<EventType>, IEnumerable<EventTypeDto>>(await repoWrapper.EventType.GetAllAsync()),
                EventCategories = await eventCategoryManager.GetDTOAsync()
            };
        }

        public async Task<int> CreateEventAsync(EventCreateDto model)
        {
            await GetAdministrationTypeId();
            model.Event.EventStatusID = await eventStatusManager.GetStatusIdAsync("Не затверджено");

            if (model.Event.EventDateStart >= model.Event.EventDateEnd)
            {
                throw new InvalidOperationException("End date was before start day");
            }

            if (model.Event.EventDateStart <= DateTime.Now)
            {
                throw new InvalidOperationException("Start date can not be before today");
            }

            var eventToCreate = mapper.Map<EventCreationDto, Event>(model.Event);

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

        public async Task<EventCreateDto> InitializeEventEditDTOAsync(int eventId)
        {
            await GetAdministrationTypeId();

            return new EventCreateDto()
            {
                Event = mapper.Map<Event, EventCreationDto>(await repoWrapper.Event.GetFirstAsync(predicate: i => i.ID == eventId,
                        include: source => source.Include(i => i.EventAdministrations))),

                Users = mapper.Map<IEnumerable<User>, IEnumerable<UserInfoDto>>(await repoWrapper.User.GetAllAsync()),

                EventTypes = mapper.Map<IEnumerable<EventType>, IEnumerable<EventTypeDto>>(await repoWrapper.EventType.GetAllAsync()),

                EventCategories = await eventCategoryManager.GetDTOAsync(),

                Сommandant = mapper.Map<EventAdministration, EventAdministrationDto>(await repoWrapper.EventAdministration.
                             GetFirstAsync(predicate: i => i.EventAdministrationTypeID == commandantTypeId
                             && i.EventID == eventId, include: source => source.Include(q => q.User))),

                Alternate = mapper.Map<EventAdministration, EventAdministrationDto>(await repoWrapper.EventAdministration.
                            GetFirstOrDefaultAsync(predicate: i => i.EventAdministrationTypeID == alternateTypeId
                            && i.EventID == eventId, include: source => source.Include(q => q.User))),

                Bunchuzhnyi = mapper.Map<EventAdministration, EventAdministrationDto>(await repoWrapper.EventAdministration.
                              GetFirstAsync(predicate: i => i.EventAdministrationTypeID == bunchuzhnyiTypeID
                              && i.EventID == eventId, include: source => source.Include(q => q.User))),

                Pysar = mapper.Map<EventAdministration, EventAdministrationDto>(await repoWrapper.EventAdministration.
                        GetFirstAsync(predicate: i => i.EventAdministrationTypeID == pysarTypeId
                        && i.EventID == eventId, include: source => source.Include(q => q.User)))
            };
        }

        public async Task<bool> EditEventAsync(EventCreateDto model, User currentUser)
        {
            var userAccesses = await _userAccesses.GetUserEventAccessAsync(currentUser.Id, currentUser, model.Event.ID);
            if (!userAccesses["EditEvent"]) return false;

            await GetAdministrationTypeId();
            var eventToEdit = mapper.Map<EventCreationDto, Event>(model.Event);
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
            }
            
            eventToEdit.EventAdministrations = newAdmins;
            repoWrapper.Event.Update(eventToEdit);
            await repoWrapper.SaveAsync();
            return true;
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
