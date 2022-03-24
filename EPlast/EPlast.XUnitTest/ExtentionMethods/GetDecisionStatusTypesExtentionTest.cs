using EPlast.BLL;
using EPlast.BLL.ExtensionMethods;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace EPlast.XUnitTest.ExtentionMethods
{
    public class GetDecisionStatusTypesExtentionTest
    {
        private Mock<IDecisionVmInitializer> _mockGetDecisionStatusTypesExtention;
        
        public GetDecisionStatusTypesExtentionTest()
        {
            _mockGetDecisionStatusTypesExtention = new Mock<IDecisionVmInitializer>();
        }


        [Fact]
        public void GetDecisionStatusTypesTest()
        {
            _mockGetDecisionStatusTypesExtention.Setup(vm => vm.GetDecesionStatusTypes()).Returns(new List<SelectListItem>());

            var s = new GetDecisionStatusTypesExtention();
            var actualReturn = s.GetDecesionStatusTypes();

            _mockGetDecisionStatusTypesExtention.Verify();
            Assert.IsType<List<SelectListItem>>(actualReturn);
        }
    }
}
