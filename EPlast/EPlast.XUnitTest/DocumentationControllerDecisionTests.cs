using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Xunit;
using Organization = EPlast.Models.Organization;

namespace EPlast.XUnitTest
{
    public class DocumentationControllerDecisionTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDecisionService> _decisionService;

        public DocumentationControllerDecisionTests()
        {
            _mapper = new Mock<IMapper>();
            _decisionService = new Mock<IDecisionService>();
        }

        [Theory]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(42, false)]
        [InlineData(21, false)]
        public void GetDecisionByIdTest(int id, bool expected)
        {
            _decisionService.Setup(d => d.GetDecision(It.IsAny<int>()))
                .Returns(() => CreateDecisionsDtoQueryable().First(x => x.ID == id));
            _mapper.Setup(d => d.Map<Decision>(It.IsAny<DecisionDTO>()))
                .Returns(() => CreateDecisionsQueryable().First(x => x.ID == id));
            DocumentationController documentationController = CreateDocumentationController;

            JsonResult jsonResult = documentationController.GetDecision(id);
            bool actual = jsonResult.Value.ToString().Contains("True");

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ChangeDecisionTest(bool expected)
        {
            DecisionViewModel decisionViewModel = CreateDecisionViewModel();
            _mapper.Setup(m => m.Map<DecisionDTO>(It.IsAny<Decision>()))
                 .Returns(new DecisionDTO());
            _decisionService.Setup(d => d.ChangeDecision(It.IsAny<DecisionDTO>()))
                .Returns(() => expected);
            DocumentationController documentationController = CreateDocumentationController;

            JsonResult jsonResult = documentationController.ChangeDecision(decisionViewModel);
            bool actual = jsonResult.Value.ToString().Contains("True");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task SaveDecisionWithNullDecisionViewMode()
        {

            DecisionViewModel decisionViewModel = null;
            DocumentationController documentationController = CreateDocumentationController;

            JsonResult jsonResult = await documentationController.SaveDecision(decisionViewModel);
            bool actual = jsonResult.Value.ToString().Contains("False");

            Assert.True(actual);
        }

        [Fact]
        public async Task SaveDecisionWithIncorrectFileTest()
        {
            DecisionViewModel decisionViewModel = CreateDecisionViewModel();
            decisionViewModel.DecisionWrapper.File = new FormFile(null, 1234, 11241234, "fdd", "dfsdf");
            DocumentationController documentationController = CreateDocumentationController;

            JsonResult jsonResult = await documentationController.SaveDecision(decisionViewModel);
            bool actual = jsonResult.Value.ToString().Contains("file length > 10485760");

            Assert.True(actual);
        }

        [Fact]
        public async Task SaveDecisionCorrectTest()
        {
            _mapper.Setup(m => m.Map<DecisionWrapperDTO>(It.IsAny<DecisionWrapper>()))
                .Returns(new DecisionWrapperDTO());
            _decisionService.Setup(d => d.SaveDecisionAsync(It.IsAny<DecisionWrapperDTO>()))
                .Returns(Task.FromResult(true));
            DecisionViewModel decisionViewModel = CreateDecisionViewModel();
            DocumentationController documentationController = CreateDocumentationController;

            JsonResult jsonResult = await documentationController.SaveDecision(decisionViewModel);
            bool actual = jsonResult.Value.ToString().Contains("True");

            Assert.True(actual);
        }

        [Fact]
        public void CreateDecisionCorrectTest()
        {
            _mapper.Setup(m => m.Map<List<Organization>>(It.IsAny<List<OrganizationDTO>>()))
                .Returns(new List<Organization>());
            _mapper.Setup(m => m.Map<List<DecisionWrapper>>(It.IsAny<List<DecisionWrapperDTO>>()))
                .Returns(new List<DecisionWrapper>());
            _mapper.Setup(m => m.Map<List<DecisionTarget>>(It.IsAny<List<DecisionTargetDTO>>()))
                .Returns(new List<DecisionTarget>());
            _decisionService.Setup(d => d.GetDecisionStatusTypes()).Returns(new List<SelectListItem>());
            DocumentationController documentationController = CreateDocumentationController;

            DecisionViewModel decisionViewModel = documentationController.CreateDecision();

            Assert.IsType<DecisionViewModel>(decisionViewModel);

        }
        [Fact]
        public void CreateDecisionFailTest()
        {
            _mapper.Setup(m => m.Map<List<Organization>>(It.IsAny<List<OrganizationDTO>>()))
                .Returns(() => null);
            _mapper.Setup(m => m.Map<List<DecisionWrapper>>(It.IsAny<List<DecisionWrapperDTO>>()))
                .Returns(new List<DecisionWrapper>());
            _mapper.Setup(m => m.Map<List<DecisionTarget>>(It.IsAny<List<DecisionTargetDTO>>()))
                .Returns(new List<DecisionTarget>());
            _decisionService.Setup(d => d.GetDecisionStatusTypes()).Returns(new List<SelectListItem>());
            DocumentationController documentationController = CreateDocumentationController;

            DecisionViewModel decisionViewModel = documentationController.CreateDecision();

            Assert.True(decisionViewModel == null);

        }
        [Fact]
        public void ReadDecisionFailTest()
        {
            _mapper.Setup(m => m.Map<List<DecisionWrapper>>(It.IsAny<List<DecisionWrapperDTO>>()))
                .Returns(() => null);
            DocumentationController documentationController = CreateDocumentationController;

            var result = documentationController.ReadDecision();
            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("HandleError", viewResult.ActionName);
            Assert.Equal("Error", viewResult.ControllerName);
        }
        [Fact]
        public void ReadDecisionCorrectTest()
        {
            _mapper.Setup(m => m.Map<List<DecisionWrapper>>(It.IsAny<List<DecisionWrapperDTO>>()))
                .Returns(new List<DecisionWrapper>());
            _mapper.Setup(m => m.Map<List<Organization>>(It.IsAny<List<OrganizationDTO>>()))
                .Returns(new List<Organization>());
            _mapper.Setup(m => m.Map<List<DecisionWrapper>>(It.IsAny<List<DecisionWrapperDTO>>()))
                .Returns(new List<DecisionWrapper>());
            _mapper.Setup(m => m.Map<List<DecisionTarget>>(It.IsAny<List<DecisionTargetDTO>>()))
                .Returns(new List<DecisionTarget>());
            DocumentationController documentationController = CreateDocumentationController;

            var result = documentationController.ReadDecision();

            Assert.IsType<ViewResult>(result);

        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(42, false)]
        [InlineData(-1, false)]
        public void DeleteDecisionTest(int id, bool expected)
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
            _decisionService.Setup(d => d.DeleteDecision(It.IsAny<int>()))
                .Returns(() => decision != null);
            DocumentationController documentationController = CreateDocumentationController;

            JsonResult jsonResult = documentationController.DeleteDecision(id);
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
        private DocumentationController CreateDocumentationController =>
            new DocumentationController(null, null, null, null, null, null, null, _decisionService.Object, _mapper.Object);
    }
}