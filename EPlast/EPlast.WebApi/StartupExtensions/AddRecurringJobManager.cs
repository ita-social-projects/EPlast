using System;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPlast.WebApi.StartupExtensions
{
    public static class AddRecurringJobManager
    {
        public static void AddRecurringJobs(this IServiceProvider serviceProvider,
                                                 IRecurringJobManager recurringJobManager,
                                                 IConfiguration Configuration)
        {
            recurringJobManager.AddOrUpdate("Run every day",
                                            () => serviceProvider.GetService<IPlastDegreeService>()
                                                                 .GetDergeesAsync(),
                                            "59 23 * * *",
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
                if (!(await roleManager.RoleExistsAsync(role)))
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
