using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest
{
    public class HomeControllerTests
    {
        private Mock<IHomeService> _homeService;
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;

        public HomeControllerTests()
        {
            _homeService = new Mock<IHomeService>();
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public void IndexViewResultNotNull()
        {
            var controller = new HomeController(_homeService.Object, _repoWrapper.Object, _mapper.Object);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void AboutPLASTViewResultNotNull()
        {
            var controller = new HomeController(_homeService.Object, _repoWrapper.Object, _mapper.Object);

            var result = controller.AboutPLAST();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void ContactsViewResultNotNull()
        {
            var controller = new HomeController(_homeService.Object, _repoWrapper.Object, _mapper.Object);

            var result = controller.Contacts();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void FAQViewResultNotNull()
        {
            var controller = new HomeController(_homeService.Object, _repoWrapper.Object, _mapper.Object);

            var result = controller.FAQ();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void GetInformationViewResultNotNull()
        {
            var controller = new HomeController(_homeService.Object, _repoWrapper.Object, _mapper.Object);

            var result = controller.GetInformation();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void SearchResultNotNull()
        {
            _repoWrapper.Setup(p => p.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(GetTestQueryableUsersFullNames());

            var homecontroller = new HomeController(_homeService.Object, _repoWrapper.Object, _mapper.Object);

            var searchResult = homecontroller.Search("Іванків");

            var viewResult = Assert.IsType<ViewResult>(searchResult);
            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.Model);

            var model = viewResult.Model as SearchSurname;
            var resultQuery = Assert.IsAssignableFrom<IQueryable<User>>(model.Users);

            Assert.True(resultQuery.All(it => it.LastName == "Іванків"));
        }

        [Fact]
        public void GetSerchUserTest()
        {
            _repoWrapper.Setup(u => u.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(GetTestQueryableUsersWithId());

            var homecontroller = new HomeController(_homeService.Object, _repoWrapper.Object, _mapper.Object);

            var searchResult = homecontroller.GetSearchUser("aaaa-bbbb-cccc");

            var viewResult = Assert.IsType<PartialViewResult>(searchResult);
            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.Model);

            var model = Assert.IsAssignableFrom<IQueryable<User>>(viewResult.Model);
            Assert.True(model.All(it => it.Id == "aaaa-bbbb-cccc"));
        }

        [Fact]
        public void FeedBackSendedViewResultNotNull()
        {
            var controller = new HomeController(_homeService.Object, _repoWrapper.Object, _mapper.Object);

            var result = controller.FeedBackSended();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task TestSendContactsFeedBackSended()
        {
            var controller = new HomeController(_homeService.Object, _repoWrapper.Object, _mapper.Object);
            _mapper
                .Setup(s => s.Map<ContactDTO>(It.IsAny<ContactsViewModel>()))
                .Returns(GetTestContactDtoWithAllFields());

            var validResult = await controller.SendContacts(GetTestValidContactViewModel());

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(validResult);
            Assert.NotNull(redirectToActionResult);
            Assert.Equal("FeedBackSended", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task TestSendContactsReturnContacts()
        {
            var controller = new HomeController(_homeService.Object, _repoWrapper.Object, _mapper.Object);
            controller.ModelState.AddModelError("NameError", "Required");

            var invalidResult = await controller.SendContacts(GetTestInvalidContactViewModel());

            var viewResultWrong = Assert.IsType<ViewResult>(invalidResult);
            Assert.NotNull(viewResultWrong);
            Assert.Equal("Contacts", viewResultWrong.ViewName);
        }

        private ContactsViewModel GetTestValidContactViewModel()
        {
            return new ContactsViewModel
            {
                Name = "Настя",
                Email = "nasty@gmail.com",
                PhoneNumber = "0934353139",
                FeedBackDescription = "Хотіла б стати вашим волонтером"
            };
        }

        private ContactsViewModel GetTestInvalidContactViewModel()
        {
            return new ContactsViewModel
            {
                Name = "",
                Email = "",
                PhoneNumber = "",
                FeedBackDescription = ""
            };
        }

        private ContactDTO GetTestContactDtoWithAllFields()
        {
            return new ContactDTO()
            {
                Name = "Настя",
                Email = "nasty@gmail.com",
                PhoneNumber = "0934353139",
                FeedBackDescription = "Хотіла б стати вашим волонтером"
            };
        }

        private IQueryable<User> GetTestQueryableUsersFullNames()
        {
            return new List<User> {
                    new User
                    {
                        FirstName="Денис",
                        LastName = "Іванків"
                    },
                    new User
                    {
                        FirstName="Олег",
                        LastName = "Іванків"
                    },
                    new User
                    {
                        FirstName="Микола",
                        LastName = "Іванків"
                    },
            }.AsQueryable();
        }

        private IQueryable<User> GetTestQueryableUsersWithId()
        {
            return new List<User> {
                    new User
                    {
                        Id="aaaa-bbbb-cccc",
                        FirstName="Олег",
                        LastName="Іванків"
                    },
            }.AsQueryable();
        }
    }
}
