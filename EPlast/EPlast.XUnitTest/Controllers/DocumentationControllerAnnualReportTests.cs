using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Exceptions;
using EPlast.BussinessLayer.Interfaces.City;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.Controllers;
using EPlast.Models.Enums;
using EPlast.ViewModels;
using EPlast.ViewModels.City;
using EPlast.ViewModels.UserInformation.UserProfile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest
{
    public class DocumentationControllerAnnualReportTests
    {
        private readonly Mock<ILogger<DocumentationController>> _logger = new Mock<ILogger<DocumentationController>>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IAnnualReportService> _annualReportService = new Mock<IAnnualReportService>();
        private readonly Mock<ICityAccessService> _cityAccessService = new Mock<ICityAccessService>();
        private readonly Mock<ICityMembersService> _cityMembersService = new Mock<ICityMembersService>();
        private readonly Mock<ICityService> _cityService = new Mock<ICityService>();
        private readonly DocumentationController controller;

        public DocumentationControllerAnnualReportTests()
        {
            controller = new DocumentationController(null, _logger.Object, null, _mapper.Object, _annualReportService.Object, _cityAccessService.Object,
                _cityMembersService.Object, _cityService.Object);
        }

        [Fact]
        public async Task CreateAnnualReportAsyncCorrect()
        {
            // Arrange
            IEnumerable<CityDTO> cities = new List<CityDTO>
            {
                new CityDTO { ID = 1, Name = "Львів" }
            };
            var city = new CityViewModel { ID = cities.First().ID, Name = cities.First().Name };
            IEnumerable<CityMembersDTO> cityMembersDTOs = new List<CityMembersDTO>
            {
                new CityMembersDTO { UserId = "1", User = new UserDTO { FirstName = "Петро", LastName = "Петренко" } }
            };
            var cityMembers = new List<CityMembersViewModel>
            {
                new CityMembersViewModel
                {
                    UserId = cityMembersDTOs.First().UserId,
                    User = new UserViewModel { FirstName = cityMembersDTOs.First().User.FirstName, LastName = cityMembersDTOs.First().User.LastName }
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
                .Returns(Task.FromResult(cities));
            _mapper.Setup(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()))
                .Returns(city);
            _annualReportService.Setup(a => a.HasUnconfirmedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));
            _annualReportService.Setup(a => a.HasCreatedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));
            _cityMembersService.Setup(c => c.GetCurrentByCityIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(cityMembersDTOs));
            _mapper.Setup(m => m.Map<IEnumerable<CityMembersDTO>, IEnumerable<CityMembersViewModel>>(It.IsAny<IEnumerable<CityMembersDTO>>()))
                .Returns(cityMembers);

            // Act
            var result = await controller.CreateAnnualReportAsync();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAnnualReport", viewResult.ViewName);
            Assert.Null(viewResult.ViewData["ErrorMessage"]);
            var actualViewModel = Assert.IsType<CreateEditAnnualReportViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel), JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public async Task CreateAnnualReportAsyncHasUnconfirmed()
        {
            // Arrange
            IEnumerable<CityDTO> cities = new List<CityDTO>
            {
                new CityDTO { ID = 1, Name = "Львів" }
            };
            var city = new CityViewModel { ID = cities.First().ID, Name = cities.First().Name };
            _cityAccessService.Setup(cas => cas.GetCitiesAsync(It.IsAny<ClaimsPrincipal>()))
               .Returns(Task.FromResult(cities));
            _mapper.Setup(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()))
                .Returns(city);
            _annualReportService.Setup(a => a.HasUnconfirmedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            // Act
            var result = await controller.CreateAnnualReportAsync();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAnnualReport", viewResult.ViewName);
            Assert.Equal("Станиця має непідтверджені звіти!", viewResult.ViewData["ErrorMessage"]);
            _annualReportService.Verify(a => a.HasUnconfirmedAsync(It.IsAny<int>()));
            _annualReportService.Verify(a => a.HasCreatedAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAnnualReportAsyncHasCreated()
        {
            // Arrange
            IEnumerable<CityDTO> cities = new List<CityDTO>
            {
                new CityDTO { ID = 1, Name = "Львів" }
            };
            var city = new CityViewModel { ID = cities.First().ID, Name = cities.First().Name };
            _cityAccessService.Setup(cas => cas.GetCitiesAsync(It.IsAny<ClaimsPrincipal>()))
               .Returns(Task.FromResult(cities));
            _mapper.Setup(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()))
                .Returns(city);
            _annualReportService.Setup(a => a.HasUnconfirmedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));
            _annualReportService.Setup(a => a.HasCreatedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            // Act
            var result = await controller.CreateAnnualReportAsync();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAnnualReport", viewResult.ViewName);
            Assert.Equal("Річний звіт для даної станиці вже створений!", viewResult.ViewData["ErrorMessage"]);
            _annualReportService.Verify(a => a.HasCreatedAsync(It.IsAny<int>()));
            _cityMembersService.Verify(c => c.GetCurrentByCityIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAnnualReportAsyncError()
        {
            // Arrange
            _cityAccessService.Setup(cas => cas.GetCitiesAsync(It.IsAny<ClaimsPrincipal>()))
               .Returns(Task.FromResult(Enumerable.Empty<CityDTO>()));

            // Act
            var result = await controller.CreateAnnualReportAsync();

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(StatusCodes.Status500InternalServerError, viewResult.RouteValues["code"]);
            _mapper.Verify(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()), Times.Never);
        }

        [Fact]
        public async Task CreateAnnualReportLikeAdminAsyncCorrect()
        {
            // Arrange
            var cityDTO = new CityDTO { ID = 1, Name = "Львів" };
            var city = new CityViewModel { ID = cityDTO.ID, Name = cityDTO.Name };
            IEnumerable<CityMembersDTO> cityMembersDTOs = new List<CityMembersDTO>
            {
                new CityMembersDTO { UserId = "1", User = new UserDTO { FirstName = "Петро", LastName = "Петренко" } }
            };
            var cityMembers = new List<CityMembersViewModel>
            {
                new CityMembersViewModel
                {
                    UserId = cityMembersDTOs.First().UserId,
                    User = new UserViewModel { FirstName = cityMembersDTOs.First().User.FirstName, LastName = cityMembersDTOs.First().User.LastName }
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
                .Returns(Task.FromResult(true));
            _annualReportService.Setup(a => a.HasUnconfirmedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));
            _annualReportService.Setup(a => a.HasCreatedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));
            _cityService.Setup(c => c.GetById(It.IsAny<int>()))
                .Returns(cityDTO);
            _mapper.Setup(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()))
                .Returns(city);
            _cityMembersService.Setup(c => c.GetCurrentByCityIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(cityMembersDTOs));
            _mapper.Setup(m => m.Map<IEnumerable<CityMembersDTO>, IEnumerable<CityMembersViewModel>>(It.IsAny<IEnumerable<CityMembersDTO>>()))
                .Returns(cityMembers);

            // Act
            var result = await controller.CreateAnnualReportLikeAdminAsync(city.ID);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAnnualReport", viewResult.ViewName);
            Assert.Null(viewResult.ViewData["ErrorMessage"]);
            var actualViewModel = Assert.IsType<CreateEditAnnualReportViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel), JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public async Task CreateAnnualReportLikeAdminAsyncHasUnconfirmed()
        {
            // Arrange
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            _annualReportService.Setup(a => a.HasUnconfirmedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            // Act
            var result = await controller.CreateAnnualReportLikeAdminAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAnnualReport", viewResult.ViewName);
            Assert.Equal("Станиця має непідтверджені звіти!", viewResult.ViewData["ErrorMessage"]);
            _annualReportService.Verify(a => a.HasUnconfirmedAsync(It.IsAny<int>()));
            _annualReportService.Verify(a => a.HasCreatedAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAnnualReportLikeAdminAsyncHasCreated()
        {
            // Arrange
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            _annualReportService.Setup(a => a.HasUnconfirmedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));
            _annualReportService.Setup(a => a.HasCreatedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            // Act
            var result = await controller.CreateAnnualReportLikeAdminAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAnnualReport", viewResult.ViewName);
            Assert.Equal("Річний звіт для даної станиці вже створений!", viewResult.ViewData["ErrorMessage"]);
            _annualReportService.Verify(a => a.HasCreatedAsync(It.IsAny<int>()));
            _cityMembersService.Verify(c => c.GetCurrentByCityIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAnnualReportLikeAdminAsyncError()
        {
            // Arrange
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            _annualReportService.Setup(a => a.HasUnconfirmedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));
            _annualReportService.Setup(a => a.HasCreatedAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));
            _cityService.Setup(c => c.GetById(It.IsAny<int>()))
                .Returns(default(CityDTO));
            _mapper.Setup(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()))
                .Returns(default(CityViewModel));

            // Act
            var result = await controller.CreateAnnualReportLikeAdminAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(StatusCodes.Status500InternalServerError, viewResult.RouteValues["code"]);
            _mapper.Verify(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()));
            _cityMembersService.Verify(c => c.GetCurrentByCityIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAnnualReportLikeAdminAsyncErrorNoAccess()
        {
            // Arrange
            _cityAccessService.Setup(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Returns(Task.FromResult(false));

            // Act
            var result = await controller.CreateAnnualReportLikeAdminAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(StatusCodes.Status500InternalServerError, viewResult.RouteValues["code"]);
            _cityAccessService.Verify(c => c.HasAccessAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()));
            _annualReportService.Verify(a => a.HasUnconfirmedAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CreateAnnualReportAsyncPostCorrect()
        {
            // Arrange
            _mapper.Setup(m => m.Map<AnnualReportViewModel, AnnualReportDTO>(It.IsAny<AnnualReportViewModel>()))
                .Returns(default(AnnualReportDTO));

            // Act
            var result = await controller.CreateAnnualReportAsync(It.IsAny<AnnualReportViewModel>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAnnualReport", viewResult.ViewName);
            Assert.Equal("Річний звіт станиці успішно створено!", viewResult.ViewData["Message"]);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public async Task CreateAnnualReportAsyncPostInvalid()
        {
            // Arrange
            var cityDTO = new CityDTO { ID = 1, Name = "Львів" };
            var city = new CityViewModel { ID = cityDTO.ID, Name = cityDTO.Name };
            IEnumerable<CityMembersDTO> cityMembersDTOs = new List<CityMembersDTO>
            {
                new CityMembersDTO { UserId = "1", User = new UserDTO { FirstName = "Петро", LastName = "Петренко" } }
            };
            var cityMembers = new List<CityMembersViewModel>
            {
                new CityMembersViewModel
                {
                    UserId = cityMembersDTOs.First().UserId,
                    User = new UserViewModel { FirstName = cityMembersDTOs.First().User.FirstName, LastName = cityMembersDTOs.First().User.LastName }
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
            _cityService.Setup(c => c.GetById(It.IsAny<int>()))
                .Returns(cityDTO);
            _mapper.Setup(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()))
                .Returns(city);
            _cityMembersService.Setup(c => c.GetCurrentByCityIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(cityMembersDTOs));
            _mapper.Setup(m => m.Map<IEnumerable<CityMembersDTO>, IEnumerable<CityMembersViewModel>>(It.IsAny<IEnumerable<CityMembersDTO>>()))
                .Returns(cityMembers);

            // Act
            var result = await controller.CreateAnnualReportAsync(expectedViewModel.AnnualReport);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAnnualReport", viewResult.ViewName);
            Assert.Equal("Річний звіт заповнений некоректно!", viewResult.ViewData["ErrorMessage"]);
            var actualViewModel = Assert.IsType<CreateEditAnnualReportViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel), JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public async Task CreateAnnualReportAsyncPostAnnualReportError()
        {
            // Arrange
            _mapper.Setup(m => m.Map<AnnualReportViewModel, AnnualReportDTO>(It.IsAny<AnnualReportViewModel>()))
                .Returns(default(AnnualReportDTO));
            _annualReportService.Setup(a => a.CreateAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<AnnualReportDTO>()))
                .Throws(new AnnualReportException(string.Empty));

            // Act
            var result = await controller.CreateAnnualReportAsync(It.IsAny<AnnualReportViewModel>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAnnualReport", viewResult.ViewName);
            Assert.Null(viewResult.Model);
            Assert.Equal(string.Empty, viewResult.ViewData["ErrorMessage"]);

        }

        [Fact]
        public async Task CreateAnnualReportAsyncPostError()
        {
            // Arrange
            controller.ModelState.AddModelError(string.Empty, string.Empty);
            _cityService.Setup(c => c.GetById(It.IsAny<int>()))
                .Returns(default(CityDTO));
            _mapper.Setup(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()))
                .Returns(default(CityViewModel));

            // Act
            var result = await controller.CreateAnnualReportAsync(new AnnualReportViewModel());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(StatusCodes.Status500InternalServerError, viewResult.RouteValues["code"]);
        }

        [Fact]
        public async Task ViewAnnualReportsAsyncCorrect()
        {
            // Arrange
            var cities = new List<CityViewModel>
            {
                new CityViewModel { ID = 1, Name = "Львів" }
            };
            var annualReports = new List<AnnualReportViewModel>
            {
                new AnnualReportViewModel { ID = 1, CityId = cities.First().ID, City = cities.First(), UserId = "1", Date = DateTime.Now }
            };
            var expectedViewModel = new ViewAnnualReportsViewModel(cities)
            {
                Cities = new List<SelectListItem>
                {
                    new SelectListItem { Value = cities.First().ID.ToString(), Text = cities.First().Name }
                },
                AnnualReports = annualReports
            };
            _mapper.Setup(m => m.Map<IEnumerable<CityDTO>, IEnumerable<CityViewModel>>(It.IsAny<IEnumerable<CityDTO>>()))
                .Returns(cities);
            _mapper.Setup(m => m.Map<IEnumerable<AnnualReportDTO>, IEnumerable<AnnualReportViewModel>>(It.IsAny<IEnumerable<AnnualReportDTO>>()))
                .Returns(annualReports);

            // Act
            var result = await controller.ViewAnnualReportsAsync();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualViewModel = Assert.IsType<ViewAnnualReportsViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel), JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public async Task ViewAnnualReportsAsyncError()
        {
            // Arrange
            _cityAccessService.Setup(c => c.GetCitiesAsync(It.IsAny<ClaimsPrincipal>()))
                .Throws(default);

            // Act
            var result = await controller.ViewAnnualReportsAsync();

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(StatusCodes.Status500InternalServerError, viewResult.RouteValues["code"]);
        }

        [Fact]
        public async Task GetAnnualReportAsyncCorrect()
        {
            // Arrange
            var annualReportViewModel = new AnnualReportViewModel { ID = 1, CityId = 1, UserId = "1", Date = DateTime.Now };
            _mapper.Setup(m => m.Map<AnnualReportDTO, AnnualReportViewModel>(It.IsAny<AnnualReportDTO>()))
                .Returns(annualReportViewModel);

            // Act
            var result = await controller.GetAnnualReportAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<PartialViewResult>(result);
            Assert.Equal("_GetAnnualReport", viewResult.ViewName);
            var actualViewModel = Assert.IsType<AnnualReportViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(annualReportViewModel), JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public async Task GetAnnualReportAsyncNotFound()
        {
            // Arrange
            _mapper.Setup(m => m.Map<AnnualReportDTO, AnnualReportViewModel>(It.IsAny<AnnualReportDTO>()))
                .Throws(default(AnnualReportException));

            // Act
            var result = await controller.GetAnnualReportAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetAnnualReportAsyncError()
        {
            // Arrange
            _mapper.Setup(m => m.Map<AnnualReportDTO, AnnualReportViewModel>(It.IsAny<AnnualReportDTO>()))
                .Throws(default);

            // Act
            var result = await controller.GetAnnualReportAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.Equal("Не вдалося завантажити річний звіт!", objectResult.Value);
        }

        [Fact]
        public async Task ConfirmAnnualReportAsyncCorrect()
        {
            // Act
            var result = await controller.ConfirmAnnualReportAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Річний звіт станиці успішно підтверджено!", objectResult.Value);
        }

        [Fact]
        public async Task ConfirmAnnualReportAsyncAnnualReportError()
        {
            // Arrange
            _annualReportService.Setup(a => a.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(default(AnnualReportException));

            // Act
            var result = await controller.ConfirmAnnualReportAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task ConfirmAnnualReportAsyncError()
        {
            // Arrange
            _annualReportService.Setup(a => a.ConfirmAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(default);

            // Act
            var result = await controller.ConfirmAnnualReportAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.Equal("Не вдалося підтвердити річний звіт!", objectResult.Value);
        }

        [Fact]
        public async Task CancelAnnualReportAsyncCorrect()
        {
            // Act
            var result = await controller.CancelAnnualReportAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Річний звіт станиці успішно скасовано!", objectResult.Value);
        }

        [Fact]
        public async Task CancelAnnualReportAsyncAnnualReportError()
        {
            // Arrange
            _annualReportService.Setup(a => a.CancelAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(default(AnnualReportException));

            // Act
            var result = await controller.CancelAnnualReportAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task CancelAnnualReportAsyncError()
        {
            // Arrange
            _annualReportService.Setup(a => a.CancelAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(default);

            // Act
            var result = await controller.CancelAnnualReportAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.Equal("Не вдалося скасувати річний звіт!", objectResult.Value);
        }

        [Fact]
        public async Task DeleteAnnualReportAsyncCorrect()
        {
            // Act
            var result = await controller.DeleteAnnualReportAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Річний звіт станиці успішно видалено!", objectResult.Value);
        }

        [Fact]
        public async Task DeleteAnnualReportAsyncAnnualReportError()
        {
            // Arrange
            _annualReportService.Setup(a => a.DeleteAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(default(AnnualReportException));

            // Act
            var result = await controller.DeleteAnnualReportAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteAnnualReportAsyncError()
        {
            // Arrange
            _annualReportService.Setup(a => a.DeleteAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(default);

            // Act
            var result = await controller.DeleteAnnualReportAsync(It.IsAny<int>());

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.Equal("Не вдалося видалити річний звіт!", objectResult.Value);
        }

        [Fact]
        public async Task EditAnnualReportAssyncCorrect()
        {
            // Arrange
            var city = new CityViewModel
            {
                ID = 1, Name = "Львів"
            };
            var annualReport = new AnnualReportViewModel
            {
                ID = 1, CityId = city.ID, UserId = "1", Date = DateTime.Now, City = city
            };
            var cityMembers = new List<CityMembersViewModel>
            {
                new CityMembersViewModel
                {
                    UserId = "1",
                    User = new UserViewModel { FirstName = "Петро", LastName = "Петренко" }
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
            _mapper.Setup(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()))
                .Returns(city);
            _mapper.Setup(m => m.Map<IEnumerable<CityMembersDTO>, IEnumerable<CityMembersViewModel>>(It.IsAny<IEnumerable<CityMembersDTO>>()))
                .Returns(cityMembers);

            // Act
            var result = await controller.EditAnnualReportAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAnnualReport", viewResult.ViewName);
            Assert.Null(viewResult.ViewData["ErrorMessage"]);
            var actualViewModel = Assert.IsType<CreateEditAnnualReportViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel), JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public async Task EditAnnualReportAsyncAnnualReportError()
        {
            // Arrange
            _annualReportService.Setup(a => a.GetByIdAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(new AnnualReportException("Станиця має непідтверджені звіти!"));

            // Act
            var result = await controller.EditAnnualReportAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAnnualReport", viewResult.ViewName);
            Assert.Equal("Станиця має непідтверджені звіти!", viewResult.ViewData["ErrorMessage"]);
            _annualReportService.Verify(a => a.GetByIdAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()));
            _mapper.Verify(m => m.Map<AnnualReportDTO, AnnualReportViewModel>(It.IsAny<AnnualReportDTO>()), Times.Never);
        }

        [Fact]
        public async Task EditAnnualReportAsyncError()
        {
            // Arrange
            _annualReportService.Setup(a => a.GetByIdAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
                .Throws(default);

            // Act
            var result = await controller.EditAnnualReportAsync(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(StatusCodes.Status500InternalServerError, viewResult.RouteValues["code"]);
            _annualReportService.Verify(a => a.GetByIdAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()));
            _mapper.Verify(m => m.Map<AnnualReportDTO, AnnualReportViewModel>(It.IsAny<AnnualReportDTO>()), Times.Never);
        }

        [Fact]
        public async Task EditAnnualReportAsyncPostCorrect()
        {
            // Act
            var result = await controller.EditAnnualReportAsync(It.IsAny<AnnualReportViewModel>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAnnualReport", viewResult.ViewName);
            Assert.Equal("Річний звіт станиці успішно відредаговано!", viewResult.ViewData["Message"]);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public async Task EditAnnualReportAsyncPostInvalid()
        {
            // Arrange
            var city = new CityViewModel
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
                City = city
            };
            var cityMembers = new List<CityMembersViewModel>
            {
                new CityMembersViewModel
                {
                    UserId = "1",
                    User = new UserViewModel { FirstName = "Петро", LastName = "Петренко" }
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
            _mapper.Setup(m => m.Map<CityDTO, CityViewModel>(It.IsAny<CityDTO>()))
                .Returns(city);
            _mapper.Setup(m => m.Map<IEnumerable<CityMembersDTO>, IEnumerable<CityMembersViewModel>>(It.IsAny<IEnumerable<CityMembersDTO>>()))
                .Returns(cityMembers);
            controller.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = await controller.EditAnnualReportAsync(annualReport);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAnnualReport", viewResult.ViewName);
            Assert.Equal("Річний звіт заповнений некоректно!", viewResult.ViewData["ErrorMessage"]);
            var actualViewModel = Assert.IsType<CreateEditAnnualReportViewModel>(viewResult.Model);
            Assert.Equal(JsonConvert.SerializeObject(expectedViewModel), JsonConvert.SerializeObject(actualViewModel));
        }

        [Fact]
        public async Task EditAnnualReportAsyncPostAnnualReportError()
        {
            // Arrange
            _annualReportService.Setup(a => a.EditAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<AnnualReportDTO>()))
                .Throws(new AnnualReportException(string.Empty));

            // Act
            var result = await controller.EditAnnualReportAsync(It.IsAny<AnnualReportViewModel>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreateEditAnnualReport", viewResult.ViewName);
            Assert.Null(viewResult.Model);
            Assert.Equal(string.Empty, viewResult.ViewData["ErrorMessage"]);
        }

        [Fact]
        public async Task EditAnnualReportAsyncPostError()
        {
            // Arrange
            _annualReportService.Setup(a => a.EditAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<AnnualReportDTO>()))
                .Throws(default);

            // Act
            var result = await controller.EditAnnualReportAsync(It.IsAny<AnnualReportViewModel>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
            Assert.Equal(StatusCodes.Status500InternalServerError, viewResult.RouteValues["code"]);
        }
    }
}