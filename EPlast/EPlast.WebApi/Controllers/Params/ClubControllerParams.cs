using AutoMapper;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace EPlast.WebApi.Controllers.Params
{
    public class ClubControllerParams
    {
        internal ILoggerService<ClubController> logger;
        internal IMapper mapper;
        internal IClubService clubService;
        internal IClubParticipantsService clubParticipantsService;
        internal IClubDocumentsService clubDocumentsService;
        internal UserManager<User> userManager;
        internal IClubAccessService clubAccessService;
        internal IDistributedCache cache;
    }
}
