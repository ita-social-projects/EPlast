using AutoMapper;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
namespace EPlast.WebApi.Controllers.Params
{
    public class CitiesControllerParams
    {
        internal ILoggerService<CitiesController> logger;
        internal IMapper mapper;
        internal ICityService cityService;
        internal ICityDocumentsService cityDocumentsService;
        internal ICityAccessService cityAccessService;
        internal UserManager<User> userManager;
        internal ICityParticipantsService cityParticipantsService;
        internal IDistributedCache cache;
    }
}
