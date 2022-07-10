using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EPlast.BLL.DTO.AboutBase;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Services.AboutBase;
using EPlast.DataAccess.Entities.AboutBase;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;

namespace EPlast.XUnitTest.Services.AboutBase
{
    public class AboutBasePicturesManagerTests
    {
        private readonly Mock<IRepositoryWrapper> _repoWrapper;
        private readonly Mock<IAboutBaseBlobStorageRepository> _aboutBaseBlobStorage;

        public AboutBasePicturesManagerTests()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _aboutBaseBlobStorage = new Mock<IAboutBaseBlobStorageRepository>();
        }

        [Fact]
        public async Task DeletePictureSuccessTest()
        {
            //Arrange
            int subsectionId = 145;
            _repoWrapper.Setup(x =>
                    x.Pictures.GetFirstAsync(It.IsAny<Expression<Func<Pictures, bool>>>(), null))
                .ReturnsAsync(new Pictures { ID = 2, PictureFileName = "picture.jpj" });
            _aboutBaseBlobStorage.Setup(x => x.DeleteBlobAsync(It.IsAny<string>()));
            //Act
            var aboutBasePicturesManager = new AboutBasePicturesManager(
                _repoWrapper.Object,
                _aboutBaseBlobStorage.Object
            );
            var methodResult = await aboutBasePicturesManager.DeletePictureAsync(subsectionId);
            //Assert
            _repoWrapper.Verify(r => r.Pictures.Delete(It.IsAny<Pictures>()), Times.Once());
            _repoWrapper.Verify(r => r.SaveAsync(), Times.Once());
            Assert.Equal(StatusCodes.Status200OK, methodResult);
        }

        [Fact]
        public async Task DeletePictureFailTest()
        {
            //Arrange
            int subsectionId = 145;
            _repoWrapper.Setup(x =>
                    x.Pictures.GetFirstAsync(It.IsAny<Expression<Func<Pictures, bool>>>(), null))
                .ThrowsAsync(new Exception());
            _aboutBaseBlobStorage.Setup(x => x.DeleteBlobAsync(It.IsAny<string>()));
            //Act
            var aboutBasePicturesManager = new AboutBasePicturesManager(
                _repoWrapper.Object,
                _aboutBaseBlobStorage.Object
            );
            var methodResult = await aboutBasePicturesManager.DeletePictureAsync(subsectionId);
            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, methodResult);
        }

        [Fact]
        public async Task GetPicturesInBase64Test()
        {
            //Arrange
            int subsectionId = 145;
            var picture = "StringInBase64";
            _repoWrapper.Setup(x =>
                    x.SubsectionPictures.GetAllAsync(It.IsAny<Expression<Func<SubsectionPictures, bool>>>(), It.IsAny<Func<IQueryable<SubsectionPictures>, IIncludableQueryable<SubsectionPictures, object>>>()))
                .ReturnsAsync(GetSubsectionsPictures());
            _aboutBaseBlobStorage.Setup(x => x.GetBlobBase64Async(It.IsAny<string>()))
                .ReturnsAsync(picture);
            //Act
            var aboutBasePicturesManager = new AboutBasePicturesManager(
                _repoWrapper.Object,
                _aboutBaseBlobStorage.Object
            );
            var methodResult = await aboutBasePicturesManager.GetPicturesInBase64(subsectionId);
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<SubsectionPicturesDto>>(methodResult);
            Assert.Equal(GetSubsectionsPictures().Count(), methodResult.ToList().Count);
        }

        [Fact]
        public async Task FillEventGalleryTest()
        {
            //Arrange
            int subsectionId = 145;
            var picture = "StringInBase64";
            _repoWrapper.Setup(x => x.Pictures.CreateAsync((It.IsAny<Pictures>())));
            _repoWrapper.Setup(x => x.SubsectionPictures.CreateAsync((It.IsAny<SubsectionPictures>())));
            _aboutBaseBlobStorage.Setup(x => x.UploadBlobAsync(It.IsAny<IFormFile>(), It.IsAny<string>()));
            _aboutBaseBlobStorage.Setup(x => x.GetBlobBase64Async(It.IsAny<string>()))
                .ReturnsAsync(picture);
            //Act  
            var aboutBasePicturesManager = new AboutBasePicturesManager(
                _repoWrapper.Object,
                _aboutBaseBlobStorage.Object
            );
            var methodResult = await aboutBasePicturesManager.AddPicturesAsync(subsectionId, FakeFiles());
            //Assert
            Assert.NotNull(methodResult);
            Assert.IsType<List<SubsectionPicturesDto>>(methodResult);
        }

        public static IList<IFormFile> FakeFiles()
        {
            var fileMock = new Mock<IFormFile>();
            Icon icon1 = new Icon(SystemIcons.Exclamation, 40, 40);
            var content = icon1.ToBitmap();
            var fileName = "picture.png";
            var ms = new MemoryStream();
            content.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            var arr = new[] { fileMock.Object, fileMock.Object };
            return arr;
        }

        public IQueryable<SubsectionPictures> GetSubsectionsPictures()
        {
            var subsectionsPictures = new List<SubsectionPictures>
            {
                new SubsectionPictures{
                    SubsectionID=145,
                    PictureID=145,
                    Pictures=new Pictures{ID=145,PictureFileName= "FileName"}
                }
            }.AsQueryable();
            return subsectionsPictures;
        }
    }
}
