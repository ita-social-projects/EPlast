using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.AboutBase;
using EPlast.BLL.Interfaces.AboutBase;
using EPlast.BLL.Services.AboutBase;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.AboutBase;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Services.AboutBase
{
    public class PicturesManagerTests
    {
        private readonly Mock<IAboutBaseWrapper> _aboutBaseWrapper;

        public PicturesManagerTests()
        {
            var userStore = new Mock<IUserStore<User>>();
            _aboutBaseWrapper = new Mock<IAboutBaseWrapper>();
        }

        [Fact]
        public async Task GetPicturesTestAsync()
        {
            //Arrange
            int subsectionId = 3;
            _aboutBaseWrapper.Setup(x => x.AboutBasePicturesManager.GetPicturesInBase64(It.IsAny<int>()))
                .ReturnsAsync(new List<SubsectionPicturesDto>());
            //Act
            var picturesManager = new PicturesManager(_aboutBaseWrapper.Object);
            var methodResult = await picturesManager.GetPicturesAsync(subsectionId);
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<SubsectionPicturesDto>>(methodResult);
        }
        [Fact]
        public async Task FillEventGalleryTestAsync()
        {
            //Arrange
            int subsectionId = 3;
            _aboutBaseWrapper.Setup(x => x.AboutBasePicturesManager.AddPicturesAsync(It.IsAny<int>(), It.IsAny<IList<IFormFile>>()))
                .ReturnsAsync(new List<SubsectionPicturesDto>());
            //Act
            var picturesManager = new PicturesManager(_aboutBaseWrapper.Object);
            var methodResult = await picturesManager.FillSubsectionPicturesAsync(subsectionId, new List<IFormFile>());
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<SubsectionPicturesDto>>(methodResult);
        }
        [Fact]
        public async Task DeletePictureTestAsync()
        {
            //Arrange
            int subsectionId = 3;
            _aboutBaseWrapper.Setup(x => x.AboutBasePicturesManager.DeletePictureAsync(It.IsAny<int>()))
                .ReturnsAsync(StatusCodes.Status200OK);
            //Act
            var picturesManager = new PicturesManager(_aboutBaseWrapper.Object);
            var methodResult = await picturesManager.DeletePictureAsync(subsectionId);
            //Assert
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }
    }
}
