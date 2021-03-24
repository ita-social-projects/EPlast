using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Events;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using Hangfire;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

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
                                            "59 23 * * *",
                                            TimeZoneInfo.Local);
            recurringJobManager.AddOrUpdate("Remove roles from previous admins",
                                            () => serviceProvider.GetService<ICityParticipantsService>()
                                                                 .CheckPreviousAdministratorsToDelete(),
                                            "59 23 * * *",
                                            TimeZoneInfo.Local);
            recurringJobManager.AddOrUpdate("Changes status of region admins when the date expires",
                                            () => serviceProvider.GetService<IRegionService>()
                                                                 .EndAdminsDueToDate(),
                                            Cron.Daily(), TimeZoneInfo.Local);
            CreateRolesAsync(serviceProvider, Configuration).Wait();
            recurringJobManager.AddOrUpdate("Remove roles from previous admins",
                                            () => serviceProvider.GetService<IClubParticipantsService>()
                                                                 .CheckPreviousAdministratorsToDelete(),
                                            "59 23 * * *", TimeZoneInfo.Local);
            CreateRolesAsync(serviceProvider, Configuration).Wait();
            recurringJobManager.AddOrUpdate("Reminder to join city",
                                            () => serviceProvider.GetService<IEmailReminderService>()
                                                                 .JoinCityReminderAsync(),
                                            "0 12 * * Mon",
                                            TimeZoneInfo.Local);
            recurringJobManager.AddOrUpdate("New Plast members greeting",
                                            () => serviceProvider.GetService<INewPlastMemberEmailGreetingService>()
                                                                 .NotifyNewPlastMembersAsync(),
                                            Cron.Daily(),
                                            TimeZoneInfo.Local);
        }

        private static async Task CreateRolesAsync(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roles = new[]
            {
                Roles.admin,
                Roles.supporter,
                Roles.plastMember,
                Roles.plastHead,
                Roles.eventAdministrator,
                Roles.kurinHead,"" +
                                Roles.kurinSecretary,
                Roles.okrugaHead,
                Roles.okrugaSecretary,
                Roles.cityHead,
                Roles.citySecretary,
                Roles.formerPlastMember,
                Roles.registeredUser,
                Roles.interested
            };
            foreach (var role in roles)
            {
                if (!(await roleManager.RoleExistsAsync(role)))
                {
                    var idRole = new IdentityRole { Name = role };
                    await roleManager.CreateAsync(idRole);
                }
            }
            var admin = Configuration.GetSection(Roles.admin);
            var profile = new User
            {
                Email = admin["Email"],
                UserName = admin["Email"],
                FirstName = Roles.admin,
                LastName = Roles.admin,
                EmailConfirmed = true,
                ImagePath = "default_user_image.png",
                UserProfile = new UserProfile(),
                RegistredOn = DateTime.Now
            };
            if (await userManager.FindByEmailAsync(admin["Email"]) == null)
            {
                var idenResCreateAdmin = await userManager.CreateAsync(profile, admin["Password"]);
                if (idenResCreateAdmin.Succeeded)
                    await userManager.AddToRoleAsync(profile, Roles.admin);
            }
            else if (!await userManager.IsInRoleAsync(userManager.Users.First(item => item.Email == profile.Email), Roles.admin))
            {
                var user = userManager.Users.First(item => item.Email == profile.Email);
                await userManager.AddToRoleAsync(user, Roles.admin);
            }
        }
    }
}
