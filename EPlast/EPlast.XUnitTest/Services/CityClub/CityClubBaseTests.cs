using EPlast.BLL.Services.CityClub;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using System.Runtime.InteropServices;

namespace EPlast.XUnitTest.Services.CityClub
{
    public class CityClubBaseTests
    {
        [Fact]
        public void GetChangedPhotoTest_FirstIf_ThrowsExternalException()
        {
            // Arrange
            ICityClubBase cityClubBase = new CityClubBase();

            const string anyBase64ImageString = "iVBORw0KGgoAAAANSUhEUgAAAA0AAA" +
                "AOCAIAAAB7HQGFAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZ" +
                "cwAADsMAAA7DAcdvqGQAAAAcSURBVChTY/z//z8DEYAJShMCo+qwg+GhjoEBAK" +
                "SgAxnWUvb6AAAAAElFTkSuQmCC";

            var byteBufferConvertedFromBase64 = Convert.FromBase64String(anyBase64ImageString);
            const int anyBaseStreamOffset = 0;
            const string anyFormFileName = "TestName";
            const string anyFileName = "TestFileName";
            IFormFile fileToPassToMethod = new FormFile(new MemoryStream(byteBufferConvertedFromBase64), anyBaseStreamOffset, byteBufferConvertedFromBase64.Length, anyFormFileName, anyFileName);

            string guidToPassToMethod = Guid.NewGuid().ToString();
            const string anyOldImageName = "OldImageName";
            const string anyEnvWebPath = "EnvWebPath";
            const string anyBaseImagePath = "images\\Cities";

            // Act 
            Action actionToAssert = () => cityClubBase.GetChangedPhoto(anyBaseImagePath, fileToPassToMethod, anyOldImageName, anyEnvWebPath, guidToPassToMethod);

            // Assert
            ExternalException exception = Assert.Throws<ExternalException>(actionToAssert);
        }
    }
}
