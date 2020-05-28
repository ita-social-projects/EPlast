//using EPlast.BussinessLayer;
//using EPlast.Controllers;
//using EPlast.DataAccess.Entities;
//using EPlast.DataAccess.Repositories;
//using EPlast.Models.ViewModelInitializations.Interfaces;
//using EPlast.ViewModels;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity;
//using Moq;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using EPlast.Wrapper;
//using Xunit;
//using Microsoft.AspNetCore.Http;
//using System.IO;
//using System;
//using Microsoft.AspNetCore.Mvc;

//namespace EPlast.XUnitTest
//{
//    public class DocumentationControllerTests
//    {
//        private static DocumentationController CreateDocumentationController()
//        {
//            var repository = new Mock<IRepositoryWrapper>();
//            var store = new Mock<IUserStore<User>>();
//            var userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
//            var decisionVmInitializer = new Mock<IDecisionVMIitializer>();
//            var pdfService = new Mock<IPDFService>();
//            var hostingEnvironment = new Mock<IHostingEnvironment>();
//            var directoryManager = new Mock<IDirectoryManager>();
//            var fileManager = new Mock<IFileManager>();
//            var fileStreamManager = new Mock<IFileStreamManager>();

//            directoryManager.Setup(dir => dir.Exists(It.IsAny<string>())).Returns(true);
//            directoryManager.Setup(dir => dir.GetFiles(It.IsAny<string>())).Returns(new string[] { "yes", "stronk", "files" });

//            fileStreamManager.Setup(f => f.GenerateFileStreamManager(It.IsAny<string>(), It.IsAny<FileMode>()))
//                .Returns<string, FileMode>((path, mode) => new FileStreamManager());
//            fileStreamManager.Setup(f => f.GetStream()).Returns(new MemoryStream());
//            fileStreamManager.Setup(f => f.CopyToAsync(It.IsAny<MemoryStream>()))
//                .Callback<MemoryStream>(mem => mem.SetLength(5));

//            fileStreamManager.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<Stream>()))
//               .Callback<Stream, Stream>((memFrom, memTo) => memTo.SetLength(5));

//            fileManager.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);

//            repository.Setup(rep => rep.Organization.FindAll()).Returns(GetTestOrganizations());
//            repository.Setup(rep => rep.DecesionTarget.FindAll()).Returns(GetTestDecesionTargets());
//            repository.Setup(rep => rep.Decesion.Attach(new Decesion()));
//            repository.Setup(rep => rep.Decesion.Create(new Decesion()));
//            repository.Setup(rep => rep.Decesion.FindAll()).Returns(GetTestDecesion());
//            repository.Setup(rep => rep.Decesion.Include(x => x.DecesionTarget, x => x.Organization)).Returns(GetTestDecesion());
//            repository.Setup(rep => rep.Save());

//            return new DocumentationController(repository.Object, userManager.Object, null, decisionVmInitializer.Object, pdfService.Object,
//                hostingEnvironment.Object, null, null, directoryManager.Object, fileManager.Object, fileStreamManager.Object, null);
//        }

//        private static IQueryable<Decesion> GetTestDecesion()
//        {
//            var decesion = new List<Decesion>();
//            for (int i = 0; i < 5; ++i)
//            {
//                decesion.Add(CreateFakeDecesion());
//            }
//            return decesion.AsQueryable();
//        }

//        private static IQueryable<Organization> GetTestOrganizations()
//        {
//            var organization = new List<Organization>
//            {
//                 new Organization{ID=1,OrganizationName="Test1"},
//                 new Organization{ID=2,OrganizationName="Test2"},
//                 new Organization{ID=3,OrganizationName="Test3"}
//            }.AsQueryable();
//            return organization;
//        }

//        private static IQueryable<DecesionTarget> GetTestDecesionTargets()
//        {
//            var organization = new List<DecesionTarget>
//            {
//                 new DecesionTarget{ID = 1, TargetName = "First DecesionTarget"},
//                 new DecesionTarget{ID = 2, TargetName = "Second DecesionTarget"},
//                 new DecesionTarget{ID = 3, TargetName = "Third DecesionTarget"}
//            }.AsQueryable();
//            return organization;
//        }

//        [Fact]
//        public void CreateDecesionTest()
//        {
//            var controller = CreateDocumentationController();

//            var result = controller.CreateDecesion();

//            Assert.IsType<DecesionViewModel>(result);
//        }

