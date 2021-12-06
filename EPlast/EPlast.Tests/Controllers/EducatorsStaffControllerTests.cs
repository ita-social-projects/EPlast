using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.BLL.Interfaces.Logging;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities.EducatorsStaff;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    internal class EducatorsStaffControllerTests
    {
        private Mock<ILoggerService<EducatorsStaffController>> _loggerService;
        private Mock<IEducatorsStaffService> _educatorsStaffService;
        private Mock<IEducatorsStaffTypesService> _educatorsStaffTypesService;

        private EducatorsStaffController _educatorsStaffController;

        [SetUp]
        public void SetUp()
        {
            _loggerService = new Mock<ILoggerService<EducatorsStaffController>>();
            _educatorsStaffService = new Mock<IEducatorsStaffService>();
            _educatorsStaffTypesService = new Mock<IEducatorsStaffTypesService>();

            _educatorsStaffController = new EducatorsStaffController(
                _loggerService.Object,
                _educatorsStaffService.Object,
                _educatorsStaffTypesService.Object
                );
        }

        [Test]
        public async Task Create_Kadra_CallCreateKadra()
        {
            // Arange
            var dto = new EducatorsStaffDTO
            {
                ID = 1
            };

            _educatorsStaffService.Setup(x => x.CreateKadra(dto)).ReturnsAsync(dto);

            // Act
            await _educatorsStaffController.CreateKadra(dto);

            // Assert
            _educatorsStaffService.Verify(x => x.CreateKadra(dto));
        }

        [Test]
        public async Task Create_Kadra_ReturnsOkStatus()
        {
            // Arange
            var inputModel = new EducatorsStaffDTO
            {
                ID = 1
            };

            var outputModel = new EducatorsStaffDTO
            {
                ID = 2
            };

            _educatorsStaffService.Setup(x => x.CreateKadra(inputModel)).ReturnsAsync(outputModel);

            // Act

            var result = await _educatorsStaffController.CreateKadra(inputModel);

            // Assert
            _educatorsStaffService.Verify();
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Create_Kadra_ReturnsRightEducatorsStaffDTO()
        {
            // Arange
            var inputModel = new EducatorsStaffDTO
            {
                ID = 1
            };

            var outputModel = new EducatorsStaffDTO
            {
                ID = 2
            };

            _educatorsStaffService.Setup(x => x.CreateKadra(inputModel)).ReturnsAsync(outputModel);

            // Act

            var result = await _educatorsStaffController.CreateKadra(inputModel);
            var actualResult = (result as ObjectResult).Value as EducatorsStaffDTO;

            // Assert
            _educatorsStaffService.Verify();
            Assert.AreEqual(outputModel.ID, actualResult.ID);
        }

        [Test]
        public async Task Remove_Kadra_CallsDeleteKadra()
        {
            // Arange
            int kadraId = 1;
            _educatorsStaffService.Setup(x => x.DeleteKadra(kadraId));

            // Act
            await _educatorsStaffController.Remove(kadraId);

            // Assert
            _educatorsStaffService.Verify(x => x.DeleteKadra(kadraId));
        }

        [Test]
        public async Task Remove_Kadra_ReturnsStatusCodes200()
        {
            // Arange
            var expected = StatusCodes.Status200OK;

            // Act
            var result = await _educatorsStaffController.Remove(It.IsAny<int>());
            var actual = result as StatusCodeResult;

            // Assert
            Assert.AreEqual(expected, actual.StatusCode);
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
        }

        [Test]
        public async Task Update_Kadra_ReturnsStatusCodes200()
        {
            // Arange

            var expected = StatusCodes.Status200OK;

            // Act
            var result = await _educatorsStaffController.Update(It.IsAny<EducatorsStaffDTO>());
            var actual = result as StatusCodeResult;

            // Assert
            Assert.AreEqual(expected, actual.StatusCode);
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
        }

        [Test]
        public async Task Update_Kadra_CallUpdateKadra()
        {
            // Arange
            var dto = new EducatorsStaffDTO
            {
                ID = 1
            };
            _educatorsStaffService.Setup(x => x.UpdateKadra(dto));

            // Act
            await _educatorsStaffController.Update(dto);
            
            // Assert
            _educatorsStaffService.Verify(x => x.UpdateKadra(It.IsAny<EducatorsStaffDTO>()));
        }

        [Test]
        public async Task GetUsersKVs_NullUserIdString_ReturnsNotFound()
        {
            // Arange
            string nullString = null;

            // Act
            var result = await _educatorsStaffController.GetUsersKVs(nullString);

            // Assert
            _loggerService.Verify(x => x.LogError(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetUsersKVs_User_ReturnsOkObjectResult()
        {
            // Arange
            string id = "1";
            _educatorsStaffService.
                Setup(x => x.GetKVsOfGivenUser(id)).
                ReturnsAsync(CreateFakeEducatorsStaffDTO);

            // Act
            var result = await _educatorsStaffController.GetUsersKVs(id);

            // Assert
            _educatorsStaffService.Verify();
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUsersKVs_NullUser_ReturnsNotFoundResult()
        {
            string kadras = null;
            var expected = StatusCodes.Status404NotFound;

            // Act
            var result = await _educatorsStaffController.GetUsersKVs(kadras);
            var actual = result as StatusCodeResult;

            // Assert
            Assert.AreEqual(expected, actual.StatusCode);
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
        }

        [Test]
        public async Task GetUsersKVs_Status404NotFound()
        {
            // Arange
            string nullString = "1";

            // Act
            _educatorsStaffService.
             Setup(x => x.GetKVsOfGivenUser(nullString)).
             ReturnsAsync((List<EducatorsStaffDTO>)null);

            var expected = StatusCodes.Status404NotFound;
            var result = await _educatorsStaffController.GetUsersKVs(nullString);
            var actual = result as StatusCodeResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        [Test]
        public async Task GetKVsWithType_User_CallsGetKVsWithKVType()
        {
            // Arange
            int id = 1;
            _educatorsStaffService.
                Setup(x => x.GetKVsWithKVType(id)).
                ReturnsAsync(CreateFakeEducatorsStaffDTO);

            // Act
            var result = await _educatorsStaffController.GetKVsWithType(id);

            // Assert
            _educatorsStaffService.Verify(x => x.GetKVsWithKVType(It.IsAny<int>()));
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetKVTypes_Types_ReturnsOkObjectResult()
        {
            // Arange
            _educatorsStaffTypesService.
                Setup(x => x.GetAllKVTypesAsync());

            // Act
            var result = await _educatorsStaffController.GetKVTypes();

            // Assert
            _educatorsStaffTypesService.Verify(x => x.GetAllKVTypesAsync());
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetKVTypes_NullTypes_ReturnsNotFoundResult()
        {
            _educatorsStaffTypesService.Setup(x => x.GetAllKVTypesAsync()).ReturnsAsync(() => null);
            var expected = StatusCodes.Status404NotFound;

            // Act
            var result = await _educatorsStaffController.GetKVTypes();
            var actual = result as StatusCodeResult;

            // Assert
            _educatorsStaffTypesService.Verify();
            Assert.AreEqual(expected, actual.StatusCode);
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
        }

        [Test]
        public async Task GetAllKVs_KVs_ReturnsOkObjectResult()
        {
            // Arange
            _educatorsStaffService.
                Setup(x => x.GetAllKVsAsync());

            // Act
            var result = await _educatorsStaffController.GetAllKVs();

            // Assert
            _educatorsStaffService.Verify(x => x.GetAllKVsAsync());
            Assert.NotNull((result as ObjectResult).Value);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAllKVs_NullKVs_ReturnsNotFoundResult()
        {
            _educatorsStaffService.Setup(x => x.GetAllKVsAsync()).ReturnsAsync(() => null);
            var expected = StatusCodes.Status404NotFound;

            // Act
            var result = await _educatorsStaffController.GetAllKVs();
            var actual = result as StatusCodeResult;

            // Assert
            _educatorsStaffService.Verify();
            Assert.AreEqual(expected, actual.StatusCode);
            Assert.NotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
        }

        [Test]
        public async Task GetUserStaff_CallsDoesUserHaveSuchStaff()
        {
            // Arange
            string userId = "1";
            int kadraId = 1;
            _educatorsStaffService.
                Setup(x => x.DoesUserHaveSuchStaff(userId, kadraId)).
                ReturnsAsync(It.IsAny<bool>);

            // Act
            await _educatorsStaffController.GetUserStaff(userId, kadraId);

            // Assert
            _educatorsStaffService.Verify(x => x.DoesUserHaveSuchStaff(It.IsAny<string>(), It.IsAny<int>()));
        }

        [Test]
        public async Task GetStaffWithRegisternumber_CallsStaffWithRegisternumberExists()
        {
            // Arange
            int numberInRegister = 1;
            _educatorsStaffService.
                Setup(x => x.StaffWithRegisternumberExists(numberInRegister)).
                ReturnsAsync(It.IsAny<bool>);

            // Act
            await _educatorsStaffController.GetStaffWithRegisternumber(numberInRegister);

            // Assert
            _educatorsStaffService.Verify(x => x.StaffWithRegisternumberExists(It.IsAny<int>()));
        }

        [Test]
        public async Task GetUserStaffEdit_CallsUserHasSuchStaffEdit()
        {
            // Arange
            string userId = "1";
            int kadraId = 1;
            _educatorsStaffService.
                Setup(x => x.UserHasSuchStaffEdit(userId, kadraId)).
                ReturnsAsync(It.IsAny<bool>);

            // Act
            await _educatorsStaffController.GetUserStaffEdit(userId, kadraId);

            // Assert
            _educatorsStaffService.Verify(x => x.UserHasSuchStaffEdit(It.IsAny<string>(), It.IsAny<int>()));
        }

        [Test]
        public async Task GetStaffWithRegisternumberEdit_CallsStaffWithRegisternumberExistsEdit()
        {
            // Arange
            int kadraId = 1;
            int numberInRegister = 1;
            _educatorsStaffService.
                Setup(x => x.StaffWithRegisternumberExistsEdit(kadraId, numberInRegister)).
                ReturnsAsync(It.IsAny<bool>);

            // Act
            await _educatorsStaffController.GetStaffWithRegisternumberEdit(kadraId, numberInRegister);

            // Assert
            _educatorsStaffService.Verify(x => x.StaffWithRegisternumberExistsEdit(It.IsAny<int>(), It.IsAny<int>()));
        }

        [Test]
        public async Task GetUserByEduStaff_CallsGetUserByEduStaff()
        {
            // Arange
            int eduStaffId = 1;
            _educatorsStaffService.
                Setup(x => x.GetUserByEduStaff(eduStaffId)).
                ReturnsAsync(It.IsAny<string>);

            // Act
            await _educatorsStaffController.GetUserByEduStaff(eduStaffId);

            // Assert
            _educatorsStaffService.Verify(x => x.GetUserByEduStaff(It.IsAny<int>()));
        }

        [TestCase(1)]
        public async Task GetEduStaffById_ReturnsOkObjectResult_Test(int kadraId)
        {
            // Arrange
            _educatorsStaffService.Setup(x => x.GetKadraById(It.IsAny<int>()))
                .ReturnsAsync(new EducatorsStaffDTO());

            // Act
            var result = await _educatorsStaffController.GetEduStaffById(kadraId);

            // Assert
            Assert.NotNull(result);
            _educatorsStaffService.Verify(x => x.GetKadraById(It.IsAny<int>()), Times.AtLeastOnce());
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetEducatorsStaffForTable_ReturnsOkObjectResult()
        {
            //Arrange
            _educatorsStaffService
                .Setup(x => x.GetEducatorsStaffTableObject(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(new List<EducatorsStaffTableObject>());

            //Act
            var result = _educatorsStaffController.GetEducatorsStaffForTable(It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<int>(), It.IsAny<int>());
            var resultValue = (result as OkObjectResult)?.Value;

            //Assert
            _educatorsStaffService.Verify();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(resultValue);
            Assert.IsInstanceOf<List<EducatorsStaffTableObject>>(resultValue);
        }

        private List<EducatorsStaffDTO> CreateFakeEducatorsStaffDTO()
            => new List<EducatorsStaffDTO>()
            {
                new EducatorsStaffDTO()
                {
                    ID = 1
                },
                 new EducatorsStaffDTO()
                {
                    ID = 2
                },
            };
    }
}
