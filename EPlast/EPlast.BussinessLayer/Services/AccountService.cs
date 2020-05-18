using EPlast.BussinessLayer.AccessManagers.Interfaces;
using EPlast.BussinessLayer.Interfaces;
using EPlast.BussinessLayer.ViewModels.Account;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPlast.BussinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ILogger _logger;
        private readonly IEmailConfirmation _emailConfirmation;
        private readonly IHostingEnvironment _env;
        private readonly IUserAccessManager _userAccessManager;

        public AccountService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IRepositoryWrapper repoWrapper,
            ILogger<AccountService> logger,
            IEmailConfirmation emailConfirmation,
            IHostingEnvironment env,
            IUserAccessManager userAccessManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repoWrapper = repoWrapper;
            _logger = logger;
            _emailConfirmation = emailConfirmation;
            _env = env;
            _userAccessManager = userAccessManager;
        }

        public async LoginViewModel LoginAsyncGet(string returnUrl)
        {
            LoginViewModel loginmVM = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return loginmVM;
        }

    }
}