//        private static Decesion CreateFakeDecesion(bool haveFile = false)
//        {
//            return new Decesion
//            {
//                ID = 1,
//                Name = "Test Decesion",
//                DecesionStatusType = DecesionStatusType.InReview,
//                DecesionTarget = new DecesionTarget { ID = 1, TargetName = "Test Decesion target" },
//                Description = "Test Decesion Description",
//                Organization = new Organization { ID = 1, OrganizationName = "Test Decesion Organization" },
//                Date = DateTime.Now,
//                HaveFile = haveFile
//            };
//        }

//        private static DecesionViewModel CreateDecesionViewModel(int DecesionTargetID = 1, bool haveFile = false) => new DecesionViewModel
//        {
//            Decesion = new Decesion
//            {
//                ID = 1,
//                Name = "Test Decesion",
//                DecesionStatusType = DecesionStatusType.InReview,
//                DecesionTarget = new DecesionTarget { ID = DecesionTargetID, TargetName = "Test Decesion target" },
//                Description = "Test Decesion Description",
//                Organization = new Organization { ID = 1, OrganizationName = "Test Decesion Organization" },
//                Date = DateTime.Now,
//                HaveFile = haveFile
//            }
//        };

//        public static IEnumerable<object[]> TestDecesionViewModelWithoutFile =>
//        new List<object[]> {
//            new object[]{CreateDecesionViewModel(), true },
//            new object[]{CreateDecesionViewModel(DecesionTargetID: 0), true },
//            new object[]{null, false}
//        };

//        [Theory]
//        [MemberData(nameof(TestDecesionViewModelWithoutFile))]
//        public async Task SaveDecesionAsyncTestWithoutFileAsync(DecesionViewModel model, bool expected)
//        {
//            var controller = CreateDocumentationController();

//            var result = await controller.SaveDecesionAsync(model);
//            bool actual = result.Value.ToString().Contains("True") ? true : false;

//            Assert.Equal(expected, actual);
//        }

//        public static IEnumerable<object[]> TestDecesionViewModelWithFile =>
//            new List<object[]> {
//            new object[]{CreateDecesionViewModel(haveFile:true), true },
//            new object[]{CreateDecesionViewModel(haveFile: true), true },
//            new object[]{null, false}
//            };

//        public static IFormFile FakeFile()
//        {
//            var fileMock = new Mock<IFormFile>();
//            var content = "Hello World from a Fake File";
//            var fileName = "test.pdf";
//            var ms = new MemoryStream();
//            var writer = new StreamWriter(ms);
//            writer.Write(content);
//            writer.Flush();
//            ms.Position = 0;
//            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
//            fileMock.Setup(_ => _.FileName).Returns(fileName);
//            fileMock.Setup(_ => _.Length).Returns(ms.Length);

//            return fileMock.Object;
//        }

//        [Theory]
//        [MemberData(nameof(TestDecesionViewModelWithFile))]
//        public async Task SaveDecesionAsyncTestWithFileAsync(DecesionViewModel model, bool expected)
//        {
//            if (model != null)
//                model.File = FakeFile();

//            var controller = CreateDocumentationController();

//            var result = await controller.SaveDecesionAsync(model);

//            bool actual = result.Value.ToString().Contains("True") ? true : false;

//            Assert.Equal(expected, actual);
//        }

//        [Theory]
//        [InlineData("1", "text.txt")]
//        public async Task DownloadTest(string id, string filename)
//        {
//            var controller = CreateDocumentationController();

//            var result = await controller.Download(id, filename);
//            var viewResult = Assert.IsType<FileStreamResult>(result);

//            Assert.Equal(filename, viewResult.FileDownloadName);
//        }

//        [Theory]
//        [InlineData("", "text.txt")]
//        [InlineData("", "")]
//        public async Task DownloadWrongDataTest(string id, string filename)
//        {
//            var controller = CreateDocumentationController();

//            var result = await controller.Download(id, filename);
//            var viewResult = Assert.IsType<ContentResult>(result);

//            Assert.Equal("filename or id not present", viewResult.Content);
//        }

//        [Fact]
//        public void ReadDecesionTest()
//        {
//            var controller = CreateDocumentationController();

//            var result = (ViewResult)controller.ReadDecesion();

//            var model = result.ViewData.Model;

//            Assert.IsType<Tuple<DecesionViewModel, List<DecesionViewModel>>>(model);
//        }
//    }
//}