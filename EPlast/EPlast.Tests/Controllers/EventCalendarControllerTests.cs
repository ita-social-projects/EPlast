using EPlast.BLL.DTO.EventCalendar;
using EPlast.BLL.Interfaces.EventCalendar;
using EPlast.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Tests.Controllers
{
    [TestFixture]
    public class EventsCalendarControllerTests
    {
        Mock<IEventCalendarService> _mockCalendarService;
        EventsCalendarController _eventsCalendarController;
        int expectedCount;

        [SetUp]
        public void SetUp()
        {
            _mockCalendarService = new Mock<IEventCalendarService>();

            _eventsCalendarController = new EventsCalendarController(_mockCalendarService.Object);

            expectedCount = GetAllEventsTest().Count;
        }

        [Test]
        public async Task GetActions_ReturnsOkObjectResult()
        {
            //Arrange
            _mockCalendarService
                .Setup(x => x.GetAllActions())
                .ReturnsAsync(GetAllEventsTest());

            //Act
            var result = await _eventsCalendarController.GetActions();

            //Assert
            _mockCalendarService.Verify();
            Assert.IsNotNull(result);
            Assert.AreEqual(((result as ObjectResult).Value as IEnumerable<EventCalendarInfoDTO>).Count(), expectedCount);
            Assert.IsInstanceOf<ObjectResult>(result);
        }

        [Test]
        public async Task GetAction_ReturnsExpectedCount()
        {
            //Arrange
            _mockCalendarService
               .Setup(x => x.GetAllActions())
               .ReturnsAsync(GetAllEventsTest());

            //Act 
            var resultActionCount = ((
                await _eventsCalendarController.GetActions() as ObjectResult)
                .Value as List<EventCalendarInfoDTO>)
                .Count;
            
            //Assert
            _mockCalendarService.Verify();
            Assert.AreEqual(expectedCount, resultActionCount);
        }

        [Test]
        public async Task GetEducations_ReturnsOkObjectResult()
        {
            //Arrange
            _mockCalendarService
                .Setup(x => x.GetAllEducations())
                .ReturnsAsync(GetAllEventsTest());

            //Act
            var result = await _eventsCalendarController.GetEducations();

            //Assert
            _mockCalendarService.Verify();
            Assert.AreEqual(((result as ObjectResult).Value as IEnumerable<EventCalendarInfoDTO>).Count(), expectedCount);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetEducation_ReturnsExpectedCount()
        {
            //Arrange
            _mockCalendarService
                .Setup(x => x.GetAllEducations())
                .ReturnsAsync(GetAllEventsTest());

            //Act
            var resultEducationsCount = ((
              await _eventsCalendarController.GetEducations() as ObjectResult)
              .Value as List<EventCalendarInfoDTO>)
              .Count;

            //Assert
            _mockCalendarService.Verify();
            Assert.AreEqual(expectedCount, resultEducationsCount);
        }

        [Test]
        public async Task GetCamps_ReturnOkObjectResult()
        {
            //Arrange
            _mockCalendarService
                .Setup(x => x.GetAllCamps())
                .ReturnsAsync(GetAllEventsTest());

            //Act
            var result = await _eventsCalendarController.GetCamps();

            //Assert
            _mockCalendarService.Verify();
            Assert.AreEqual(((result as ObjectResult).Value as IEnumerable<EventCalendarInfoDTO>).Count(), expectedCount);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetCamps_ReturnsExpectedCount()
        {
            //Arrange
            _mockCalendarService
                .Setup(x => x.GetAllCamps())
                .ReturnsAsync(GetAllEventsTest());

            //Act 
            var resultCamplsCount = ((
                await _eventsCalendarController.GetCamps() as ObjectResult)
                .Value as List<EventCalendarInfoDTO>)
                .Count;

            //Assert
            _mockCalendarService.Verify();
            Assert.AreEqual(expectedCount, resultCamplsCount);
        }

        //Fakes
        private List<EventCalendarInfoDTO> GetAllEventsTest()
        {
            List<EventCalendarInfoDTO> events = new List<EventCalendarInfoDTO>() {
                new EventCalendarInfoDTO(),
                new EventCalendarInfoDTO()
            };
            return events;
        }

    }
}
