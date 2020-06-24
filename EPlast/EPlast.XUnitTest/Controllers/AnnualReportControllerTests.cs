using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.AnnualReport;
using CityDTOs = EPlast.BusinessLogicLayer.DTO.City;
using UserDTOs = EPlast.BusinessLogicLayer.DTO.UserProfiles;
using EPlast.BusinessLogicLayer.Interfaces.City;
using EPlast.BusinessLogicLayer.Services.Interfaces;
using EPlast.Controllers;
using EPlast.Models.Enums;
using EPlast.ViewModels.AnnualReport;
using CityVMs = EPlast.ViewModels.City;
using UserVMs = EPlast.ViewModels.UserInformation.UserProfile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BusinessLogicLayer.Interfaces.Logging;
using Xunit;

namespace EPlast.XUnitTest
{
    public class AnnualReportControllerTests
    {
        private readonly Mock<ILoggerService<AnnualReportController>> _logger = new Mock<ILoggerService<AnnualReportController>>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IAnnualReportService> _annualReportService = new Mock<IAnnualReportService>();
        private readonly Mock<ICityAccessService> _cityAccessService = new Mock<ICityAccessService>();
        private readonly Mock<ICityMembersService> _cityMembersService = new Mock<ICityMembersService>();
        private readonly Mock<ICityService> _cityService = new Mock<ICityService>();
        private readonly AnnualReportController controller;

        public AnnualReportControllerTests()
        {
            controller = new AnnualReportController(_logger.Object, _mapper.Object, _annualReportService.Object, _cityAccessService.Object,
                _cityMembersService.Object, _cityService.Object);
        }

