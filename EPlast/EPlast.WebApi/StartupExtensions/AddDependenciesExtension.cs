﻿using EPlast.BLL;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Interfaces.Blank;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.BLL.Interfaces.EventCalendar;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.BLL.Interfaces.Jwt;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Interfaces.RegionBoard;
using EPlast.BLL.Interfaces.Resources;
using EPlast.BLL.Interfaces.Statistics;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.SecurityModel;
using EPlast.BLL.Services;
using EPlast.BLL.Services.ActiveMembership;
using EPlast.BLL.Services.Admin;
using EPlast.BLL.Services.Auth;
using EPlast.BLL.Services.AzureStorage;
using EPlast.BLL.Services.AzureStorage.Base;
using EPlast.BLL.Services.Blank;
using EPlast.BLL.Services.City;
using EPlast.BLL.Services.City.CityAccess;
using EPlast.BLL.Services.Club;
using EPlast.BLL.Services.Club.ClubAccess;
using EPlast.BLL.Services.Distinctions;
using EPlast.BLL.Services.EmailSending;
using EPlast.BLL.Services.Events;
using EPlast.BLL.Services.EventUser;
using EPlast.BLL.Services.GoverningBodies;
using EPlast.BLL.Services.GoverningBodies.Sector;
using EPlast.BLL.Services.Interfaces;
using EPlast.BLL.Services.Jwt;
using EPlast.BLL.Services.Logging;
using EPlast.BLL.Services.Notifications;
using EPlast.BLL.Services.PDF;
using EPlast.BLL.Services.Precautions;
using EPlast.BLL.Services.Region;
using EPlast.BLL.Services.Region.RegionAccess;
using EPlast.BLL.Services.Statistics;
using EPlast.BLL.Services.UserProfiles;
using EPlast.BLL.Settings;
using EPlast.DataAccess.Repositories;
using EPlast.DataAccess.Repositories.Realizations.Base;
using EPlast.WebApi.WebSocketHandlers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EPlast.WebApi.StartupExtensions
{
    public static class AddDependencyExtension
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
            services.AddScoped<CityAccessSettings, CityAccessSettings>();
            services.AddScoped<ClubAccessSettings, ClubAccessSettings>();
            services.AddScoped<IAccessLevelService, AccessLevelService>();
            services.AddScoped<IActionManager, ActionManager>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IAdminTypeService, AdminTypeService>();
            services.AddScoped<IAnnualReportService, AnnualReportService>();
            services.AddScoped<IAuthEmailService, AuthEmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBlankAchievementBlobStorageRepository, BlankAchievementBlobStorageRepository>();
            services.AddScoped<IBlankAchievementDocumentService, AchievementDocumentService>();
            services.AddScoped<IBlankBiographyDocumentService, BlankBiographyDocumentsService>();
            services.AddScoped<IBlankExtractFromUPUBlobStorageRepository, BlankExtractFromUpuBlobStorageRepository>();
            services.AddScoped<IBlankExtractFromUPUDocumentService, BlankExtractFromUpuDocumentService>();
            services.AddScoped<IBlankFilesBlobStorageRepository, BlankFilesBlobStorageRepository>();
            services.AddScoped<ICityAccessService, CityAccessService>();
            services.AddScoped<ICityBlobStorageRepository, CityBlobStorageRepository>();
            services.AddScoped<ICityDocumentsService, CityDocumentsService>();
            services.AddScoped<ICityFilesBlobStorageRepository, CityFilesBlobStorageRepository>();
            services.AddScoped<ICityParticipantsService, CityParticipantsService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICityStatisticsService, StatisticsService>();
            services.AddScoped<IClubAccessService, ClubAccessService>();
            services.AddScoped<IClubAnnualReportService, ClubAnnualReportService>();
            services.AddScoped<IClubBlobStorageRepository, ClubBlobStorageRepository>();
            services.AddScoped<IClubBlobStorageRepository, ClubBlobStorageRepository>();
            services.AddScoped<IClubDocumentsService, ClubDocumentsService>();
            services.AddScoped<IClubFilesBlobStorageRepository, ClubFilesBlobStorageRepository>();
            services.AddScoped<IClubParticipantsService, ClubParticipantsService>();
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<IConfirmedUsersService, ConfirmedUsersService>();
            services.AddScoped<IDateTimeHelper, DateTimeHelper>();
            services.AddScoped<IDecisionBlobStorageRepository, DecisionBlobStorageRepository>();
            services.AddScoped<IDecisionService, DecisionService>();
            services.AddScoped<IDecisionVmInitializer, DecisionVmInitializer>();
            services.AddScoped<IDirectoryManager, DirectoryManager>();
            services.AddScoped<IDistinctionService, DistinctionService>();
            services.AddScoped<IEducatorsStaffService, EducatorsStaffService>();
            services.AddScoped<IEducatorsStaffTypesService, EducatorsStaffTypesService>();
            services.AddScoped<IEmailContentService, EmailContentService>();
            services.AddScoped<IEmailReminderService, EmailReminderService>();
            services.AddScoped<IEmailSendingService, EmailSendingService>();
            services.AddScoped<IEventAdmininistrationManager, EventAdministrationManager>();
            services.AddScoped<IEventAdministrationTypeManager, EventAdministrationTypeManager>();
            services.AddScoped<IEventAdminManager, EventAdminManager>();
            services.AddScoped<IEventBlobStorageRepository, EventBlobStorageRepository>();
            services.AddScoped<IEventCalendarService, EventCalendarService>();
            services.AddScoped<IEventCategoryManager, EventCategoryManager>();
            services.AddScoped<IEventGalleryManager, EventGalleryManager>();
            services.AddScoped<IEventsManager, EventsManager>();
            services.AddScoped<IEventStatusManager, EventStatusManager>();
            services.AddScoped<IEventTypeManager, EventTypeManager>();
            services.AddScoped<IEventUserManager, EventUserManager>();
            services.AddScoped<IEventWrapper, EventWrapper>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IFileStreamManager, FileStreamManager>();
            services.AddScoped<IGlobalLoggerService, GlobalLoggerService>();
            services.AddScoped<IRegionsBoardService, RegionsBoardService>();
            services.AddScoped<IGoverningBodiesService, GoverningBodiesService>();
            services.AddScoped<IGoverningBodyAdministrationService, GoverningBodyAdministrationService>();
            services.AddScoped<IGoverningBodyDocumentsService, GoverningBodyDocumentsService>();
            services.AddScoped<IGoverningBodyBlobStorageRepository, GoverningBodyBlobStorageRepository>();
            services.AddScoped<IGoverningBodyFilesBlobStorageRepository, GoverningBodyFilesBlobStorageRepository>();
            services.AddScoped<ISectorService, SectorService>();
            services.AddScoped<ISectorAdministrationService, SectorAdministrationService>();
            services.AddScoped<ISectorDocumentsService, SectorDocumentsService>();
            services.AddScoped<IGoverningBodySectorBlobStorageRepository, GoverningBodySectorBlobStorageRepository>();
            services.AddScoped<IGoverningBodySectorFilesBlobStorageRepository, GoverningBodySectorFilesBlobStorageRepository>();
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IMethodicDocumentBlobStorageRepository, MethodicDocumentBlobStarageRepository>();
            services.AddScoped<IMethodicDocumentService, MethodicDocumentService>();
            services.AddScoped<IMethodicDocumentService, MethodicDocumentService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INewPlastMemberEmailGreetingService, NewPlastMemberEmailGreetingService>();
            services.AddScoped<IParticipantManager, ParticipantManager>();
            services.AddScoped<IParticipantStatusManager, ParticipantStatusManager>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IPlastDegreeService, PlastDegreeService>();
            services.AddScoped<IPrecautionService, PrecautionService>();
            services.AddScoped<IRegionAccessService, RegionAccessService>();
            services.AddScoped<IRegionAdministrationService, RegionAdministrationService>();
            services.AddScoped<IRegionAnnualReportService, RegionAnnualReportService>();
            services.AddScoped<IRegionBlobStorageRepository, RegionBlobStorageRepository>();
            services.AddScoped<IRegionFilesBlobStorageRepository, RegionFilesBlobStorageRepository>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IRegionStatisticsService, StatisticsService>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IResources, BLL.Services.Resources.Resources>();
            services.AddScoped<IUserBlobStorageRepository, UserBlobStorageRepository>();
            services.AddScoped<IUserDatesService, UserDatesService>();
            services.AddScoped<IUserDistinctionService, UserDistinctionService>();
            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<IUserPersonalDataService, UserPersonalDataService>();
            services.AddScoped<IUserPrecautionService, UserPrecautionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<RegionAccessSettings>();
            services.AddScoped<StatisticsServiceSettings>();
            services.AddScoped<ISecurityModel, SecurityModel>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IAzureBlobConnectionFactory, AzureBlobConnectionFactory>();
            services.AddSingleton<INotificationConnectionManager, NotificationConnectionManager>();
            services.AddSingleton<UserNotificationHandler>();
            services.AddTransient<IEventUserService, EventUserService>();
            services.AddTransient<IEventUserService, EventUserService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IUniqueIdService, UniqueIdService>();
            services.AddSingleton<IUserMapService, UserMapService>();
            return services;
        }
    }
}
