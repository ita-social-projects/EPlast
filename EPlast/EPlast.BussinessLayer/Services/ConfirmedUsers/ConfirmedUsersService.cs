using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;

namespace EPlast.BussinessLayer.Services
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

        public void Create(ClaimsPrincipal user, string userId, bool isClubAdmin = false, bool isCityAdmin = false)
        {
            var id = _userManager.GetUserId(user);
            var conUser = new ConfirmedUser { UserID = userId, ConfirmDate = DateTime.Now, isClubAdmin = isClubAdmin, isCityAdmin = isCityAdmin };
            var appUser = new Approver { UserID = id, ConfirmedUser = conUser };
            conUser.Approver = appUser;

            _repoWrapper.ConfirmedUser.Create(conUser);
            _repoWrapper.Save();
        }
        public void Delete(int confirmedUserId)
        {
            _repoWrapper.ConfirmedUser.Delete(_repoWrapper.ConfirmedUser.FindByCondition(x => x.ID == confirmedUserId).First());
            _repoWrapper.Save();
        }
    }
}
