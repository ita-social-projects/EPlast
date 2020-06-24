using AutoMapper;
using EPlast.BussinessLayer;
using EPlast.BussinessLayer.DTO;
using EPlast.Controllers;
using EPlast.Models;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BussinessLayer.Interfaces.Logging;
using EPlast.BussinessLayer.Services.Interfaces;
using Xunit;
using Organization = EPlast.Models.Organization;

namespace EPlast.XUnitTest
{
    public class DecisionControllerTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDecisionService> _decisionService;
        private readonly Mock<ILoggerService<DecisionController>> _loggerService;

        public DecisionControllerTests()

        {
            _mapper = new Mock<IMapper>();
            _decisionService = new Mock<IDecisionService>();
            _loggerService = new Mock<ILoggerService<DecisionController>>();
        }

        [Theory]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(42, false)]
        [InlineData(21, false)]
        public async Task GetDecisionByIdTest(int id, bool expected)
        {
            _decisionService.Setup(d => d.GetDecisionAsync(It.IsAny<int>()))
                .ReturnsAsync(() => CreateDecisionsDtoQueryable().First(x => x.ID == id));
            _mapper.Setup(d => d.Map<Decision>(It.IsAny<DecisionDTO>()))
                .Returns(() => CreateDecisionsQueryable().First(x => x.ID == id));
            DecisionController documentationController = CreateDocumentationController;

            JsonResult jsonResult = await documentationController.GetDecision(id);
            bool actual = jsonResult.Value.ToString().Contains("True");

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ChangeDecisionTest(bool expected)
        {
            DecisionViewModel decisionViewModel = CreateDecisionViewModel();
            _mapper.Setup(m => m.Map<DecisionDTO>(It.IsAny<Decision>()))
                 .Returns(new DecisionDTO());
            _decisionService.Setup(d => d.ChangeDecisionAsync(It.IsAny<DecisionDTO>()))
                .ReturnsAsync(() => expected);
            DecisionController documentationController = CreateDocumentationController;

            JsonResult jsonResult = await documentationController.ChangeDecision(decisionViewModel.DecisionWrapper.Decision);
            bool actual = jsonResult.Value.ToString().Contains("True");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task SaveDecisionWithNullDecisionViewMode()
        {
            DecisionWrapper decisionWrapper = null;
            DecisionController documentationController = CreateDocumentationController;

            JsonResult jsonResult = await documentationController.SaveDecision(decisionWrapper);
            bool actual = jsonResult.Value.ToString().Contains("False");

            Assert.True(actual);
        }

        [Fact]
        public async Task SaveDecisionWithIncorrectFileTest()
        {
            DecisionViewModel decisionViewModel = CreateDecisionViewModel();
            decisionViewModel.DecisionWrapper.File = new FormFile(null, 1234, 11241234, "fdd", "dfsdf");
            DecisionController documentationController = CreateDocumentationController;

            JsonResult jsonResult = await documentationController.SaveDecision(decisionViewModel.DecisionWrapper);
            bool actual = jsonResult.Value.ToString().Contains("file length > 10485760");

            Assert.True(actual);
        }

        [Fact]
        public async Task SaveDecisionCorrectTest()
        {
            _mapper.Setup(m => m.Map<DecisionWrapperDTO>(It.IsAny<DecisionWrapper>()))
                .Returns(new DecisionWrapperDTO());
            _decisionService.Setup(d => d.SaveDecisionAsync(It.IsAny<DecisionWrapperDTO>()))
                .Returns(Task.FromResult(5));
            _decisionService.Setup(d => d.GetDecisionOrganizationAsync(It.IsAny<OrganizationDTO>()))
                .ReturnsAsync(new OrganizationDTO { OrganizationName = string.Empty });
            DecisionViewModel decisionViewModel = CreateDecisionViewModel();
            DecisionController documentationController = CreateDocumentationController;

            JsonResult jsonResult = await documentationController.SaveDecision(decisionViewModel.DecisionWrapper);
            bool actual = jsonResult.Value.ToString().Contains("True");

            Assert.True(actual);
        }

        [Fact]
        public async Task CreateDecisionCorrectTest()
        {
            _mapper.Setup(m => m.Map<IEnumerable<Organization>>(It.IsAny<IEnumerable<OrganizationDTO>>()))
                .Returns(new List<Organization>());
            _mapper.Setup(m => m.Map<IEnumerable<DecisionWrapper>>(It.IsAny<IEnumerable<DecisionWrapperDTO>>()))
                .Returns(new List<DecisionWrapper>());
            _mapper.Setup(m => m.Map<IEnumerable<DecisionTarget>>(It.IsAny<IEnumerable<DecisionTargetDTO>>()))
                .Returns(new List<DecisionTarget>());
            _decisionService.Setup(d => d.GetDecisionStatusTypes()).Returns(new List<SelectListItem>());
            DecisionController documentationController = CreateDocumentationController;

            DecisionViewModel decisionViewModel = await documentationController.CreateDecision();

            Assert.IsType<DecisionViewModel>(decisionViewModel);
        }

        [Fact]
        public async Task CreateDecisionFailTest()
        {
            _mapper.Setup(m => m.Map<IEnumerable<Organization>>(It.IsAny<IEnumerable<OrganizationDTO>>()))
                .Returns(() => null);
            _mapper.Setup(m => m.Map<IEnumerable<DecisionWrapper>>(It.IsAny<IEnumerable<DecisionWrapperDTO>>()))
                .Returns(new List<DecisionWrapper>());
            _mapper.Setup(m => m.Map<IEnumerable<DecisionTarget>>(It.IsAny<IEnumerable<DecisionTargetDTO>>()))
                .Returns(new List<DecisionTarget>());
            _decisionService.Setup(d => d.GetDecisionStatusTypes()).Returns(new List<SelectListItem>());
            DecisionController documentationController = CreateDocumentationController;

            DecisionViewModel decisionViewModel = await documentationController.CreateDecision();

            Assert.True(decisionViewModel == null);
        }

        [Fact]
        public async Task ReadDecisionFailTest()
        {
            _mapper.Setup(m => m.Map<IEnumerable<DecisionWrapper>>(It.IsAny<IEnumerable<DecisionWrapperDTO>>()))
                .Returns(() => null);
            DecisionController documentationController = CreateDocumentationController;

            var result = await documentationController.ReadDecision();
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsType<ViewResult>(result);
            Assert.Null(((Tuple<DecisionViewModel, List<DecisionViewModel>>)viewResult.Model).Item2);
        }

        [Fact]
        public async Task ReadDecisionCorrectTest()
        {
            _mapper.Setup(m => m.Map<IEnumerable<DecisionWrapper>>(It.IsAny<IEnumerable<DecisionWrapperDTO>>()))
                .Returns(new List<DecisionWrapper>());
            _mapper.Setup(m => m.Map<IEnumerable<Organization>>(It.IsAny<IEnumerable<OrganizationDTO>>()))
                .Returns(new List<Organization>());
            _mapper.Setup(m => m.Map<IEnumerable<DecisionWrapper>>(It.IsAny<IEnumerable<DecisionWrapperDTO>>()))
                .Returns(new List<DecisionWrapper>());
            _mapper.Setup(m => m.Map<IEnumerable<DecisionTarget>>(It.IsAny<IEnumerable<DecisionTargetDTO>>()))
                .Returns(new List<DecisionTarget>());
            DecisionController documentationController = CreateDocumentationController;

            var result = await documentationController.ReadDecision();

            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(42, false)]
        [InlineData(-1, false)]
        public async Task DeleteDecisionTest(int id, bool expected)
        {
            Decision decision;
            try
            {
                decision = CreateDecisionsQueryable().First(x => x.ID == id);
            }
            catch
            {
                decision = null;
            }
            _decisionService.Setup(d => d.DeleteDecisionAsync(It.IsAny<int>()))
                .ReturnsAsync(() => decision != null);
            DecisionController documentationController = CreateDocumentationController;

            JsonResult jsonResult = await documentationController.DeleteDecision(id);
            bool actual = jsonResult.Value.ToString().Contains("True");

            Assert.Equal(expected, actual);
        }

        private static IQueryable<DecisionDTO> CreateDecisionsDtoQueryable()
        {
            List<DecisionDTO> decisions = new List<DecisionDTO>();
            for (int i = 0; i < 10; i++)
            {
                decisions.Add(new DecisionDTO
                {
                    ID = i,
                    Name = "Name " + i
                });
            }
            return decisions.AsQueryable();
        }

        private static IEnumerable<Decision> CreateDecisionsQueryable()
        {
            List<Decision> decisions = new List<Decision>();
            for (int i = 0; i < 10; i++)
            {
                decisions.Add(new Decision
                {
                    ID = i,
                    Name = "Name " + i
                });
            }
            return decisions.AsEnumerable();
        }

        private static DecisionViewModel CreateDecisionViewModel(int decisionTargetId = 1, bool haveFile = false) => new DecisionViewModel
        {
            DecisionWrapper = new DecisionWrapper
            {
                Decision = new Decision
                {
                    ID = 1,
                    Name = "Test Decision",
                    DecisionStatusType = DecisionStatusType.InReview,
                    DecisionTarget = new DecisionTarget { ID = decisionTargetId, TargetName = "Test Decision target" },
                    Description = "Test Decision Description",
                    Organization = new Organization { ID = 1, OrganizationName = "Test Decision Organization" },
                    Date = DateTime.Now,
                    HaveFile = haveFile
                }
            }
        };

        private DecisionController CreateDocumentationController =>
            new DecisionController(null, _decisionService.Object, _mapper.Object, _loggerService.Object);
    }
}