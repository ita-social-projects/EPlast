using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.FormerMember;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.FormerMember
{
    public class FormerMemberService : IFormerMemberService
    {
        private readonly ICityParticipantsService _cityParticipants;
        private readonly IClubParticipantsService _clubParticipants;
        private readonly IFormerMemberAdminService _formerMemberAdminService;
        private readonly IUserDatesService _userDatesService;
        private readonly UserManager<User> _userManager;
        public FormerMemberService(ICityParticipantsService cityParticipants,
                                    IClubParticipantsService clubParticipants,
                                    IFormerMemberAdminService formerMemberAdminService,
                                    IUserDatesService userDatesService,
                                    UserManager<User> userManager)
        {
            _cityParticipants = cityParticipants;
            _clubParticipants = clubParticipants;
            _formerMemberAdminService = formerMemberAdminService;
            _userDatesService = userDatesService;
            _userManager = userManager;
        }

        public async Task MakeUserFormerMeberAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            await _cityParticipants.RemoveMemberAsync(userId);
            await _clubParticipants.RemoveMemberAsync(userId);
            await _formerMemberAdminService.RemoveFromAdminRolesAsync(userId);
            await _userDatesService.AddFormerEntryDateAsync(userId);
            await _userDatesService.EndUserMembership(userId);
            await _userManager.AddToRoleAsync(user, Roles.FormerPlastMember);
        }
    }
}
