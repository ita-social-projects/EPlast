using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Services
{
    public class ConfirmedUsersService : IConfirmedUsersService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;

        public ConfirmedUsersService(IRepositoryWrapper repoWrapper, UserManager<User> userManager)
        {
            _repoWrapper = repoWrapper;
            _userManager = userManager;
        }

        public async Task CreateAsync(ClaimsPrincipal user, string userId, bool isClubAdmin = false, bool isCityAdmin = false)
        {
            var id = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(user));
            var conUser = new ConfirmedUser { UserID = userId, ConfirmDate = DateTime.Now, isClubAdmin = isClubAdmin, isCityAdmin = isCityAdmin };
            var appUser = new Approver { UserID = id, ConfirmedUser = conUser };
            conUser.Approver = appUser;

            await _repoWrapper.ConfirmedUser.CreateAsync(conUser);
            await _repoWrapper.SaveAsync();
        }
        public async Task DeleteAsync(int confirmedUserId)
        {
            _repoWrapper.ConfirmedUser.Delete(await _repoWrapper.ConfirmedUser.GetFirstAsync(x => x.ID == confirmedUserId));
            await _repoWrapper.SaveAsync();
        }
    }
}
