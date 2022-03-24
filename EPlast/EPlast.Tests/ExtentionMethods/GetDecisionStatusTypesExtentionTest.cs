using EPlast.BLL;
using EPlast.BLL.ExtensionMethods;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace EPlast.Tests.ExtentionMethods
{
    public class GetDecisionStatusTypesExtentionTest
    {
        private Mock<IDecisionVmInitializer> _mockGetDecisionStatusTypesExtention;
        [SetUp]
        public void SetUp()
        {
            _mockGetDecisionStatusTypesExtention = new Mock<IDecisionVmInitializer>();
        }

        [Test]
        public void GetDecisionStatusTypesTest()
        {
            //Arrange
            _mockGetDecisionStatusTypesExtention.Setup(vm => vm.GetDecesionStatusTypes()).Returns(new List<SelectListItem>());

            //Act
            var s = new GetDecisionStatusTypesExtention();
            var actualReturn = s.GetDecesionStatusTypes();

            //Assert
            _mockGetDecisionStatusTypesExtention.Verify();
            Assert.IsInstanceOf<List<SelectListItem>>(actualReturn);
        }

    }
 }
