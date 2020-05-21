using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Controllers
{
    public class ActionController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IHostingEnvironment _env;

        public ActionController(UserManager<User> userManager, IRepositoryWrapper repoWrapper, IHostingEnvironment env)
        {
            _userManager = userManager;
            _repoWrapper = repoWrapper;
            _env = env;
        }

        [Authorize]
        public IActionResult GetAction()
        {
            try
            {
                List<EventCategoryViewModel> _evc = _repoWrapper.EventCategory.FindAll()
                .Select(eventCategory => new EventCategoryViewModel() { EventCategory = eventCategory })
                .ToList();
                return View(_evc);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize]
        public IActionResult Events(int ID)
        {
            try
            {
                int actionID = _repoWrapper.EventType.FindByCondition(et => et.EventTypeName == "Акція").First().ID;
                int approvedStatus = _repoWrapper.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Учасник").First().ID;
                int undeterminedStatus = _repoWrapper.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Розглядається").First().ID;
                int rejectedStatus = _repoWrapper.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Відмовлено").First().ID;
                int approvedEvent = _repoWrapper.EventStatus.FindByCondition(st => st.EventStatusName == "Затверджений(-на)").First().ID;
                int finishedEvent = _repoWrapper.EventStatus.FindByCondition(st => st.EventStatusName == "Завершений(-на)").First().ID;
                int notApprovedEvent = _repoWrapper.EventStatus.FindByCondition(st => st.EventStatusName == "Не затверджені").First().ID;
                CheckEventsStatuses(ID, actionID, finishedEvent);
                List<GeneralEventViewModel> newEvents = _repoWrapper.Event
                 .FindByCondition(e => e.EventCategoryID == ID && e.EventTypeID == actionID)
                 .Include(e => e.EventAdmins)
                 .Include(e => e.Participants)
                 .Select(ev => new GeneralEventViewModel
                 {
                     Event = ev,
                     IsUserEventAdmin = (ev.EventAdmins.Any(e => e.UserID == _userManager.GetUserId(User))) || User.IsInRole("Адміністратор подій"),
                     IsUserParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(User)),
                     IsUserApprovedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(User) && p.ParticipantStatusId == approvedStatus),
                     IsUserUndeterminedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(User) && p.ParticipantStatusId == undeterminedStatus),
                     IsUserRejectedParticipant = ev.Participants.Any(p => p.UserId == _userManager.GetUserId(User) && p.ParticipantStatusId == rejectedStatus),
                     IsEventApproved = ev.EventStatusID == approvedEvent,
                     IsEventNotApproved = ev.EventStatusID == notApprovedEvent,
                     IsEventFinished = ev.EventStatusID == finishedEvent
                 }).ToList();
                return View(newEvents);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize]
        public IActionResult EventInfo(int ID)
        {
            try
            {
                int approvedStatus = _repoWrapper.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Учасник").First().ID;
                int undeterminedStatus = _repoWrapper.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Розглядається").First().ID;
                int rejectedStatus = _repoWrapper.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Відмовлено").First().ID;
                int finishedEvent = _repoWrapper.EventStatus.FindByCondition(st => st.EventStatusName == "Завершений(-на)").First().ID;
                bool isUserGlobalEventAdmin = User?.IsInRole("Адміністратор подій") ?? false;
                CheckEventStatus(ID, finishedEvent);
                EventViewModel eventModal = _repoWrapper.Event.FindByCondition(e => e.ID == ID)
                       .Include(e => e.Participants)
                            .ThenInclude(p => p.User)
                       .Include(e => e.Participants)
                            .ThenInclude(p => p.ParticipantStatus)
                       .Include(e => e.EventAdmins)
                       .ThenInclude(evAdm => evAdm.User)
                       .Include(e => e.EventStatus)
                       .Include(e => e.EventAdministrations)
                       .Include(e => e.EventType)
                       .Include(e => e.EventCategory)
                       .Include(e => e.EventGallarys)
                            .ThenInclude(eg => eg.Gallary)
                       .Select(e => new EventViewModel()
                       {
                           Event = e,
                           EventParticipants = e.Participants,
                           IsUserEventAdmin = (e.EventAdmins.Any(evAdm => evAdm.UserID == _userManager.GetUserId(User))) || isUserGlobalEventAdmin,
                           IsUserParticipant = e.Participants.Any(p => p.UserId == _userManager.GetUserId(User)),
                           IsUserApprovedParticipant = e.Participants.Any(p => p.UserId == _userManager.GetUserId(User) && p.ParticipantStatusId == approvedStatus),
                           IsUserUndeterminedParticipant = e.Participants.Any(p => p.UserId == _userManager.GetUserId(User) && p.ParticipantStatusId == undeterminedStatus),
                           IsUserRejectedParticipant = e.Participants.Any(p => p.UserId == _userManager.GetUserId(User) && p.ParticipantStatusId == rejectedStatus),
                           IsEventFinished = e.EventStatusID == finishedEvent
                       })
                       .First();

                if (!eventModal.IsUserEventAdmin)
                {
                    eventModal.EventParticipants = eventModal.EventParticipants.Where(p => p.ParticipantStatusId == approvedStatus);
                }

                return View(eventModal);
            }
            catch
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        private void CheckEventsStatuses(int ID, int actionID, int finishedEvent)
        {
            var eventsToCheck = _repoWrapper.Event
                 .FindByCondition(e => e.EventCategoryID == ID && e.EventTypeID == actionID);
            foreach (var eventToCheck in eventsToCheck)
            {
                if (eventToCheck.EventDateEnd.Date <= DateTime.Now.Date && eventToCheck.EventStatusID != finishedEvent)
                {
                    eventToCheck.EventStatusID = finishedEvent;
                    _repoWrapper.Event.Update(eventToCheck);
                }
            }
            _repoWrapper.Save();
        }

        private void CheckEventStatus(int ID, int finishedEvent)
        {
            var eventToCheck = _repoWrapper.Event.FindByCondition(e => e.ID == ID).First();
            if (eventToCheck.EventDateEnd.Date <= DateTime.Now.Date && eventToCheck.EventStatusID != finishedEvent)
            {
                eventToCheck.EventStatusID = finishedEvent;
                _repoWrapper.Event.Update(eventToCheck);
                _repoWrapper.Save();
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteEvent(int ID)
        {
            try
            {
                Event objectToDelete = _repoWrapper.Event.FindByCondition(e => e.ID == ID).First();
                _repoWrapper.Event.Delete(objectToDelete);
                _repoWrapper.Save();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeletePicture(int ID)
        {
            try
            {
                Gallary objectToDelete = _repoWrapper.Gallary.FindByCondition(g => g.ID == ID).First();
                _repoWrapper.Gallary.Delete(objectToDelete);
                var picturePath = Path.Combine(_env.WebRootPath, "images\\EventsGallery", objectToDelete.GalaryFileName);
                if (System.IO.File.Exists(picturePath))
                {
                    System.IO.File.Delete(picturePath);
                }
                _repoWrapper.Save();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult SubscribeOnEvent(int ID)
        {
            try
            {
                ParticipantStatus participantStatus = _repoWrapper.ParticipantStatus.FindByCondition(ps => ps.ParticipantStatusName == "Розглядається").First();
                int finishedEvent = _repoWrapper.EventStatus.FindByCondition(st => st.EventStatusName == "Завершений(-на)").First().ID;
                Event targetEvent = _repoWrapper.Event.FindByCondition(e => e.ID == ID).First();
                if (targetEvent.EventStatusID == finishedEvent)
                {
                    return StatusCode(409);
                }
                _repoWrapper.Participant.Create(new Participant() { ParticipantStatusId = participantStatus.ID, EventId = ID, UserId = _userManager.GetUserId(User) });
                _repoWrapper.Save();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult UnSubscribeOnEvent(int ID)
        {
            try
            {
                int rejectedStatus = _repoWrapper.ParticipantStatus.FindByCondition(p => p.ParticipantStatusName == "Відмовлено").First().ID;
                int finishedEvent = _repoWrapper.EventStatus.FindByCondition(st => st.EventStatusName == "Завершений(-на)").First().ID;
                Event targetEvent = _repoWrapper.Event.FindByCondition(e => e.ID == ID).First();
                Participant participantToDelete = _repoWrapper.Participant.FindByCondition(p => p.EventId == ID && p.UserId == _userManager.GetUserId(User)).First();
                if (participantToDelete.ParticipantStatusId == rejectedStatus || targetEvent.EventStatusID == finishedEvent)
                {
                    return StatusCode(409);
                }
                _repoWrapper.Participant.Delete(participantToDelete);
                _repoWrapper.Save();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        public IActionResult ApproveParticipant(int ID)
        {
            try
            {
                Participant participant = _repoWrapper.Participant.FindByCondition(p => p.ID == ID)
                    .Include(p => p.ParticipantStatus).First();
                ParticipantStatus participantStatus = _repoWrapper.ParticipantStatus.FindByCondition(ps => ps.ParticipantStatusName == "Учасник").First();
                participant.ParticipantStatus = participantStatus;
                _repoWrapper.Participant.Update(participant);
                _repoWrapper.Save();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        public IActionResult UndetermineParticipant(int ID)
        {
            try
            {
                Participant participant = _repoWrapper.Participant.FindByCondition(p => p.ID == ID)
                   .Include(p => p.ParticipantStatus).First();
                ParticipantStatus participantStatus = _repoWrapper.ParticipantStatus.FindByCondition(ps => ps.ParticipantStatusName == "Розглядається").First();
                participant.ParticipantStatus = participantStatus;
                _repoWrapper.Participant.Update(participant);
                _repoWrapper.Save();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        public IActionResult RejectParticipant(int ID)
        {
            try
            {
                Participant participant = _repoWrapper.Participant.FindByCondition(p => p.ID == ID)
                   .Include(p => p.ParticipantStatus).First();
                ParticipantStatus participantStatus = _repoWrapper.ParticipantStatus.FindByCondition(ps => ps.ParticipantStatusName == "Відмовлено").First();
                participant.ParticipantStatus = participantStatus;
                _repoWrapper.Participant.Update(participant);
                _repoWrapper.Save();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult FillEventGallery(int ID, IList<IFormFile> files)
        {
            try
            {
                foreach (IFormFile file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var img = Image.FromStream(file.OpenReadStream());
                        var uploads = Path.Combine(_env.WebRootPath, "images\\EventsGallery");
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploads, fileName);
                        img.Save(filePath);
                        var gallery = new Gallary() { GalaryFileName = fileName };
                        _repoWrapper.Gallary.Create(gallery);
                        _repoWrapper.EventGallary.Create(new EventGallary { EventID = ID, Gallary = gallery });
                    }
                }
                _repoWrapper.Save();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}