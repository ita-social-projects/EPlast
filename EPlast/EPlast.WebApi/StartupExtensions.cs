using System;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.AboutBase;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.Admin;
using EPlast.BLL.Interfaces.Announcements;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.AzureStorage.Base;
using EPlast.BLL.Interfaces.Blank;
using EPlast.BLL.Interfaces.Cache;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.BLL.Interfaces.EventCalendar;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.BLL.Interfaces.FormerMember;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.BLL.Interfaces.HostURL;
using EPlast.BLL.Interfaces.Jwt;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Notifications;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Interfaces.RegionAdministrations;
using EPlast.BLL.Interfaces.RegionBoard;
using EPlast.BLL.Interfaces.Resources;
using EPlast.BLL.Interfaces.Statistics;
using EPlast.BLL.Interfaces.Terms;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.SecurityModel;
using EPlast.BLL.Services;
using EPlast.BLL.Services.AboutBase;
using EPlast.BLL.Services.ActiveMembership;
using EPlast.BLL.Services.Admin;
using EPlast.BLL.Services.Announcements;
using EPlast.BLL.Services.Auth;
using EPlast.BLL.Services.AzureStorage;
using EPlast.BLL.Services.AzureStorage.Base;
using EPlast.BLL.Services.Blank;
using EPlast.BLL.Services.City;
using EPlast.BLL.Services.City.CityAccess;
using EPlast.BLL.Services.Club;
using EPlast.BLL.Services.Club.ClubAccess;
using EPlast.BLL.Services.Distinctions;
using EPlast.BLL.Services.EducatorsStaff;
using EPlast.BLL.Services.EmailSending;
using EPlast.BLL.Services.Events;
using EPlast.BLL.Services.EventUser;
using EPlast.BLL.Services.EventUser.EventUserAccess;
using EPlast.BLL.Services.FormerMember;
using EPlast.BLL.Services.GoverningBodies;
using EPlast.BLL.Services.GoverningBodies.Announcement;
using EPlast.BLL.Services.GoverningBodies.Sector;
using EPlast.BLL.Services.HostURL;
using EPlast.BLL.Services.Interfaces;
using EPlast.BLL.Services.Jwt;
using EPlast.BLL.Services.Logging;
using EPlast.BLL.Services.Notifications;
using EPlast.BLL.Services.PDF;
using EPlast.BLL.Services.Precautions;
using EPlast.BLL.Services.Redis;
using EPlast.BLL.Services.Region;
using EPlast.BLL.Services.Region.RegionAccess;
using EPlast.BLL.Services.RegionAdministrations;
using EPlast.BLL.Services.Statistics;
using EPlast.BLL.Services.TermsOfUse;
using EPlast.BLL.Services.UserAccess;
using EPlast.BLL.Services.UserProfiles;
using EPlast.BLL.Settings;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.DataAccess.Repositories.Realizations.Base;
using EPlast.Resources;
using EPlast.WebApi.WebSocketHandlers;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPlast.WebApi
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
            services.AddScoped<CityAccessSettings, CityAccessSettings>();
            services.AddScoped<ClubAccessSettings, ClubAccessSettings>();
            services.AddScoped<IAboutBaseBlobStorageRepository, AboutBaseBlobStorageRepository>();
            services.AddScoped<IAboutBasePicturesManager, AboutBasePicturesManager>();
            services.AddScoped<IAboutBaseSectionService, AboutBaseSectionService>();
            services.AddScoped<IAboutBaseSubsectionService, AboutBaseSubsectionService>();
            services.AddScoped<IAboutBaseWrapper, AboutBaseWrapper>();
            services.AddScoped<IAccessLevelService, AccessLevelService>();
            services.AddScoped<IActionManager, ActionManager>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IAdminTypeService, AdminTypeService>();
            services.AddScoped<IAnnouncemetsService, AnnouncementsService>();
            services.AddScoped<IAnnualReportAccessService, AnnualReportAccessService>();
            services.AddScoped<IAnnualReportService, AnnualReportService>();
            services.AddScoped<IAuthEmailService, AuthEmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBlankAchievementBlobStorageRepository, BlankAchievementBlobStorageRepository>();
            services.AddScoped<IBlankAchievementDocumentService, AchievementDocumentService>();
            services.AddScoped<IBlankBiographyDocumentService, BlankBiographyDocumentsService>();
            services.AddScoped<IBlankExtractFromUpuBlobStorageRepository, BlankExtractFromUpuBlobStorageRepository>();
            services.AddScoped<IBlankExtractFromUpuDocumentService, BlankExtractFromUpuDocumentService>();
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
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IDecisionBlobStorageRepository, DecisionBlobStorageRepository>();
            services.AddScoped<IDirectoryManager, DirectoryManager>();
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
            services.AddScoped<IEventSectionManager, EventSectionManager>();
            services.AddScoped<IEventsManager, EventsManager>();
            services.AddScoped<IEventStatusManager, EventStatusManager>();
            services.AddScoped<IEventTypeManager, EventTypeManager>();
            services.AddScoped<IEventUserAccessService, EventUserAccessService>();
            services.AddScoped<IEventUserManager, EventUserManager>();
            services.AddScoped<IEventWrapper, EventWrapper>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IFileStreamManager, FileStreamManager>();
            services.AddScoped<IFormerMemberAdminService, FormerMemberAdminService>();
            services.AddScoped<IFormerMemberService, FormerMemberService>();
            services.AddScoped<IGlobalLoggerService, GlobalLoggerService>();
            services.AddScoped<IGoverningBodiesService, GoverningBodiesService>();
            services.AddScoped<IGoverningBodyAdministrationService, GoverningBodyAdministrationService>();
            services.AddScoped<IGoverningBodyAnnouncementService, GoverningBodyAnnouncementService>();
            services.AddScoped<IGoverningBodyBlobStorageRepository, GoverningBodyBlobStorageRepository>();
            services.AddScoped<IGoverningBodyBlobStorageService, GoverningBodyBlobStorageService>();
            services.AddScoped<IGoverningBodyDocumentsService, GoverningBodyDocumentsService>();
            services.AddScoped<IGoverningBodyFilesBlobStorageRepository, GoverningBodyFilesBlobStorageRepository>();
            services.AddScoped<IGoverningBodySectorBlobStorageRepository, GoverningBodySectorBlobStorageRepository>();
            services.AddScoped<IGoverningBodySectorFilesBlobStorageRepository, GoverningBodySectorFilesBlobStorageRepository>();
            services.AddScoped<IHtmlService, HtmlService>();
            services.AddScoped<IMethodicDocumentBlobStorageRepository, MethodicDocumentBlobStarageRepository>();
            services.AddScoped<IMethodicDocumentService, MethodicDocumentService>();
            services.AddScoped<IMethodicDocumentService, MethodicDocumentService>();
            services.AddScoped<INewPlastMemberEmailGreetingService, NewPlastMemberEmailGreetingService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IParticipantManager, ParticipantManager>();
            services.AddScoped<IParticipantStatusManager, ParticipantStatusManager>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IPicturesManager, PicturesManager>();
            services.AddScoped<IPlastDegreeService, PlastDegreeService>();
            services.AddScoped<IRegionAccessService, RegionAccessService>();
            services.AddScoped<IRegionAdministrationService, RegionAdministrationService>();
            services.AddScoped<IRegionAnnualReportService, RegionAnnualReportService>();
            services.AddScoped<IRegionBlobStorageRepository, RegionBlobStorageRepository>();
            services.AddScoped<IRegionFilesBlobStorageRepository, RegionFilesBlobStorageRepository>();
            services.AddScoped<IRegionsBoardService, RegionsBoardService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IRegionStatisticsService, StatisticsService>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IResources, BLL.Services.Resources.Resources>();
            services.AddScoped<ISectorAdministrationService, SectorAdministrationService>();
            services.AddScoped<ISectorAnnouncementsService, SectorAnnouncementsService>();
            services.AddScoped<ISectorDocumentsService, SectorDocumentsService>();
            services.AddScoped<ISectorService, SectorService>();
            services.AddScoped<ISecurityModel, SecurityModel>();
            services.AddScoped<ITermsService, TermsService>();
            services.AddScoped<IUserAccessService, UserAccessService>();
            services.AddScoped<IUserAccessWrapper, UserAccessWrapper>();
            services.AddScoped<IUserBlobStorageRepository, UserBlobStorageRepository>();
            services.AddScoped<IUserCourseService, UserCourseService>();
            services.AddScoped<IUserDatesService, UserDatesService>();
            services.AddScoped<IUserDistinctionService, UserDistinctionService>();
            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<IUserPersonalDataService, UserPersonalDataService>();
            services.AddScoped<IUserPrecautionService, UserPrecautionService>();
            services.AddScoped<IUserProfileAccessService, UserProfileAccessService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<RegionAccessSettings>();
            services.AddScoped<StatisticsServiceSettings>();
            services.AddScoped<IRegionAdministrationAccessService, RegionAdministrationAccessService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IAzureBlobConnectionFactory, AzureBlobConnectionFactory>();
            services.AddSingleton<ICacheService, RedisCacheService>();
            services.AddSingleton<INotificationConnectionManager, NotificationConnectionManager>();
            services.AddSingleton<IUserMapService, UserMapService>();
            services.AddSingleton<UserNotificationHandler>();
            services.AddTransient<IEventUserService, EventUserService>();
            services.AddTransient<IEventUserService, EventUserService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddSingleton<IUserMapService, UserMapService>();
            services.AddSingleton<ICacheService, RedisCacheService>();
            services.AddSingleton<IHostUrlService, HostUrlService>();
            return services;
        }


        public static void AddAppRecurringJobs(
            this IServiceProvider serviceProvider,
            IRecurringJobManager recurringJobManager,
            IConfiguration Configuration)
        {
            recurringJobManager.AddOrUpdate("Run every day",
                                            () => serviceProvider.GetService<IPlastDegreeService>()
                                                                 .GetDergeesAsync(),
                                            "59 23 * * *",
                                            TimeZoneInfo.Local);

            recurringJobManager.AddOrUpdate("Cheak register users and sent notifications to admins",
                                            () => serviceProvider.GetService<IUserService>()
                                                                 .CheckRegisteredUsersAsync(),
                                            "1 * * * *", // every day at 01:00
                                            TimeZoneInfo.Local);

            recurringJobManager.AddOrUpdate("Cheak register users and sent notifications to admins",
                                         () => serviceProvider.GetService<IUserService>()
                                                              .CheckRegisteredWithoutCityUsersAsync(),
                                             "1 * * * *", // every day at 01:00
                                            TimeZoneInfo.Local);

            recurringJobManager.AddOrUpdate("Check and change event status",
                                            () => serviceProvider.GetService<IActionManager>()
                                                                 .CheckEventsStatusesAsync(),
                                            "0 * * * *",
                                            TimeZoneInfo.Local);

            recurringJobManager.AddOrUpdate("Changes status of region admins when the date expires",
                                            () => serviceProvider.GetService<IRegionService>()
                                                                 .ContinueAdminsDueToDate(),
                                            "59 23 * * *", TimeZoneInfo.Local);
            CreateRolesAsync(serviceProvider, Configuration).Wait();

            recurringJobManager.AddOrUpdate("Changes status of club admins when the date expires",
                                            () => serviceProvider.GetService<IClubParticipantsService>()
                                                                 .ContinueAdminsDueToDate(),
                                            "59 23 * * *", TimeZoneInfo.Local);

            recurringJobManager.AddOrUpdate("Changes status of city admins when the date expires",
                                            () => serviceProvider.GetService<ICityParticipantsService>()
                                                                 .ContinueAdminsDueToDate(),
                                            "59 23 * * *", TimeZoneInfo.Local);

            recurringJobManager.AddOrUpdate("Reminder to join city",
                                            () => serviceProvider.GetService<IEmailReminderService>()
                                                                 .JoinCityReminderAsync(),
                                            "0 12 * * Mon", TimeZoneInfo.Local);
            recurringJobManager.AddOrUpdate("Reminder to approve new city followers",
                                            () => serviceProvider.GetService<IEmailReminderService>()
                                                                 .RemindCityAdminsToApproveFollowers(),
                                            "0 12 * * Mon", TimeZoneInfo.Local);
            recurringJobManager.AddOrUpdate("New Plast members greeting",
                                            () => serviceProvider.GetService<INewPlastMemberEmailGreetingService>()
                                                                 .NotifyNewPlastMembersAndCityAdminsAsync(),
                                            Cron.Daily(), TimeZoneInfo.Local);
            recurringJobManager.AddOrUpdate("Delete unconfirmed users every hour",
                                            () => serviceProvider.GetService<IAuthService>()
                                                                 .DeleteUserIfEmailNotConfirmedAsync(),
                                            "0 */1 * * *", TimeZoneInfo.Local);

            recurringJobManager.AddOrUpdate("Changes status of GoverningBody admins when the date expires",
                                            () => serviceProvider.GetService<IGoverningBodiesService>()
                                                                .ContinueGoverningBodyAdminsDueToDateAsync(),
                                            "59 23 * * *", TimeZoneInfo.Local);

            recurringJobManager.AddOrUpdate("Changes status of Sector admins when the date expires",
                                            () => serviceProvider.GetService<ISectorService>()
                                                                .ContinueSectorAdminsDueToDateAsync(),
                                            "59 23 * * *", TimeZoneInfo.Local);
        }

        private static async Task CreateRolesAsync(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var userDatesService = serviceProvider.GetRequiredService<IUserDatesService>();
            var roles = new[]
            {
                Roles.Admin,
                Roles.Supporter,
                Roles.PlastMember,
                Roles.PlastHead,
                Roles.EventAdministrator,
                Roles.KurinHead,
                Roles.KurinHeadDeputy,
                Roles.KurinSecretary,
                Roles.OkrugaHead,
                Roles.OkrugaHeadDeputy,
                Roles.OkrugaSecretary,
                Roles.OkrugaReferentUPS,
                Roles.OkrugaReferentUSP,
                Roles.OkrugaReferentOfActiveMembership,
                Roles.CityHead,
                Roles.CityHeadDeputy,
                Roles.CitySecretary,
                Roles.CityReferentUPS,
                Roles.CityReferentUSP,
                Roles.CityReferentOfActiveMembership,
                Roles.FormerPlastMember,
                Roles.RegisteredUser,
                Roles.Interested,
                Roles.GoverningBodyAdmin,
                Roles.GoverningBodyHead,
                Roles.GoverningBodySecretary,
                Roles.GoverningBodySectorHead,
                Roles.GoverningBodySectorSecretary
            };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var idRole = new IdentityRole { Name = role };
                    await roleManager.CreateAsync(idRole);
                }
            }
            var admin = Configuration.GetSection(Roles.Admin);
            var profile = new User
            {
                Email = admin["Email"],
                UserName = admin["Email"],
                FirstName = Roles.Admin,
                LastName = Roles.Admin,
                EmailConfirmed = true,
                ImagePath = "default_user_image.png",
                UserProfile = new UserProfile(),
                RegistredOn = DateTime.Now
            };
            if (await userManager.FindByEmailAsync(admin["Email"]) == null)
            {
                var idenResCreateAdmin = await userManager.CreateAsync(profile, admin["Password"]);
                if (idenResCreateAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(profile, Roles.Admin);
                    var createdUser = await userManager.FindByEmailAsync(admin["Email"]);
                    await userDatesService.AddDateEntryAsync(createdUser.Id);
                }

            }
            else if (!await userManager.IsInRoleAsync(userManager.Users.First(item => item.Email == profile.Email), Roles.Admin))
            {
                var user = userManager.Users.First(item => item.Email == profile.Email);
                await userManager.AddToRoleAsync(user, Roles.Admin);
            }
        }
    }
}