        [Fact]
        public async Task CreateAsyncCorrect()
        {
            // Arrange
            IEnumerable<CityDTOs.CityDTO> cities = new List<CityDTOs.CityDTO>
            {
                new CityDTOs.CityDTO { ID = 1, Name = "Львів" }
            };
            var city = new CityVMs.CityViewModel { ID = cities.First().ID, Name = cities.First().Name };
            IEnumerable<CityDTOs.CityMembersDTO> cityMembersDTOs = new List<CityDTOs.CityMembersDTO>
            {
                new CityDTOs.CityMembersDTO { UserId = "1", User = new UserDTOs.UserDTO { FirstName = "Петро", LastName = "Петренко" } }
            };
            var cityMembers = new List<CityVMs.CityMembersViewModel>
            {
                new CityVMs.CityMembersViewModel
                {
                    UserId = cityMembersDTOs.First().UserId,
                    User = new UserVMs.UserViewModel { FirstName = cityMembersDTOs.First().User.FirstName, LastName = cityMembersDTOs.First().User.LastName }
                }
            };
            var expectedViewModel = new CreateEditAnnualReportViewModel(cityMembers)
            {
                Operation = AnnualReportOperation.Creating,
                CityName = city.Name,
                CityMembers = new List<SelectListItem>
                {
                    new SelectListItem { Text = "" },
                    new SelectListItem { Value = cityMembers.First().UserId, Text = $"{cityMembers.First().User.FirstName} {cityMembers.First().User.LastName}" }
                },
                CityLegalStatusTypes = new List<SelectListItem>
                {
                    new SelectListItem { Value = "RegisteredLegalEntity", Text = "Зареєстрована юридична особа" },
                    new SelectListItem { Value = "LegalizedByMessage", Text = "Легалізована шляхом повідомлення" },
                    new SelectListItem { Value = "NotLegalizedInByLocalAuthorities", Text = "Нелегалізована у місцевих органах влади" },
                    new SelectListItem { Value = "InTheProcessOfLegalization", Text = "В процесі легалізації/реєстрації" },
                    new SelectListItem { Value = "RegisteredSeparatedSubdivision", Text = "Зареєстрований відокремлений підрозділ" }
                },
                AnnualReport = new AnnualReportViewModel { CityId = city.ID, MembersStatistic = new MembersStatisticViewModel() }
            };
            _cityAccessService.Setup(cas => cas.GetCitiesAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(cities);
            _mapper.Setup(m => m.Map<CityDTOs.CityDTO, CityVMs.CityViewModel>(It.IsAny<CityDTOs.CityDTO>()))
                .Returns(city);
            _cityMembersService.Setup(c => c.GetCurrentByCityIdAsync(It.IsAny<int>()))
                .ReturnsAsync(cityMembersDTOs);
            _mapper.Setup(m => m.Map<IEnumerable<CityDTOs.CityMembersDTO>, IEnumerable<CityVMs.CityMembersViewModel>>(It.IsAny<IEnumerable<CityDTOs.CityMembersDTO>>()))
                .Returns(cityMembers);

            // Act
            var result = await controller.CreateAsync();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAsync", viewResult.ViewName);
            Assert.Null(viewResult.ViewData["ErrorMessage"]);
            var actualViewModel = Assert.IsType<CreateEditAnnualReportViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel), JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public async Task CreateAsyncHasCreatedOrUnconfirmed()
        {
            // Arrange
            IEnumerable<CityDTOs.CityDTO> cities = new List<CityDTOs.CityDTO>
            {
                new CityDTOs.CityDTO { ID = 1, Name = "Львів" }
            };
            var city = new CityVMs.CityViewModel { ID = cities.First().ID, Name = cities.First().Name };
            _cityAccessService.Setup(cas => cas.GetCitiesAsync(It.IsAny<ClaimsPrincipal>()))
               .ReturnsAsync(cities);
            _mapper.Setup(m => m.Map<CityDTOs.CityDTO, CityVMs.CityViewModel>(It.IsAny<CityDTOs.CityDTO>()))
                .Returns(city);
            _annualReportService.Setup(a => a.CheckCanBeCreatedAsync(It.IsAny<int>()))
                .Throws(new InvalidOperationException("Станиця має непідтверджені звіти!"));

            // Act
            var result = await controller.CreateAsync();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAsync", viewResult.ViewName);
            Assert.Equal("Станиця має непідтверджені звіти!", viewResult.ViewData["ErrorMessage"]);
        }

        [Fact]
        public async Task CreateAsyncError()
        {
            // Arrange
            _cityAccessService.Setup(cas => cas.GetCitiesAsync(It.IsAny<ClaimsPrincipal>()))
               .ReturnsAsync(default(IEnumerable<CityDTOs.CityDTO>));

            // Act
            var result = await controller.CreateAsync();

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(StatusCodes.Status500InternalServerError, viewResult.RouteValues["code"]);
            _mapper.Verify(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsAdminAsyncCorrect()
        {
            // Arrange
            var cityDTO = new CityDTOs.CityDTO { ID = 1, Name = "Львів" };
            var city = new CityVMs.CityViewModel { ID = cityDTO.ID, Name = cityDTO.Name };
            IEnumerable<CityDTOs.CityMembersDTO> cityMembersDTOs = new List<CityDTOs.CityMembersDTO>
            {
                new CityDTOs.CityMembersDTO { UserId = "1", User = new UserDTOs.UserDTO { FirstName = "Петро", LastName = "Петренко" } }
            };
            var cityMembers = new List<CityVMs.CityMembersViewModel>
            {
                new CityVMs.CityMembersViewModel
                {
                    UserId = cityMembersDTOs.First().UserId,
                    User = new UserVMs.UserViewModel { FirstName = cityMembersDTOs.First().User.FirstName, LastName = cityMembersDTOs.First().User.LastName }
                }
            };
            var expectedViewModel = new CreateEditAnnualReportViewModel(cityMembers)
            {
                Operation = AnnualReportOperation.Creating,
                CityName = city.Name,
                CityMembers = new List<SelectListItem>
                {
                    new SelectListItem { Text = "" },
                    new SelectListItem { Value = cityMembers.First().UserId, Text = $"{cityMembers.First().User.FirstName} {cityMembers.First().User.LastName}" }
                },
                CityLegalStatusTypes = new List<SelectListItem>
                {
                    new SelectListItem { Value = "RegisteredLegalEntity", Text = "Зареєстрована юридична особа" },
                    new SelectListItem { Value = "LegalizedByMessage", Text = "Легалізована шляхом повідомлення" },
                    new SelectListItem { Value = "NotLegalizedInByLocalAuthorities", Text = "Нелегалізована у місцевих органах влади" },
                    new SelectListItem { Value = "InTheProcessOfLegalization", Text = "В процесі легалізації/реєстрації" },
                    new SelectListItem { Value = "RegisteredSeparatedSubdivision", Text = "Зареєстрований відокремлений підрозділ" }
                },
                AnnualReport = new AnnualReportViewModel { CityId = city.ID, MembersStatistic = new MembersStatisticViewModel() }
            };
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _cityService.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(cityDTO);
            _mapper.Setup(m => m.Map<CityDTOs.CityDTO, CityVMs.CityViewModel>(It.IsAny<CityDTOs.CityDTO>()))
                .Returns(city);
            _cityMembersService.Setup(c => c.GetCurrentByCityIdAsync(It.IsAny<int>()))
                .ReturnsAsync(cityMembersDTOs);
            _mapper.Setup(m => m.Map<IEnumerable<CityDTOs.CityMembersDTO>, IEnumerable<CityVMs.CityMembersViewModel>>(It.IsAny<IEnumerable<CityDTOs.CityMembersDTO>>()))
                .Returns(cityMembers);

            // Act
            var result = await controller.CreateAsAdminAsync(city.ID);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAsync", viewResult.ViewName);
            Assert.Null(viewResult.ViewData["ErrorMessage"]);
            var actualViewModel = Assert.IsType<CreateEditAnnualReportViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel), JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public async Task CreateAsAdminAsyncHasCreatedOrUnconfirmed()
        {
            // Arrange
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _annualReportService.Setup(a => a.CheckCanBeCreatedAsync(It.IsAny<int>()))
                .Throws(new InvalidOperationException("Станиця має непідтверджені звіти!"));

            // Act
            var result = await controller.CreateAsAdminAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAsync", viewResult.ViewName);
            Assert.Equal("Станиця має непідтверджені звіти!", viewResult.ViewData["ErrorMessage"]);
        }

        [Fact]
        public async Task CreateAsAdminAsyncError()
        {
            // Arrange
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            _cityService.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(default(CityDTOs.CityDTO));
            _mapper.Setup(m => m.Map<CityDTOs.CityDTO, CityVMs.CityViewModel>(It.IsAny<CityDTOs.CityDTO>()))
                .Returns(default(CityVMs.CityViewModel));

            // Act
            var result = await controller.CreateAsAdminAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(StatusCodes.Status500InternalServerError, viewResult.RouteValues["code"]);
            _mapper.Verify(m => m.Map<CityDTOs.CityDTO, CityVMs.CityViewModel>(It.IsAny<CityDTOs.CityDTO>()));
            _cityMembersService.Verify(c => c.GetCurrentByCityIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsAdminAsyncErrorNoAccess()
        {
            // Arrange
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var result = await controller.CreateAsAdminAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(StatusCodes.Status500InternalServerError, viewResult.RouteValues["code"]);
            _cityAccessService.Verify(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()));
        }

        [Fact]
        public async Task CreateAsyncPostCorrect()
        {
            // Arrange
            _mapper.Setup(m => m.Map<AnnualReportViewModel, AnnualReportDTO>(It.IsAny<AnnualReportViewModel>()))
                .Returns(default(AnnualReportDTO));

            // Act
            var result = await controller.CreateAsync(It.IsAny<AnnualReportViewModel>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAsync", viewResult.ViewName);
            Assert.Equal("Річний звіт станиці успішно створено!", viewResult.ViewData["Message"]);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public async Task CreateAsyncPostInvalid()
        {
            // Arrange
            var cityDTO = new CityDTOs.CityDTO { ID = 1, Name = "Львів" };
            var city = new CityVMs.CityViewModel { ID = cityDTO.ID, Name = cityDTO.Name };
            IEnumerable<CityDTOs.CityMembersDTO> cityMembersDTOs = new List<CityDTOs.CityMembersDTO>
            {
                new CityDTOs.CityMembersDTO { UserId = "1", User = new UserDTOs.UserDTO { FirstName = "Петро", LastName = "Петренко" } }
            };
            var cityMembers = new List<CityVMs.CityMembersViewModel>
            {
                new CityVMs.CityMembersViewModel
                {
                    UserId = cityMembersDTOs.First().UserId,
                    User = new UserVMs.UserViewModel { FirstName = cityMembersDTOs.First().User.FirstName, LastName = cityMembersDTOs.First().User.LastName }
                }
            };
            var expectedViewModel = new CreateEditAnnualReportViewModel(cityMembers)
            {
                Operation = AnnualReportOperation.Creating,
                CityName = city.Name,
                CityMembers = new List<SelectListItem>
                {
                    new SelectListItem { Text = "" },
                    new SelectListItem { Value = cityMembers.First().UserId, Text = $"{cityMembers.First().User.FirstName} {cityMembers.First().User.LastName}" }
                },
                CityLegalStatusTypes = new List<SelectListItem>
                {
                    new SelectListItem { Value = "RegisteredLegalEntity", Text = "Зареєстрована юридична особа" },
                    new SelectListItem { Value = "LegalizedByMessage", Text = "Легалізована шляхом повідомлення" },
                    new SelectListItem { Value = "NotLegalizedInByLocalAuthorities", Text = "Нелегалізована у місцевих органах влади" },
                    new SelectListItem { Value = "InTheProcessOfLegalization", Text = "В процесі легалізації/реєстрації" },
                    new SelectListItem { Value = "RegisteredSeparatedSubdivision", Text = "Зареєстрований відокремлений підрозділ" }
                },
                AnnualReport = new AnnualReportViewModel { CityId = city.ID, MembersStatistic = new MembersStatisticViewModel() }
            };
            controller.ModelState.AddModelError(string.Empty, string.Empty);
            _cityService.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(cityDTO);
            _mapper.Setup(m => m.Map<CityDTOs.CityDTO, CityVMs.CityViewModel>(It.IsAny<CityDTOs.CityDTO>()))
                .Returns(city);
            _cityMembersService.Setup(c => c.GetCurrentByCityIdAsync(It.IsAny<int>()))
                .ReturnsAsync(cityMembersDTOs);
            _mapper.Setup(m => m.Map<IEnumerable<CityDTOs.CityMembersDTO>, IEnumerable<CityVMs.CityMembersViewModel>>(It.IsAny<IEnumerable<CityDTOs.CityMembersDTO>>()))
                .Returns(cityMembers);

            // Act
            var result = await controller.CreateAsync(expectedViewModel.AnnualReport);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAsync", viewResult.ViewName);
            Assert.Equal("Річний звіт заповнений некоректно!", viewResult.ViewData["ErrorMessage"]);
            var actualViewModel = Assert.IsType<CreateEditAnnualReportViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel), JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public async Task CreateAsyncPostInvalidOperationException()
        {
            // Arrange
            _mapper.Setup(m => m.Map<AnnualReportViewModel, AnnualReportDTO>(It.IsAny<AnnualReportViewModel>()))
                .Returns(default(AnnualReportDTO));
            _annualReportService.Setup(a => a.CreateAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<AnnualReportDTO>()))
                .Throws(new InvalidOperationException(string.Empty));

            // Act
            var result = await controller.CreateAsync(It.IsAny<AnnualReportViewModel>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAsync", viewResult.ViewName);
            Assert.Null(viewResult.Model);
            Assert.Equal(string.Empty, viewResult.ViewData["ErrorMessage"]);
        }

        [Fact]
        public async Task CreateAsyncPostUnauthorizedAccessException()
        {
            // Arrange
            _mapper.Setup(m => m.Map<AnnualReportViewModel, AnnualReportDTO>(It.IsAny<AnnualReportViewModel>()))
                .Returns(default(AnnualReportDTO));
            _annualReportService.Setup(a => a.CreateAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<AnnualReportDTO>()))
                .Throws(new UnauthorizedAccessException(string.Empty));

            // Act
            var result = await controller.CreateAsync(It.IsAny<AnnualReportViewModel>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAsync", viewResult.ViewName);
            Assert.Null(viewResult.Model);
            Assert.Equal(string.Empty, viewResult.ViewData["ErrorMessage"]);
        }

        [Fact]
        public async Task CreateAsyncPostError()
        {
            // Arrange
            controller.ModelState.AddModelError(string.Empty, string.Empty);
            _cityService.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(default(CityDTOs.CityDTO));
            _mapper.Setup(m => m.Map<CityDTOs.CityDTO, CityVMs.CityViewModel>(It.IsAny<CityDTOs.CityDTO>()))
                .Returns(default(CityVMs.CityViewModel));

            // Act
            var result = await controller.CreateAsync(new AnnualReportViewModel());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(StatusCodes.Status500InternalServerError, viewResult.RouteValues["code"]);
        }

        [Fact]
        public async Task GetAllAsyncCorrect()
        {
            // Arrange
            var cities = new List<CityViewModel>
            {
                new CityViewModel { ID = 1, Name = "Львів" }
            };
            var citiesVMs = new List<CityVMs.CityViewModel>
            {
                new CityVMs.CityViewModel { ID = 1, Name = "Львів" }
            };
            var annualReports = new List<AnnualReportViewModel>
            {
                new AnnualReportViewModel { ID = 1, CityId = cities.First().ID, City = cities.First(), UserId = "1", Date = DateTime.Now }
            };
            var expectedViewModel = new ViewAnnualReportsViewModel(citiesVMs)
            {
                Cities = new List<SelectListItem>
                {
                    new SelectListItem { Value = cities.First().ID.ToString(), Text = cities.First().Name }
                },
                AnnualReports = annualReports
            };
            _mapper.Setup(m => m.Map<IEnumerable<CityDTOs.CityDTO>, IEnumerable<CityVMs.CityViewModel>>(It.IsAny<IEnumerable<CityDTOs.CityDTO>>()))
                .Returns(citiesVMs);
            _mapper.Setup(m => m.Map<IEnumerable<AnnualReportDTO>, IEnumerable<AnnualReportViewModel>>(It.IsAny<IEnumerable<AnnualReportDTO>>()))
                .Returns(annualReports);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualViewModel = Assert.IsType<ViewAnnualReportsViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel), JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public async Task GetAllAsyncError()
        {
            // Arrange
            _cityAccessService.Setup(c => c.GetCitiesAsync(It.IsAny<ClaimsPrincipal>()))
                .Throws(default);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(StatusCodes.Status500InternalServerError, viewResult.RouteValues["code"]);
        }

        [Fact]
        public async Task GetAsyncCorrect()
        {
            // Arrange
            var annualReportViewModel = new AnnualReportViewModel { ID = 1, CityId = 1, UserId = "1", Date = DateTime.Now };
            _mapper.Setup(m => m.Map<AnnualReportDTO, AnnualReportViewModel>(It.IsAny<AnnualReportDTO>()))
                .Returns(annualReportViewModel);

            // Act
            var result = await controller.GetAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<PartialViewResult>(result);
            Assert.Equal("_Get", viewResult.ViewName);
            var actualViewModel = Assert.IsType<AnnualReportViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(annualReportViewModel), JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public async Task GetAsyncNotFound()
        {
            // Arrange
            _mapper.Setup(m => m.Map<AnnualReportDTO, AnnualReportViewModel>(It.IsAny<AnnualReportDTO>()))
                .Throws(default(UnauthorizedAccessException));

            // Act
            var result = await controller.GetAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetAsyncError()
        {
            // Arrange
            _mapper.Setup(m => m.Map<AnnualReportDTO, AnnualReportViewModel>(It.IsAny<AnnualReportDTO>()))
                .Throws(default);

            // Act
            var result = await controller.GetAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.Equal("Не вдалося завантажити річний звіт!", objectResult.Value);
        }

        [Fact]
        public async Task ConfirmAsyncCorrect()
        {
            // Act
            var result = await controller.ConfirmAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Річний звіт станиці успішно підтверджено!", objectResult.Value);
        }

        [Fact]
        public async Task ConfirmAsyncAnnualReportError()
        {
            // Arrange
            _annualReportService.Setup(a => a.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(default(UnauthorizedAccessException));

            // Act
            var result = await controller.ConfirmAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task ConfirmAsyncError()
        {
            // Arrange
            _annualReportService.Setup(a => a.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(default);

            // Act
            var result = await controller.ConfirmAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.Equal("Не вдалося підтвердити річний звіт!", objectResult.Value);
        }

        [Fact]
        public async Task CancelAsyncCorrect()
        {
            // Act
            var result = await controller.CancelAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Річний звіт станиці успішно скасовано!", objectResult.Value);
        }

        [Fact]
        public async Task CancelAsyncAnnualReportError()
        {
            // Arrange
            _annualReportService.Setup(a => a.CancelAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(default(UnauthorizedAccessException));

            // Act
            var result = await controller.CancelAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task CancelAsyncError()
        {
            // Arrange
            _annualReportService.Setup(a => a.CancelAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(default);

            // Act
            var result = await controller.CancelAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.Equal("Не вдалося скасувати річний звіт!", objectResult.Value);
        }

        [Fact]
        public async Task DeleteAsyncCorrect()
        {
            // Act
            var result = await controller.DeleteAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Річний звіт станиці успішно видалено!", objectResult.Value);
        }

        [Fact]
        public async Task DeleteAsyncAnnualReportError()
        {
            // Arrange
            _annualReportService.Setup(a => a.DeleteAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(default(UnauthorizedAccessException));

            // Act
            var result = await controller.DeleteAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteAsyncError()
        {
            // Arrange
            _annualReportService.Setup(a => a.DeleteAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(default);

            // Act
            var result = await controller.DeleteAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.Equal("Не вдалося видалити річний звіт!", objectResult.Value);
        }

        [Fact]
        public async Task EditAssyncCorrect()
        {
            // Arrange
            var city = new CityViewModel
            {
                ID = 1,
                Name = "Львів"
            };
            var cityVM = new CityVMs.CityViewModel
            {
                ID = 1, Name = "Львів"
            };
            var annualReport = new AnnualReportViewModel
            {
                ID = 1, CityId = city.ID, UserId = "1", Date = DateTime.Now, City = city
            };
            var cityMembers = new List<CityVMs.CityMembersViewModel>
            {
                new CityVMs.CityMembersViewModel
                {
                    UserId = "1",
                    User = new UserVMs.UserViewModel { FirstName = "Петро", LastName = "Петренко" }
                }
            };
            var expectedViewModel = new CreateEditAnnualReportViewModel(cityMembers)
            {
                Operation = AnnualReportOperation.Editing,
                CityName = city.Name,
                CityMembers = new List<SelectListItem>
                {
                    new SelectListItem { Text = "" },
                    new SelectListItem { Value = cityMembers.First().UserId, Text = $"{cityMembers.First().User.FirstName} {cityMembers.First().User.LastName}" }
                },
                CityLegalStatusTypes = new List<SelectListItem>
                {
                    new SelectListItem { Value = "RegisteredLegalEntity", Text = "Зареєстрована юридична особа" },
                    new SelectListItem { Value = "LegalizedByMessage", Text = "Легалізована шляхом повідомлення" },
                    new SelectListItem { Value = "NotLegalizedInByLocalAuthorities", Text = "Нелегалізована у місцевих органах влади" },
                    new SelectListItem { Value = "InTheProcessOfLegalization", Text = "В процесі легалізації/реєстрації" },
                    new SelectListItem { Value = "RegisteredSeparatedSubdivision", Text = "Зареєстрований відокремлений підрозділ" }
                },
                AnnualReport = annualReport
            };
            _mapper.Setup(m => m.Map<AnnualReportDTO, AnnualReportViewModel>(It.IsAny<AnnualReportDTO>()))
                .Returns(annualReport);
            _mapper.Setup(m => m.Map<CityDTOs.CityDTO, CityVMs.CityViewModel>(It.IsAny<CityDTOs.CityDTO>()))
                .Returns(cityVM);
            _mapper.Setup(m => m.Map<IEnumerable<CityDTOs.CityMembersDTO>, IEnumerable<CityVMs.CityMembersViewModel>>(It.IsAny<IEnumerable<CityDTOs.CityMembersDTO>>()))
                .Returns(cityMembers);

            // Act
            var result = await controller.EditAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAsync", viewResult.ViewName);
            Assert.Null(viewResult.ViewData["ErrorMessage"]);
            var actualViewModel = Assert.IsType<CreateEditAnnualReportViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel), JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public async Task EditAsyncAnnualReportError()
        {
            // Arrange
            _annualReportService.Setup(a => a.GetByIdAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(new UnauthorizedAccessException("Станиця має непідтверджені звіти!"));

            // Act
            var result = await controller.EditAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAsync", viewResult.ViewName);
            Assert.Equal("Станиця має непідтверджені звіти!", viewResult.ViewData["ErrorMessage"]);
            _annualReportService.Verify(a => a.GetByIdAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()));
            _mapper.Verify(m => m.Map<AnnualReportDTO, AnnualReportViewModel>(It.IsAny<AnnualReportDTO>()), Times.Never);
        }

        [Fact]
        public async Task EditAsyncError()
        {
            // Arrange
            _annualReportService.Setup(a => a.GetByIdAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(default);

            // Act
            var result = await controller.EditAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(StatusCodes.Status500InternalServerError, viewResult.RouteValues["code"]);
            _annualReportService.Verify(a => a.GetByIdAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()));
            _mapper.Verify(m => m.Map<AnnualReportDTO, AnnualReportViewModel>(It.IsAny<AnnualReportDTO>()), Times.Never);
        }

        [Fact]
        public async Task EditAsyncPostCorrect()
        {
            // Act
            var result = await controller.EditAsync(It.IsAny<AnnualReportViewModel>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAsync", viewResult.ViewName);
            Assert.Equal("Річний звіт станиці успішно відредаговано!", viewResult.ViewData["Message"]);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public async Task EditAsyncPostInvalid()
        {
            // Arrange
            var city = new CityVMs.CityViewModel
            {
                ID = 1,
                Name = "Львів"
            };
            var annualReport = new AnnualReportViewModel
            {
                ID = 1,
                CityId = city.ID,
                UserId = "1",
                Date = DateTime.Now,
                City = new CityViewModel
                {
                    ID = 1,
                    Name = "Львів"
                }
            };
            var cityMembers = new List<CityVMs.CityMembersViewModel>
            {
                new CityVMs.CityMembersViewModel
                {
                    UserId = "1",
                    User = new UserVMs.UserViewModel { FirstName = "Петро", LastName = "Петренко" }
                }
            };
            var expectedViewModel = new CreateEditAnnualReportViewModel(cityMembers)
            {
                Operation = AnnualReportOperation.Editing,
                CityName = city.Name,
                CityMembers = new List<SelectListItem>
                {
                    new SelectListItem { Text = "" },
                    new SelectListItem { Value = cityMembers.First().UserId, Text = $"{cityMembers.First().User.FirstName} {cityMembers.First().User.LastName}" }
                },
                CityLegalStatusTypes = new List<SelectListItem>
                {
                    new SelectListItem { Value = "RegisteredLegalEntity", Text = "Зареєстрована юридична особа" },
                    new SelectListItem { Value = "LegalizedByMessage", Text = "Легалізована шляхом повідомлення" },
                    new SelectListItem { Value = "NotLegalizedInByLocalAuthorities", Text = "Нелегалізована у місцевих органах влади" },
                    new SelectListItem { Value = "InTheProcessOfLegalization", Text = "В процесі легалізації/реєстрації" },
                    new SelectListItem { Value = "RegisteredSeparatedSubdivision", Text = "Зареєстрований відокремлений підрозділ" }
                },
                AnnualReport = annualReport
            };
            _mapper.Setup(m => m.Map<CityDTOs.CityDTO, CityVMs.CityViewModel>(It.IsAny<CityDTOs.CityDTO>()))
                .Returns(city);
            _mapper.Setup(m => m.Map<IEnumerable<CityDTOs.CityMembersDTO>, IEnumerable<CityVMs.CityMembersViewModel>>(It.IsAny<IEnumerable<CityDTOs.CityMembersDTO>>()))
                .Returns(cityMembers);
            controller.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = await controller.EditAsync(annualReport);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAsync", viewResult.ViewName);
            Assert.Equal("Річний звіт заповнений некоректно!", viewResult.ViewData["ErrorMessage"]);
            var actualViewModel = Assert.IsType<CreateEditAnnualReportViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel), JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public async Task EditAsyncPostUnauthorizedAccessException()
        {
            // Arrange
            _annualReportService.Setup(a => a.EditAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<AnnualReportDTO>()))
                .Throws(new UnauthorizedAccessException(string.Empty));

            // Act
            var result = await controller.EditAsync(It.IsAny<AnnualReportViewModel>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAsync", viewResult.ViewName);
            Assert.Null(viewResult.Model);
            Assert.Equal(string.Empty, viewResult.ViewData["ErrorMessage"]);
        }

        [Fact]
        public async Task EditAsyncPostInvalidOperationException()
        {
            // Arrange
            _annualReportService.Setup(a => a.EditAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<AnnualReportDTO>()))
                .Throws(new InvalidOperationException(string.Empty));

            // Act
            var result = await controller.EditAsync(It.IsAny<AnnualReportViewModel>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAsync", viewResult.ViewName);
            Assert.Null(viewResult.Model);
            Assert.Equal(string.Empty, viewResult.ViewData["ErrorMessage"]);
        }

        [Fact]
        public async Task EditAsyncPostError()
        {
            // Arrange
            _annualReportService.Setup(a => a.EditAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<AnnualReportDTO>()))
                .Throws(default);

            // Act
            var result = await controller.EditAsync(It.IsAny<AnnualReportViewModel>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(StatusCodes.Status500InternalServerError, viewResult.RouteValues["code"]);
        }
    }
}