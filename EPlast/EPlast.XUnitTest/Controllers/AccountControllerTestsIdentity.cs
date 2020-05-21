using AutoMapper;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.BussinessLayer.Interfaces;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest
{
    public class AccountControllerTestsIdentity
    {
        public (Mock<IAccountService>, Mock<IUserService>, Mock<IMapper>, AccountController) CreateAccountController()
        {
            Mock<IAccountService> mockAccountService = new Mock<IAccountService>();
            Mock<IUserService> mockUserService = new Mock<IUserService>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();

            AccountController accountController = new AccountController(mockAccountService.Object, mockUserService.Object, mockMapper.Object);
            return (mockAccountService, mockUserService, mockMapper, accountController);
        }

        //Login
        [Fact]
        public async Task TestLoginGetReturnsViewWithModel()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
                .Setup(s => s.GetAuthSchemesAsync())
                .ReturnsAsync(GetTestAuthenticationSchemes());

            //Act
            var result = await accountController.Login(GetTestReturnUrl());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LoginViewModel>(viewResult.ViewData.Model);
            Assert.Equal(GetTestLoginViewModel().ReturnUrl, model.ReturnUrl);
            Assert.NotNull(viewResult);
        }

        private IEnumerable<AuthenticationScheme> GetTestAuthenticationSchemes()
        {
            AuthenticationScheme[] authenticationScheme = new AuthenticationScheme[2];
            authenticationScheme[0] = new AuthenticationScheme("GoogleExample", "Google", typeof(IAuthenticationHandler));
            authenticationScheme[1] = new AuthenticationScheme("FacebookExample", "Facebook", typeof(IAuthenticationHandler));
            return authenticationScheme;
        }

        [Fact]
        public async Task TestLoginPostModelIsNotValid()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
                .Setup(s => s.GetAuthSchemesAsync())
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
                .Setup(s => s.GetAuthSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockMapper
                .Setup(m => m.Map<LoginDto>(It.IsAny<LoginViewModel>()))
                .Returns(GetTestLoginDto());

            mockAccountService
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

        private LoginDto GetTestLoginDto()
        {
            var loginDto = new LoginDto
            {
                Email = "andriishainoha@gmail.com",
                Password = "andrii123",
                RememberMe = true,
                ReturnUrl = "/google.com/"
            };
            return loginDto;
        }

        [Fact]
        public async Task TestLoginPostEmailConfReturnsViewWithModel()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
                .Setup(s => s.GetAuthSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockMapper
                .Setup(m => m.Map<LoginDto>(It.IsAny<LoginViewModel>()))
                .Returns(GetTestLoginDto());

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockAccountService
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
                .Setup(s => s.GetAuthSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockMapper
                .Setup(m => m.Map<LoginDto>(It.IsAny<LoginViewModel>()))
                .Returns(GetTestLoginDto());

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockAccountService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            mockAccountService
                .Setup(s => s.SignInAsync(It.IsAny<LoginDto>()))
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
                .Setup(s => s.GetAuthSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockMapper
                .Setup(m => m.Map<LoginDto>(It.IsAny<LoginViewModel>()))
                .Returns(GetTestLoginDto());

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockAccountService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            mockAccountService
                .Setup(s => s.SignInAsync(It.IsAny<LoginDto>()))
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
                .Setup(s => s.GetAuthSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockMapper
                .Setup(m => m.Map<LoginDto>(It.IsAny<LoginViewModel>()))
                .Returns(GetTestLoginDto());

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockAccountService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            mockAccountService
                .Setup(s => s.SignInAsync(It.IsAny<LoginDto>()))
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();

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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            mockMapper
                .Setup(s => s.Map<RegisterDto>(It.IsAny<RegisterViewModel>()))
                .Returns(GetTestRegisterDto());

            mockAccountService
                .Setup(s => s.CreateUserAsync(It.IsAny<RegisterDto>()))
                .ReturnsAsync(IdentityResult.Failed(null));

            //Act
            var result = await accountController.Register(GetTestRegisterViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Register", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        private RegisterDto GetTestRegisterDto()
        {
            var registerDto = new RegisterDto
            {
                Email = "andriishainoha@gmail.com",
                Name = "Andrii",
                SurName = "Shainoha",
                Password = "andrii123",
                ConfirmPassword = "andrii123"
            };
            return registerDto;
        }

        private ForgotPasswordDto GetTestForgotPasswordDto()
        {
            var forgotpasswordDto = new ForgotPasswordDto
            {
                Email = "andriishainoha@gmail.com"
            };
            return forgotpasswordDto;
        }

        [Fact]
        public async Task TestRegisterPostReturnsAcceptingEmailView()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))  // тут не можу настроїти
                .ReturnsAsync((User)null);

            mockMapper
                .Setup(s => s.Map<RegisterDto>(It.IsAny<RegisterViewModel>()))
                .Returns(GetTestRegisterDto());

            mockAccountService
                .Setup(s => s.CreateUserAsync(It.IsAny<RegisterDto>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            mockAccountService
                .Setup(i => i.AddRoleAndTokenAsync(It.IsAny<RegisterDto>()))
                .ReturnsAsync(GetTestCodeForResetPasswordAndConfirmEmail());

            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
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

            mockAccountService
                .Setup(s => s.SendEmailRegistr(It.IsAny<string>(), It.IsAny<RegisterDto>()))
                .Verifiable();

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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();

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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();

            mockAccountService
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();

            mockAccountService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockAccountService
                .Setup(s => s.GenerateConfToken(It.IsAny<User>()))
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

            mockAccountService
                .Setup(s => s.SendEmailRegistr(It.IsAny<string>(), It.IsAny<User>()))
                .Verifiable();

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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();

            mockAccountService
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();

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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
                .Setup(s => s.SignOutAsync())
                .Verifiable();

            //Act
            var result = accountController.Logout() as RedirectToActionResult;

            //Assert
            Assert.Equal("Login", result.ActionName);
            Assert.NotNull(result);
        }

        //ForgotPassword
        [Fact]
        public void TestForgotPasswordGetReturnsForgotPasswordView()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();

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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            mockAccountService
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockAccountService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(GetTestUserWithEmailConfirmed().EmailConfirmed);

            mockAccountService
                .Setup(i => i.GenerateResetTokenAsync(It.IsAny<User>()))
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

            mockMapper
               .Setup(s => s.Map<ForgotPasswordDto>(It.IsAny<ForgotPasswordViewModel>()))
               .Returns(GetTestForgotPasswordDto());

            mockAccountService
                .Setup(s => s.SendEmailReseting(It.IsAny<string>(), It.IsAny<ForgotPasswordDto>()))
                .Verifiable();

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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();

            mockAccountService
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

        /*[Fact]
        public async Task TestResetPasswordGetReturnsResetPasswordNotAllowedView()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();

            mockAccountService
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
        }*/

        [Fact]
        public async Task TestResetPasswordPostModelIsNotValid()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockMapper
               .Setup(s => s.Map<ResetPasswordDto>(It.IsAny<ResetPasswordViewModel>()))
               .Returns(GetTestResetPssswordDto());

            mockAccountService
                .Setup(s => s.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<ResetPasswordDto>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            mockAccountService
                .Setup(s => s.CheckingForLocking(It.IsAny<User>()))
                .Verifiable();

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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserWithAllFields());

            mockMapper
               .Setup(s => s.Map<ResetPasswordDto>(It.IsAny<ResetPasswordViewModel>()))
               .Returns(GetTestResetPssswordDto());

            mockAccountService
                .Setup(s => s.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<ResetPasswordDto>()))
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            mockAccountService
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
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
            accountController.ModelState.AddModelError("CurrentPassword", "Required");

            //Act
            var result = await accountController.ChangePassword(GetTestChangeViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        //    [Fact]
        //    public async Task TestChangePasswordPostReturnsLoginRedirect()
        //    {
        //        //Arrange
        //        var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
        //        mockUserManager
        //            .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
        //            .Returns(Task.FromResult(GetTestUserWithNullFields()));

        //        //Act
        //        var result = await accountController.ChangePassword(GetTestChangeViewModel()) as RedirectToActionResult;

        //        //Assert
        //        Assert.Equal("Login", result.ActionName);
        //        Assert.NotNull(result);
        //    }

        //    [Fact]
        //    public async Task TestChangePasswordPostReturnsChangePasswordView()
        //    {
        //        //Arrange
        //        var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
        //        mockUserManager
        //            .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
        //            .Returns(Task.FromResult(GetTestUserWithAllFields()));

        //        mockUserManager
        //            .Setup(s => s.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
        //            .Returns(Task.FromResult(IdentityResult.Failed(null)));

        //        //Act
        //        var result = await accountController.ChangePassword(GetTestChangeViewModel());

        //        //Assert
        //        var viewResult = Assert.IsType<ViewResult>(result);
        //        Assert.Equal("ChangePassword", viewResult.ViewName);
        //        Assert.NotNull(viewResult);
        //    }

        //    [Fact]
        //    public async Task TestChangePasswordPostReturnChangePasswordConfirmationView()
        //    {
        //        //Arrange
        //        var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
        //        mockUserManager
        //            .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
        //            .Returns(Task.FromResult(GetTestUserWithAllFields()));

        //        mockUserManager
        //            .Setup(s => s.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
        //            .Returns(Task.FromResult(IdentityResult.Success));

        //        mockSignInManager
        //            .Setup(s => s.RefreshSignInAsync(It.IsAny<User>()))
        //            .Verifiable();

        //        //Act
        //        var result = await accountController.ChangePassword(GetTestChangeViewModel());

        //        //Assert
        //        var viewResult = Assert.IsType<ViewResult>(result);
        //        Assert.Equal("ChangePasswordConfirmation", viewResult.ViewName);
        //        Assert.NotNull(viewResult);
        //    }

        //ExternalLogin
        [Fact]
        public async Task TestExternalLoginReturnsChallengeResult()
        {
            //Arrange
            var (mockAccountService, mockUserService, mockMapper, accountController) = CreateAccountController();
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

            mockAccountService
                .Setup(s => s.GetAuthProperties(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(GetAuthenticationProperties());

            //Act
            var result = accountController.ExternalLogin(GetTestProvider(), GetTestReturnUrl());

            //Assert
            var challengeResult = Assert.IsType<ChallengeResult>(result);
            Assert.Equal(GetTestProvider(), challengeResult.AuthenticationSchemes[0]);
            Assert.NotNull(challengeResult);
        }

        //    //ExternalLoginCallBack
        //    [Fact]
        //    public async Task TestExternalLoginCallBackRemoteErrorNotNull()
        //    {
        //        //Arrange
        //        var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
        //        mockSignInManager
        //            .Setup(s => s.GetExternalAuthenticationSchemesAsync())
        //            .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

        //        //Act
        //        var result = await accountController.ExternalLoginCallBack(GetTestReturnUrl(), GetTestRemoteError());

        //        //Assert
        //        var viewResult = Assert.IsType<ViewResult>(result);
        //        var model = Assert.IsType<LoginViewModel>(viewResult.ViewData.Model);
        //        Assert.Equal(GetTestLoginViewModel().ReturnUrl, model.ReturnUrl);
        //        Assert.NotNull(viewResult);
        //    }

        //    [Fact]
        //    public async Task TestExternalLoginCallBackInfoNull()
        //    {
        //        //Arrange
        //        var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
        //        mockSignInManager
        //            .Setup(s => s.GetExternalAuthenticationSchemesAsync())
        //            .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

        //        mockSignInManager
        //            .Setup(s => s.GetExternalLoginInfoAsync(It.IsAny<string>()))
        //            .ReturnsAsync((ExternalLoginInfo)null);

        //        //Act
        //        var result = await accountController.ExternalLoginCallBack(GetTestReturnUrl());

        //        //Assert
        //        var viewResult = Assert.IsType<ViewResult>(result);
        //        var model = Assert.IsType<LoginViewModel>(viewResult.ViewData.Model);
        //        Assert.Equal(GetTestLoginViewModel().ReturnUrl, model.ReturnUrl);
        //        Assert.NotNull(viewResult);
        //    }

        //    [Fact]
        //    public async Task TestExternalLoginCallBackRedirectReturnUrl()
        //    {
        //        //Arrange
        //        var (mockSignInManager, mockUserManager, mockEmailConfirmation, accountController) = CreateAccountController();
        //        mockSignInManager
        //            .Setup(s => s.GetExternalAuthenticationSchemesAsync())
        //            .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

        //        mockSignInManager
        //            .Setup(s => s.GetExternalLoginInfoAsync(It.IsAny<string>()))
        //            .ReturnsAsync(GetExternalLoginInfoFake());

        //        mockSignInManager
        //            .Setup(s => s.ExternalLoginSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
        //            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        //        //Act
        //        var result = await accountController.ExternalLoginCallBack(GetTestReturnUrl()) as LocalRedirectResult;

        //        //Assert
        //        Assert.Equal(GetTestLoginViewModel().ReturnUrl, result.Url);
        //        Assert.NotNull(result);
        //    }

        //    //Fakes
        //    private string GetFakeEmail()
        //    {
        //        return new string("fakeExampleEmail");
        //    }

        //    private ExternalLoginInfo GetExternalLoginInfoFake()
        //    {
        //        var claims = new List<ClaimsIdentity>();
        //        var info = new ExternalLoginInfo(new ClaimsPrincipal(claims), "Google", "GoogleExample", "GoogleForDisplay");
        //        return info;
        //    }

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

        private ResetPasswordDto GetTestResetPssswordDto()
        {
            var resetPasswordDto = new ResetPasswordDto
            {
                Email = "andriishainoha@gmail.com",
                Password = "andrii123",
                ConfirmPassword = "andrii123"
            };
            return resetPasswordDto;
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

        //    private User GetTestUserWithNotEmailConfirmed()
        //    {
        //        return new User()
        //        {
        //            EmailConfirmed = false
        //        };
        //    }

        //    private User GetTestUserWithNullFields()
        //    {
        //        return null;
        //    }

        private AuthenticationProperties GetAuthenticationProperties()
        {
            Dictionary<string, string> authenticationDictionary = new Dictionary<string, string>(3);
            authenticationDictionary.Add("First", "Google");
            authenticationDictionary.Add("Second", "Facebook");
            authenticationDictionary.Add("Third", "Amazon");
            var authProperties = new AuthenticationProperties(authenticationDictionary);
            return authProperties;
        }

        //    private IEnumerable<AuthenticationScheme> GetTestAuthenticationSchemes()
        //    {
        //        AuthenticationScheme[] authenticationScheme = new AuthenticationScheme[2];
        //        authenticationScheme[0] = new AuthenticationScheme("GoogleExample", "Google", typeof(IAuthenticationHandler));
        //        authenticationScheme[1] = new AuthenticationScheme("FacebookExample", "Facebook", typeof(IAuthenticationHandler));
        //        return authenticationScheme;
        //    }

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

        //    private string GetTestRemoteError()
        //    {
        //        return new string("remoteErrorExample");
        //    }

        private string GetTestProvider()
        {
            return new string("fakeProvider");
        }
    }
}
