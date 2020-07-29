using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Localization;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest
{
    public class AuthControllerTestsIdentity
    {
        public (Mock<IAuthService>, Mock<IUserService>, Mock<IMapper>, Mock<IStringLocalizer<AuthenticationErrors>>, AuthController) CreateAuthController()
        {
            Mock<IAuthService> mockAuthService = new Mock<IAuthService>();
            Mock<IUserService> mockUserService = new Mock<IUserService>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            Mock<IStringLocalizer<AuthenticationErrors>> mockStringLocalizer = new Mock<IStringLocalizer<AuthenticationErrors>>();

            AuthController AuthController = new AuthController(mockUserService.Object, null, null,
                null, null, null, null, null, null, mockMapper.Object, null, mockAuthService.Object, mockStringLocalizer.Object);
            return (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController);
        }

        //Login
        [Fact]
        public async Task TestLoginGetReturnsViewWithModel()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetAuthSchemesAsync())
                .ReturnsAsync(GetTestAuthenticationSchemes());

            //Act
            var result = await AuthController.Login(GetTestReturnUrl());

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
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetAuthSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));
            AuthController.ModelState.AddModelError("NameError", "Required");

            //Act
            var result = await AuthController.Login(GetTestLoginViewModel(), GetTestReturnUrl());

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
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetAuthSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockMapper
                .Setup(m => m.Map<LoginDto>(It.IsAny<LoginViewModel>()))
                .Returns(GetTestLoginDto());

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockStringLocalizer
                .Setup(s => s["Login-NotRegistered"])
                .Returns(GetTestError());

            //Act
            var result = await AuthController.Login(GetTestLoginViewModel(), GetTestReturnUrl());

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
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetAuthSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockMapper
                .Setup(m => m.Map<LoginDto>(It.IsAny<LoginViewModel>()))
                .Returns(GetTestLoginDto());

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(false);

            mockStringLocalizer
                .Setup(s => s["Login-NotConfirmed"])
                .Returns(GetTestError());

            //Act
            var result = await AuthController.Login(GetTestLoginViewModel(), GetTestReturnUrl());

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
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetAuthSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockMapper
                .Setup(m => m.Map<LoginDto>(It.IsAny<LoginViewModel>()))
                .Returns(GetTestLoginDto());

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(true);

            mockAuthService
                .Setup(s => s.SignInAsync(It.IsAny<LoginDto>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

            //Act
            var result = await AuthController.Login(GetTestLoginViewModel(), GetTestReturnUrl()) as RedirectToActionResult;

            //Assert
            Assert.Equal("AccountLocked", result.ActionName);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestLoginPostReturnsViewUserProfile()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetAuthSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockMapper
                .Setup(m => m.Map<LoginDto>(It.IsAny<LoginViewModel>()))
                .Returns(GetTestLoginDto());

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(true);

            mockAuthService
                .Setup(s => s.SignInAsync(It.IsAny<LoginDto>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            //Act
            var result = await AuthController.Login(GetTestLoginViewModel(), GetTestReturnUrl()) as RedirectToActionResult;

            //Assert
            Assert.Equal("UserProfile", result.ActionName);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task TestLoginPostReturnsViewPasswordInCorrect()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetAuthSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockMapper
                .Setup(m => m.Map<LoginDto>(It.IsAny<LoginViewModel>()))
                .Returns(GetTestLoginDto());

            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(true);

            mockAuthService
                .Setup(s => s.SignInAsync(It.IsAny<LoginDto>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            mockStringLocalizer
                .Setup(s => s["Login-InCorrectPassword"])
                .Returns(GetTestError());

            //Act
            var result = await AuthController.Login(GetTestLoginViewModel(), GetTestReturnUrl());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<LoginViewModel>(viewResult.ViewData.Model);
            Assert.Equal(GetTestLoginViewModel().ReturnUrl, model.ReturnUrl);
            Assert.NotNull(viewResult);
        }

        //Register
        [Fact]
        public void TestRegisterGetReturnsRegisterView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();

            //Act
            var result = AuthController.Register();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Register", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestRegisterPostModelIsNotValid()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            AuthController.ModelState.AddModelError("NameError", "Required");

            mockStringLocalizer
                .Setup(s => s["Register-InCorrectData"])
                .Returns(GetTestError());

            //Act
            var result = await AuthController.Register(GetTestRegisterViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Register", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestRegisterPostRegisterIsInSystemReturnsRegisterView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockStringLocalizer
                .Setup(s => s["Register-RegisteredUser"])
                .Returns(GetTestError());

            //Act
            var result = await AuthController.Register(GetTestRegisterViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Register", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestRegisterPostProblemWithPasswordReturnsRegisterView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockMapper
                .Setup(s => s.Map<RegisterDto>(It.IsAny<RegisterViewModel>()))
                .Returns(GetTestRegisterDto());

            mockAuthService
                .Setup(s => s.CreateUserAsync(It.IsAny<RegisterDto>()))
                .ReturnsAsync(IdentityResult.Failed(null));

            mockStringLocalizer
                .Setup(s => s["Register-InCorrectPassword"])
                .Returns(GetTestError());

            //Act
            var result = await AuthController.Register(GetTestRegisterViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Register", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestRegisterPostReturnsAcceptingEmailView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .SetupSequence(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null)
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockMapper
                .Setup(s => s.Map<RegisterViewModel, RegisterDto>(It.IsAny<RegisterViewModel>()))
                .Returns(GetTestRegisterDto());

            mockAuthService
                .Setup(s => s.CreateUserAsync(It.IsAny<RegisterDto>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            mockAuthService
                .Setup(s => s.AddRoleAndTokenAsync(It.IsAny<RegisterDto>()))
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

            AuthController.Url = mockUrlHelper.Object;
            AuthController.ControllerContext.HttpContext = new DefaultHttpContext();

            mockAuthService
                .Setup(s => s.SendEmailRegistr(It.IsAny<string>(), It.IsAny<RegisterDto>()))
                .Verifiable();

            //Act
            var result = await AuthController.Register(GetTestRegisterViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("AcceptingEmail", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        //ConfirmedEmail
        [Fact]
        public void TestConfirmEmailGetReturnsConfirmedEmailView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();

            //Act
            var result = AuthController.ConfirmedEmail();

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
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            //Act
            var result = await AuthController.ResendEmailForRegistering(GetTestIdForConfirmingEmail()) as RedirectToActionResult;

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
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.GenerateConfToken(It.IsAny<UserDTO>()))
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

            AuthController.Url = mockUrlHelper.Object;
            AuthController.ControllerContext.HttpContext = new DefaultHttpContext();

            mockAuthService
                .Setup(s => s.SendEmailRegistr(It.IsAny<string>(), It.IsAny<UserDTO>()))
                .Verifiable();

            //Act
            var result = await AuthController.ResendEmailForRegistering(GetTestIdForConfirmingEmail());

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
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            //Act
            var result = await AuthController.ConfirmingEmail(GetTestIdConfirmingEmail(), GetBadFakeCodeConfirmingEmail()) as RedirectToActionResult;

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(500, result.RouteValues["code"]);
            Assert.NotNull(viewResult);
        }

        //AccountLocked
        [Fact]
        public void TestAccountLockedGetReturnsAccountLockedView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();

            //Act
            var result = AuthController.AccountLocked();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("AccountLocked", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        //Logout
        [Fact]
        public void TestLogoutReturnsView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.SignOutAsync())
                .Verifiable();

            //Act
            var result = AuthController.Logout() as RedirectToActionResult;

            //Assert
            Assert.Equal("Login", result.ActionName);
            Assert.NotNull(result);
        }

        //ForgotPassword
        [Fact]
        public void TestForgotPasswordGetReturnsForgotPasswordView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();

            //Act
            var result = AuthController.ForgotPassword();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ForgotPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestForgotPasswordPostModelIsNotValid()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            AuthController.ModelState.AddModelError("NameError", "Required");

            //Act
            var result = await AuthController.ForgotPassword(GetTestForgotViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ForgotPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestForgotPasswordPostReturnsForgotPasswordView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields().EmailConfirmed);

            mockStringLocalizer
                .Setup(s => s["Forgot-NotRegisteredUser"])
                .Returns(GetTestError());

            //Act
            var result = await AuthController.ForgotPassword(GetTestForgotViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ForgotPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestForgotPasswordPostReturnsForgotPasswordConfirmationView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockAuthService
                .Setup(s => s.IsEmailConfirmedAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(GetTestUserWithEmailConfirmed().EmailConfirmed);

            mockAuthService
                .Setup(i => i.GenerateResetTokenAsync(It.IsAny<UserDTO>()))
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

            AuthController.Url = mockUrlHelper.Object;
            AuthController.ControllerContext.HttpContext = new DefaultHttpContext();

            mockMapper
               .Setup(s => s.Map<ForgotPasswordDto>(It.IsAny<ForgotPasswordViewModel>()))
               .Returns(GetTestForgotPasswordDto());

            mockAuthService
                .Setup(s => s.SendEmailReseting(It.IsAny<string>(), It.IsAny<ForgotPasswordDto>()))
                .Verifiable();

            //Act
            var result = await AuthController.ForgotPassword(GetTestForgotViewModel());

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
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            //Act
            var result = await AuthController.ResetPassword(GetTestIdConfirmingEmail()) as RedirectToActionResult;

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
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();

            mockAuthService
                .Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithWrongTimeFromSendedEmail());

            mockAuthService
                .Setup(s => s.GetTimeAfterReset(It.IsAny<UserDTO>()))
                .Returns(GetTestCountOfMinutesToLate());

            //Act
            var result = await AuthController.ResetPassword(GetTestCodeForResetPasswordAndConfirmEmail(), GetTestCodeForResetPasswordAndConfirmEmail());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<UserDTO>(viewResult.ViewData.Model);
            Assert.Equal("ResetPasswordNotAllowed", viewResult.ViewName);
            Assert.NotNull(model);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestResetPasswordPostModelIsNotValid()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            AuthController.ModelState.AddModelError("NameError", "Required");

            //Act
            var result = await AuthController.ResetPassword(GetTestResetViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ResetPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestResetPasswordPostReturnsResetPasswordView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDTO)null);

            mockStringLocalizer
                .Setup(s => s["Reset-NotRegisteredUser"])
                .Returns(GetTestError());

            //Act
            var result = await AuthController.ResetPassword(GetTestResetViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ResetPassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestResetPasswordPostReturnsResetPasswordConfirmation()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockMapper
               .Setup(s => s.Map<ResetPasswordDto>(It.IsAny<ResetPasswordViewModel>()))
               .Returns(GetTestResetPssswordDto());

            mockAuthService
                .Setup(s => s.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<ResetPasswordDto>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            mockAuthService
                .Setup(s => s.CheckingForLocking(It.IsAny<UserDTO>()))
                .Verifiable();

            //Act
            var result = await AuthController.ResetPassword(GetTestResetViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ResetPasswordConfirmation", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestResetPasswordPostReturnsResultFailedResetPasswordView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockMapper
               .Setup(s => s.Map<ResetPasswordDto>(It.IsAny<ResetPasswordViewModel>()))
               .Returns(GetTestResetPssswordDto());

            mockAuthService
                .Setup(s => s.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<ResetPasswordDto>()))
                .Returns(Task.FromResult(IdentityResult.Failed(null)));

            mockStringLocalizer
                .Setup(s => s["Reset-PasswordProblems"])
                .Returns(GetTestError());

            //Act
            var result = await AuthController.ResetPassword(GetTestResetViewModel());

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
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(GetTestUserDtoWithFalseSocialNetworking());

            //Act
            var result = await AuthController.ChangePassword();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestChangePasswordGetReturnsChangePasswordNotAllowedView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            //Act
            var result = await AuthController.ChangePassword();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePasswordNotAllowed", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestChangePasswordPostModelIsNotValid()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            AuthController.ModelState.AddModelError("CurrentPassword", "Required");

            //Act
            var result = await AuthController.ChangePassword(GetTestChangeViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestChangePasswordPostReturnsLoginRedirect()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, MockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(((UserDTO)null));

            //Act
            var result = await AuthController.ChangePassword(GetTestChangeViewModel());

            //Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName);
            Assert.NotNull(redirectResult);
        }

        [Fact]
        public async Task TestChangePasswordPostReturnsChangePasswordView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockMapper
               .Setup(s => s.Map<ChangePasswordDto>(It.IsAny<ChangePasswordViewModel>()))
               .Returns(GetTestChangePssswordDto());

            mockAuthService
                .Setup(s => s.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<ChangePasswordDto>()))
                .ReturnsAsync(IdentityResult.Failed(null));

            mockStringLocalizer
                .Setup(s => s["Change-PasswordProblems"])
                .Returns(GetTestError());

            //Act
            var result = await AuthController.ChangePassword(GetTestChangeViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePassword", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestChangePasswordPostReturnChangePasswordConfirmationView()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(GetTestUserDtoWithAllFields());

            mockMapper
               .Setup(s => s.Map<ChangePasswordDto>(It.IsAny<ChangePasswordViewModel>()))
               .Returns(GetTestChangePssswordDto());

            mockAuthService
                .Setup(s => s.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<ChangePasswordDto>()))
                .ReturnsAsync(IdentityResult.Success);

            mockAuthService
                .Setup(s => s.RefreshSignInAsync(It.IsAny<UserDTO>()))
                .Verifiable();

            //Act
            var result = await AuthController.ChangePassword(GetTestChangeViewModel());

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ChangePasswordConfirmation", viewResult.ViewName);
            Assert.NotNull(viewResult);
        }

        //ExternalLogin
        [Fact]
        public void TestExternalLoginReturnsChallengeResult()
        {
            //Arrange
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(
                    x => x.Action(
                        It.IsAny<UrlActionContext>()
                    )
                )
                .Returns("callbackUrl")
                .Verifiable();

            AuthController.Url = mockUrlHelper.Object;
            AuthController.ControllerContext.HttpContext = new DefaultHttpContext();

            mockAuthService
                .Setup(s => s.GetAuthProperties(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(GetAuthenticationProperties());

            //Act
            var result = AuthController.ExternalLogin(GetTestProvider(), GetTestReturnUrl());

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
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetAuthSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockStringLocalizer
                .Setup(s => s["Error-ExternalLoginProvider"])
                .Returns(GetTestError());

            //Act
            var result = await AuthController.ExternalLoginCallBack(GetTestReturnUrl(), GetTestRemoteError());

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
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetAuthSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockAuthService
                .Setup(s => s.GetInfoAsync())
                .ReturnsAsync((ExternalLoginInfo)null);

            mockStringLocalizer
                .Setup(s => s["Error-ExternalLoginInfo"])
                .Returns(GetTestError());

            //Act
            var result = await AuthController.ExternalLoginCallBack(GetTestReturnUrl());

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
            var (mockAuthService, mockUserService, mockMapper, mockStringLocalizer, AuthController) = CreateAuthController();
            mockAuthService
                .Setup(s => s.GetAuthSchemesAsync())
                .Returns(Task.FromResult<IEnumerable<AuthenticationScheme>>(GetTestAuthenticationSchemes()));

            mockAuthService
                .Setup(s => s.GetInfoAsync())
                .ReturnsAsync(GetExternalLoginInfoFake());

            mockAuthService
                .Setup(s => s.GetSignInResultAsync(It.IsAny<ExternalLoginInfo>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            //Act
            var result = await AuthController.ExternalLoginCallBack(GetTestReturnUrl()) as LocalRedirectResult;

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
                ExternalLogins = GetTestAuthenticationSchemes().ToList()
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
        private ChangePasswordDto GetTestChangePssswordDto()
        {
            var changePasswordDto = new ChangePasswordDto
            {
                CurrentPassword = "password123",
                NewPassword = "newpassword123",
                ConfirmPassword = "newpassword123"
            };
            return changePasswordDto;
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

        private LoginDto GetTestLoginDto()
        {
            var loginDto = new LoginDto
            {
                Email = "andriishainoha@gmail.com",
                Password = "andrii123",
                RememberMe = true
            };
            return loginDto;
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

        private string GetBadFakeCodeConfirmingEmail()
        {
            string code = null;
            return code;
        }

        private IEnumerable<AuthenticationScheme> GetTestAuthenticationSchemes()
        {
            AuthenticationScheme[] authenticationScheme = new AuthenticationScheme[2];
            authenticationScheme[0] = new AuthenticationScheme("GoogleExample", "Google", typeof(IAuthenticationHandler));
            authenticationScheme[1] = new AuthenticationScheme("FacebookExample", "Facebook", typeof(IAuthenticationHandler));
            return authenticationScheme;
        }

        private string GetTestIdConfirmingEmail()
        {
            string userId = null;
            return userId;
        }

        private LocalizedString GetTestError()
        {
            var localizedString = new LocalizedString("Hello", "Fake for simple unit tests");
            return localizedString;
        }

        private UserDTO GetTestUserDtoWithAllFields()
        {
            return new UserDTO()
            {
                UserName = "andriishainoha@gmail.com",
                FirstName = "Andrii",
                LastName = "Shainoha",
                EmailConfirmed = true,
                SocialNetworking = true
            };
        }

        private UserDTO GetTestUserDtoWithFalseSocialNetworking()
        {
            return new UserDTO()
            {
                SocialNetworking = false
            };
        }

        private UserDTO GetTestUserDtoWithWrongTimeFromSendedEmail()
        {
            IDateTimeHelper dateTimeResetingPassword = new DateTimeHelper();

            return new UserDTO()
            {
                EmailSendedOnForgotPassword = dateTimeResetingPassword
                    .GetCurrentTime()
                    .AddMinutes(-GetTestCountOfMinutesToLate())
            };
        }

        private int GetTestCountOfMinutesToLate()
        {
            return 185;
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
