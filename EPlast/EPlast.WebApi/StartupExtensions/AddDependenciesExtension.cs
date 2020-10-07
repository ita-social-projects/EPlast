using EPlast.BLL;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.BLL.Interfaces.EventCalendar;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.Jwt;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services;
using EPlast.BLL.Services.ActiveMembership;
using EPlast.BLL.Services.Admin;
using EPlast.BLL.Services.AzureStorage;
using EPlast.BLL.Services.AzureStorage.Base;
using EPlast.BLL.Services.City;
using EPlast.BLL.Services.City.CityAccess;
using EPlast.BLL.Services.Club;
using EPlast.BLL.Services.Distinctions;

using EPlast.BLL.Services.Events;
using EPlast.BLL.Services.EventUser;
using EPlast.BLL.Services.Interfaces;
using EPlast.BLL.Services.Jwt;
using EPlast.BLL.Services.Logging;
using EPlast.BLL.Services.Region;
using EPlast.BLL.Services.UserProfiles;
using EPlast.BLL.Settings;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Repositories;
using EPlast.DataAccess.Repositories.Realizations.Base;
using Microsoft.Extensions.DependencyInjection;

namespace EPlast.WebApi.StartupExtensions
{
    public static class AddDependencyExtension
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IEmailConfirmation, EmailConfirmationService>();
            services.AddScoped<IDecisionVmInitializer, DecisionVmInitializer>();
            services.AddScoped<CityAccessSettings, CityAccessSettings>();
            services.AddScoped<ICityAccessService, CityAccessService>();
            services.AddScoped<ICityMembersService, CityMembersService>();
            services.AddScoped<IAnnualReportService, AnnualReportService>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IDecisionService, DecisionService>();
            services.AddScoped<IDirectoryManager, DirectoryManager>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IFileStreamManager, FileStreamManager>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INationalityService, NationalityService>();
            services.AddScoped<IReligionService, ReligionService>();
            services.AddScoped<IEducationService, EducationService>();
            services.AddScoped<IWorkService, WorkService>();
            services.AddScoped<IGenderService, GenderService>();
            services.AddScoped<IDegreeService, DegreeService>();
            services.AddScoped<IConfirmedUsersService, ConfirmedUsersService>();
            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ICityAdministrationService, CityAdministrationService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICityDocumentsService, CityDocumentsService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IRegionAdministrationService, RegionAdministrationService>();
            services.AddScoped<IGlobalLoggerService, GlobalLoggerService>();
            services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<IAdminTypeService, AdminTypeService>();
            services.AddScoped<IClubAdministrationService, ClubAdministrationService>();
            services.AddScoped<IClubMembersService, ClubMembersService>();
            services.AddScoped<IActionManager, ActionManager>();
            services.AddScoped<IEventCategoryManager, EventCategoryManager>();
            services.AddScoped<IEventTypeManager, EventTypeManager>();
            services.AddScoped<IEventStatusManager, EventStatusManager>();
            services.AddScoped<IParticipantStatusManager, ParticipantStatusManager>();
            services.AddScoped<IParticipantManager, ParticipantManager>();
            services.AddScoped<IEventGalleryManager, EventGalleryManager>();
            services.AddScoped<IEventUserManager, EventUserManager>();
            services.AddScoped<IEventAdminManager, EventAdminManager>();
            services.AddScoped<IEventAdmininistrationManager, EventAdministrationManager>();
            services.AddScoped<IEventAdministrationTypeManager, EventAdministrationTypeManager>();
            services.AddScoped<IEventCalendarService, EventCalendarService>();
            services.AddScoped<IEventsManager, EventsManager>();
            services.AddScoped<IDateTimeHelper, DateTimeHelper>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddScoped<IUserBlobStorageRepository, UserBlobStorageRepository>();
            services.AddScoped<IDecisionBlobStorageRepository, DecisionBlobStorageRepository>();
            services.AddScoped<ICityBlobStorageRepository, CityBlobStorageRepository>();
            services.AddScoped<ICityFilesBlobStorageRepository, CityFilesBlobStorageRepository>();
            services.AddScoped<IRegionBlobStorageRepository, RegionBlobStorageRepository>();
            services.AddScoped<IRegionFilesBlobStorageRepository, RegionFilesBlobStorageRepository>();
            services.AddScoped<IClubBlobStorageRepository, ClubBlobStorageRepository>();
            services.AddScoped<IEventBlobStorageRepository, EventBlobStorageRepository>();
            services.AddSingleton<IAzureBlobConnectionFactory, AzureBlobConnectionFactory>();
            services.AddScoped<IAccessLevelService, AccessLevelService>();
            services.AddScoped<IPlastDegreeService, PlastDegreeService>();
            services.AddScoped<IUserDistinctionService, UserDistinctionService>();
            services.AddScoped<IDistinctionService, DistinctionService>();
            services.AddScoped<IEducatorsStaffService, EducatorsStaffService>();
            services.AddScoped<IEducatorsStaffTypesService, EducatorsStaffTypesService>();

            return services;
        }
    }
}
