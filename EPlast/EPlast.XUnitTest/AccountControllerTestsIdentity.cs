using EPlast.BussinessLayer.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace EPlast.XUnitTest
{
    public class AccountControllerTestsIdentity
    {
        public (Mock<SignInManager<User>>, Mock<UserManager<User>>, Mock<IEmailConfirmation>, AccountController) CreateAccountController()
        {
            Mock<IUserPasswordStore<User>> userPasswordStore = new Mock<IUserPasswordStore<User>>();
            userPasswordStore.Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.FromResult(IdentityResult.Success));

            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();

            idOptions.SignIn.RequireConfirmedEmail = true;
            idOptions.Password.RequireDigit = true;
            idOptions.Password.RequiredLength = 8;
            idOptions.Password.RequireUppercase = false;
            idOptions.User.RequireUniqueEmail = true;
            idOptions.Password.RequireNonAlphanumeric = false;
            idOptions.Lockout.MaxFailedAccessAttempts = 5;
            idOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            options.Setup(o => o.Value).Returns(idOptions);
            var userValidators = new List<IUserValidator<User>>();
            UserValidator<User> validator = new UserValidator<User>();
            userValidators.Add(validator);

            var passValidator = new PasswordValidator<User>();
            var pwdValidators = new List<IPasswordValidator<User>>();
            pwdValidators.Add(passValidator);

            var userStore = new Mock<IUserStore<User>>();
            userStore.Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            var mockUserManager = new Mock<UserManager<User>>(userStore.Object,
                options.Object, new PasswordHasher<User>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<User>>>().Object);

            mockUserManager
                .Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            var _contextAccessor = new Mock<IHttpContextAccessor>();
            var _userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                           _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null);

            Mock<IRepositoryWrapper> mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            Mock<ILogger<AccountController>> mockLogger = new Mock<ILogger<AccountController>>();
            Mock<IEmailConfirmation> mockEmailConfirmation = new Mock<IEmailConfirmation>();
            Mock<IHostingEnvironment> mockHosting = new Mock<IHostingEnvironment>();

            AccountController accountController = new AccountController(mockUserManager.Object, mockSignInManager.Object,
                mockRepositoryWrapper.Object, mockLogger.Object, mockEmailConfirmation.Object, mockHosting.Object, null, null);

            return (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController);
        }

        //Login
        [Fact]
        public async Task TestLoginGetReturnsViewWithModel()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockSignInManager
                .Setup(s => s.GetExternalAuthenticationSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            //Act
            var result = await accountController.Login(GetTestReturnUrl());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LoginViewModel>(viewResult.ViewData.Model);
            Assert.Equal(GetTestLoginViewModel().ReturnUrl, model.ReturnUrl);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestLoginPostModelIsNotValid()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockSignInManager
                .Setup(s => s.GetExternalAuthenticationSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));
            accountController.ModelState.AddModelError("NameError", "Required");

            //Act
            var result = await accountController.Login(GetTestLoginViewModel(), GetTestReturnUrl());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LoginViewModel>(viewResult.ViewData.Model);
            Assert.Equal(GetTestLoginViewModel().ReturnUrl, model.ReturnUrl);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestLoginPostUserNullReturnsViewWithModel()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockSignInManager
                .Setup(s => s.GetExternalAuthenticationSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            //Act
            var result = await accountController.Login(GetTestLoginViewModel(), GetTestReturnUrl());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LoginViewModel>(viewResult.ViewData.Model);
            Assert.Equal(GetTestLoginViewModel().ReturnUrl, model.ReturnUrl);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestLoginPostEmailConfReturnsViewWithModel()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockSignInManager
                .Setup(s => s.GetExternalAuthenticationSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(false);

            //Act
            var result = await accountController.Login(GetTestLoginViewModel(), GetTestReturnUrl());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LoginViewModel>(viewResult.ViewData.Model);
            Assert.Equal(GetTestLoginViewModel().ReturnUrl, model.ReturnUrl);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestLoginPostReturnsViewAccountLocked()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockSignInManager
                .Setup(s => s.GetExternalAuthenticationSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            mockSignInManager
                .Setup(s => s.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

            //Act
            var result = await accountController.Login(GetTestLoginViewModel(), GetTestReturnUrl()) as RedirectToActionResult;

            //Assert
            Assert.Equal("AccountLocked", result.ActionName);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestLoginPostReturnsViewUserProfile()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockSignInManager
                .Setup(s => s.GetExternalAuthenticationSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            mockSignInManager
                .Setup(s => s.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            //Act
            var result = await accountController.Login(GetTestLoginViewModel(), GetTestReturnUrl()) as RedirectToActionResult;

            //Assert
            Assert.Equal("UserProfile", result.ActionName);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestLoginPostReturnsViewPasswordInCorrect()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockSignInManager
                .Setup(s => s.GetExternalAuthenticationSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            mockSignInManager
                .Setup(s => s.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            //Act
            var result = await accountController.Login(GetTestLoginViewModel(), GetTestReturnUrl());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LoginViewModel>(viewResult.ViewData.Model);
            Assert.Equal(GetTestLoginViewModel().ReturnUrl, model.ReturnUrl);
            Assert.NotNull(viewResult);
        }

        //Register
        [Fact]
        public async Task TestRegisterGetReturnsRegisterView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();

            //Act
            var result = accountController.Register();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Register", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestRegisterPostModelIsNotValid()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            accountController.ModelState.AddModelError("NameError", "Required");

            //Act
            var result = await accountController.Register(GetTestRegisterViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Register", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestRegisterPostRegisterIsInSystemReturnsRegisterView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            //Act
            var result = await accountController.Register(GetTestRegisterViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Register", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestRegisterPostProblemWithPasswordReturnsRegisterView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            mockUserManager
                .Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed(null)));

            //Act
            var result = await accountController.Register(GetTestRegisterViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Register", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestRegisterPostReturnsAcceptingEmailView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            mockUserManager
                .Setup(s => s.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            mockUserManager
                .Setup(i => i.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestCodeForResetPasswordAndConfirmEmail());

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(
                    x => x.Action(
                        It.IsAny<UrlActionContext>()
                    )
                )
                .Returns("callbackUrl")
                .Verifiable();

            accountController.Url = mockUrlHelper.Object;
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();

            //Act
            var result = await accountController.Register(GetTestRegisterViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("AcceptingEmail", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        //ConfirmedEmail
        [Fact]
        public async Task TestConfirmEmailGetReturnsConfirmedEmailView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();

            //Act
            var result = accountController.ConfirmedEmail();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ConfirmedEmail", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        //ResendEmailForRegistering
        [Fact]
        public async Task TestResendEmailForRegisteringUserIsNull()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();

            mockUserManager
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            //Act
            var result = await accountController.ResendEmailForRegistering(GetTestIdForConfirmingEmail()) as RedirectToActionResult;

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(500, result.RouteValues["code"]);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestResendEmailForRegisteringReturnsViewConfirmation()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();

            mockUserManager
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(
                    x => x.Action(
                        It.IsAny<UrlActionContext>()
                    )
                )
                .Returns("callbackUrl")
                .Verifiable();

            accountController.Url = mockUrlHelper.Object;
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();

            mockUserManager
                .Setup(s => s.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            //Act
            var result = await accountController.ResendEmailForRegistering(GetTestIdForConfirmingEmail());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ResendEmailConfirmation", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        //ConfirmingEmail
        [Fact]
        public async Task TestConfirmEmailPostUserNull()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();

            mockUserManager
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            //Act
            var result = await accountController.ConfirmingEmail(GetTestIdConfirmingEmail(), GetBadFakeCodeConfirmingEmail()) as RedirectToActionResult;

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(500, result.RouteValues["code"]);
            Assert.NotNull(viewResult);
        }

        //AccountLocked
        [Fact]
        public async Task TestAccountLockedGetReturnsAccountLockedView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();

            //Act
            var result = accountController.AccountLocked();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("AccountLocked", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        //Logout
        [Fact]
        public async Task TestLogoutReturnsView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockSignInManager
                .Setup(s => s.SignOutAsync())
                .Returns(Task.FromResult(default(object)));

            //Act
            var result = await accountController.Logout() as RedirectToActionResult;

            //Assert
            Assert.Equal("Login", result.ActionName);
            Assert.NotNull(result);
        }

        //ForgotPassword
        [Fact]
        public void TestForgotPasswordGetReturnsForgotPasswordView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();

            //Act
            var result = accountController.ForgotPassword();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ForgotPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestForgotPasswordPostModelIsNotValid()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            accountController.ModelState.AddModelError("NameError", "Required");

            //Act
            var result = await accountController.ForgotPassword(GetTestForgotViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ForgotPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestForgotPasswordPostReturnsForgotPasswordView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            mockUserManager
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestUserWithAllFields().EmailConfirmed);

            //Act
            var result = await accountController.ForgotPassword(GetTestForgotViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ForgotPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestForgotPasswordPostReturnsForgotPasswordConfirmationView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestUserWithEmailConfirmed().EmailConfirmed);

            mockUserManager
                .Setup(i => i.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestCodeForResetPasswordAndConfirmEmail());

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(
                    x => x.Action(
                        It.IsAny<UrlActionContext>()
                    )
                )
                .Returns("callbackUrl")
                .Verifiable();

            accountController.Url = mockUrlHelper.Object;
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();

            //Act
            var result = await accountController.ForgotPassword(GetTestForgotViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ForgotPasswordConfirmation", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        //ResetPassword
        [Fact]
        public async Task TestResetPasswordGetReturnsHandleError()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();

            mockUserManager
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            //Act
            var result = await accountController.ResetPassword(GetTestIdConfirmingEmail()) as RedirectToActionResult;

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(500, result.RouteValues["code"]);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestResetPasswordGetReturnsResetPasswordNotAllowedView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();

            mockUserManager
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            //Act
            var result = await accountController.ResetPassword(GetTestCodeForResetPasswordAndConfirmEmail(), GetTestCodeForResetPasswordAndConfirmEmail());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<User>(viewResult.ViewData.Model);
            Assert.Equal("ResetPasswordNotAllowed", viewResult.ViewName);
            Assert.NotNull(model);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestResetPasswordPostModelIsNotValid()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            accountController.ModelState.AddModelError("NameError", "Required");

            //Act
            var result = await accountController.ResetPassword(GetTestResetViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ResetPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestResetPasswordPostReturnsResetPasswordView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            //Act
            var result = await accountController.ResetPassword(GetTestResetViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ResetPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestResetPasswordPostReturnsResetPasswordConfirmation()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
                .Setup(s => s.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            mockUserManager
                .Setup(s => s.IsLockedOutAsync(It.IsAny<User>()))
                .Returns(Task.FromResult(false));

            //Act
            var result = await accountController.ResetPassword(GetTestResetViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ResetPasswordConfirmation", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestResetPasswordPostReturnsResultFailedResetPasswordView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockUserManager
                .Setup(s => s.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed(null)));

            //Act
            var result = await accountController.ResetPassword(GetTestResetViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ResetPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        //ChangePassword
        [Fact]
        public async Task TestChangePasswordGetReturnsChangePasswordView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(GetTestUserWithEmailConfirmed());

            //Act
            var result = await accountController.ChangePassword();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestChangePasswordGetReturnsChangePasswordNotAllowedView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            //Act
            var result = await accountController.ChangePassword();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePasswordNotAllowed", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestChangePasswordPostModelIsNotValid()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            accountController.ModelState.AddModelError("CurrentPassword", "Required");

            //Act
            var result = await accountController.ChangePassword(GetTestChangeViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestChangePasswordPostReturnsLoginRedirect()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(GetTestUserWithNullFields()));

            //Act
            var result = await accountController.ChangePassword(GetTestChangeViewModel()) as RedirectToActionResult;

            //Assert
            Assert.Equal("Login", result.ActionName);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestChangePasswordPostReturnsChangePasswordView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(GetTestUserWithAllFields()));

            mockUserManager
                .Setup(s => s.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed(null)));

            //Act
            var result = await accountController.ChangePassword(GetTestChangeViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestChangePasswordPostReturnChangePasswordConfirmationView()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockUserManager
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(GetTestUserWithAllFields()));

            mockUserManager
                .Setup(s => s.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            mockSignInManager
                .Setup(s => s.RefreshSignInAsync(It.IsAny<User>()))
                .Verifiable();

            //Act
            var result = await accountController.ChangePassword(GetTestChangeViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePasswordConfirmation", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        //ExternalLogin
        [Fact]
        public async Task TestExternalLoginReturnsChallengeResult()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(
                    x => x.Action(
                        It.IsAny<UrlActionContext>()
                    )
                )
                .Returns("callbackUrl")
                .Verifiable();

            accountController.Url = mockUrlHelper.Object;
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();

            mockSignInManager
                .Setup(s => s.ConfigureExternalAuthenticationProperties(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(GetAuthenticationProperties());

            //Act
            var result = accountController.ExternalLogin(GetTestProvider(), GetTestReturnUrl());

            //Assert
            var challengeResult = Assert.IsType<ChallengeResult>(result);
            Assert.Equal(GetTestProvider(), challengeResult.AuthenticationSchemes[0]);
            Assert.NotNull(challengeResult);
        }

        //ExternalLoginCallBack
        [Fact]
        public async Task TestExternalLoginCallBackRemoteErrorNotNull()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockSignInManager
                .Setup(s => s.GetExternalAuthenticationSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            //Act
            var result = await accountController.ExternalLoginCallBack(GetTestReturnUrl(), GetTestRemoteError());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LoginViewModel>(viewResult.ViewData.Model);
            Assert.Equal(GetTestLoginViewModel().ReturnUrl, model.ReturnUrl);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestExternalLoginCallBackInfoNull()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockSignInManager
                .Setup(s => s.GetExternalAuthenticationSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockSignInManager
                .Setup(s => s.GetExternalLoginInfoAsync(It.IsAny<string>()))
                .ReturnsAsync((ExternalLoginInfo)null);

            //Act
            var result = await accountController.ExternalLoginCallBack(GetTestReturnUrl());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LoginViewModel>(viewResult.ViewData.Model);
            Assert.Equal(GetTestLoginViewModel().ReturnUrl, model.ReturnUrl);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestExternalLoginCallBackRedirectReturnUrl()
        {
            //Arrange
            var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
            mockSignInManager
                .Setup(s => s.GetExternalAuthenticationSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockSignInManager
                .Setup(s => s.GetExternalLoginInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(GetExternalLoginInfoFake());

            mockSignInManager
                .Setup(s => s.ExternalLoginSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            //Act
            var result = await accountController.ExternalLoginCallBack(GetTestReturnUrl()) as LocalRedirectResult;

            //Assert
            Assert.Equal(GetTestLoginViewModel().ReturnUrl, result.Url);
            Assert.NotNull(result);
        }

        //Fakes
        private string GetFakeEmail()
        {
            return new string("fakeExampleEmail");
        }

        private ExternalLoginInfo GetExternalLoginInfoFake()
        {
            var claims = new List<ClaimsIdentity>();
            var info = new ExternalLoginInfo(new ClaimsPrincipal(claims), "Google", "GoogleExample", "GoogleForDisplay");
            return info;
        }

        private LoginViewModel GetTestLoginViewModel()
        {
            var loginViewModel = new LoginViewModel
            {
                Email = "andriishainoha@gmail.com",
                Password = "andrii123",
                RememberMe = true,
                ReturnUrl = "/google.com/",
                ExternalLogins = (GetTestAuthenticationSchemes()).ToList()
            };
            return loginViewModel;
        }

        private RegisterViewModel GetTestRegisterViewModel()
        {
            var registerViewModel = new RegisterViewModel
            {
                Email = "andriishainoha@gmail.com",
                Name = "Andrii",
                SurName = "Shainoha",
                Password = "andrii123",
                ConfirmPassword = "andrii123"
            };
            return registerViewModel;
        }

        private ForgotPasswordViewModel GetTestForgotViewModel()
        {
            var forgotPasswordViewModel = new ForgotPasswordViewModel
            {
                Email = "andriishainoha@gmail.com"
            };
            return forgotPasswordViewModel;
        }

        private ResetPasswordViewModel GetTestResetViewModel()
        {
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                Email = "andriishainoha@gmail.com",
                Password = "andrii123",
                ConfirmPassword = "andrii123",
                Code = "500"
            };
            return resetPasswordViewModel;
        }

        private ChangePasswordViewModel GetTestChangeViewModel()
        {
            var changePasswordViewModel = new ChangePasswordViewModel
            {
                CurrentPassword = "password123",
                NewPassword = "newpassword123",
                ConfirmPassword = "newpassword123"
            };
            return changePasswordViewModel;
        }

        private User GetTestUserWithAllFields()
        {
            return new User()
            {
                UserName = "andriishainoha@gmail.com",
                FirstName = "Andrii",
                LastName = "Shainoha",
                EmailConfirmed = true,
                SocialNetworking = true
            };
        }

        private User GetTestUserWithEmailConfirmed()
        {
            return new User()
            {
                EmailConfirmed = true
            };
        }

        private User GetTestUserWithNotEmailConfirmed()
        {
            return new User()
            {
                EmailConfirmed = false
            };
        }

        private User GetTestUserWithNullFields()
        {
            return null;
        }

        private AuthenticationProperties GetAuthenticationProperties()
        {
            Dictionary<string, string> authenticationDictionary = new Dictionary<string, string>(3);
            authenticationDictionary.Add("First", "Google");
            authenticationDictionary.Add("Second", "Facebook");
            authenticationDictionary.Add("Third", "Amazon");
            var authProperties = new AuthenticationProperties(authenticationDictionary);
            return authProperties;
        }

        private IEnumerable<AuthenticationScheme> GetTestAuthenticationSchemes()
        {
            AuthenticationScheme[] authenticationScheme = new AuthenticationScheme[2];
            authenticationScheme[0] = new AuthenticationScheme("GoogleExample", "Google", typeof(IAuthenticationHandler));
            authenticationScheme[1] = new AuthenticationScheme("FacebookExample", "Facebook", typeof(IAuthenticationHandler));
            return authenticationScheme;
        }

        private string GetBadFakeCodeConfirmingEmail()
        {
            string code = null;
            return code;
        }

        private string GetTestIdConfirmingEmail()
        {
            string userId = null;
            return userId;
        }

        private string GetTestCodeForResetPasswordAndConfirmEmail()
        {
            return new string("500");
        }

        private string GetTestIdForConfirmingEmail()
        {
            return new string("asadasd3430234-2342");
        }

        private string GetTestReturnUrl()
        {
            return new string("/google.com/");
        }

        private string GetTestRemoteError()
        {
            return new string("remoteErrorExample");
        }

        private string GetTestProvider()
        {
            return new string("fakeProvider");
        }
    }
}