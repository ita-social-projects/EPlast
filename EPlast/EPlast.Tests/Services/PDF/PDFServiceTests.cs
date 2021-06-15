using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.PDF;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;


namespace EPlast.Tests.Services.PDF
{
    [TestFixture]
    public class PdfServiceTests
    {
        private PdfService _pdfService;
        private static Mock<IRepositoryWrapper> _repository;
        private static Mock<ILoggerService<PdfService>> _logger;
        private static Mock<IDecisionBlobStorageRepository> _decisionBlobStorage;
       
        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _logger = new Mock<ILoggerService<PdfService>>();
            _decisionBlobStorage = new Mock<IDecisionBlobStorageRepository>();
            _repository.Setup(rep => rep.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(GetTestUsersQueryable());
            _repository.Setup(rep => rep.UserProfile.FindByCondition(It.IsAny<Expression<Func<UserProfile, bool>>>()))
                .Returns(GetTestUserProfilesQueryable());
            _repository.Setup(rep => rep.CityMembers.FindByCondition(It.IsAny<Expression<Func<CityMembers, bool>>>()))
                .Returns(GetTestCityMembersQueryable());
            _repository.Setup(rep => rep.ClubMembers.FindByCondition(It.IsAny<Expression<Func<ClubMembers, bool>>>()))
                .Returns(GetTestClubMembersQueryable());
            _logger.Setup(log => log.LogError(It.IsAny<string>()));
            _pdfService = new PdfService(_repository.Object,_logger.Object,_decisionBlobStorage.Object);
        }

        [TestCase(1)]
        [TestCase(546546)]
        public void DecisionCreatePDFAsync_ReturnsByteArray_Test(int decisionId)
        {

            _repository.Setup(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(Decesions.FirstOrDefault());
            _decisionBlobStorage.Setup(blob => blob.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync("Blank");
           
            var actualReturn = _pdfService.DecisionCreatePDFAsync(decisionId);

            _repository.Verify(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()), Times.Once);
            Assert.IsInstanceOf<byte[]>(actualReturn.Result);
        }

        [TestCase("1")] 
        [TestCase("546546")]
        public void BlankCreatePdfAsync_WithFatherName_ReturnsByteArray(string userId)
        {
            // Arrange
            _repository
                .Setup(x => x.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(GetUserWithFatherName(userId));
            _repository.Setup(x => x.UserProfile.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), null))
                .ReturnsAsync(new UserProfile());
            _repository
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                It.IsAny<Func<IQueryable<CityMembers>,
                IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers());
            _repository
               .Setup(x => x.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
               It.IsAny<Func<IQueryable<ClubMembers>,
               IIncludableQueryable<ClubMembers, object>>>()))
               .ReturnsAsync(new ClubMembers());

            // Act
            var actualReturn = _pdfService.BlankCreatePDFAsync(userId);

            // Assert
            _repository.Verify();
            Assert.IsInstanceOf<byte[]>(actualReturn.Result);
        }

        [TestCase("1")]
        [TestCase("546546")]
        public void BlankCreatePdfAsync_WithoutFatherName_ReturnsByteArray(string userId)
        {
            // Arrange
            _repository.Setup(x => x.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(GetUserWithoutFatherName(userId));
            _repository.Setup(x => x.UserProfile.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), null))
                .ReturnsAsync(new UserProfile());
            _repository
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                It.IsAny<Func<IQueryable<CityMembers>,
                IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers());
            _repository
               .Setup(x => x.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
               It.IsAny<Func<IQueryable<ClubMembers>,
               IIncludableQueryable<ClubMembers, object>>>()))
               .ReturnsAsync(new ClubMembers());

            // Act
            var actualReturn = _pdfService.BlankCreatePDFAsync(userId);

            // Assert
            _repository.Verify();
            Assert.IsInstanceOf<byte[]>(actualReturn.Result);
        }

        [TestCase("1")]
        [TestCase("546546")]
        public void BlankCreatePdfAsync_WithOnlyFirstName_ReturnsByteArray(string userId)
        {
            // Arrange
            _repository
                .Setup(x => x.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(GetUserWithOnlyFirstName(userId));
            _repository.Setup(x => x.UserProfile.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), null))
                .ReturnsAsync(new UserProfile());
            _repository
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>,
                        IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers());
            _repository
                .Setup(x => x.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubMembers>,
                        IIncludableQueryable<ClubMembers, object>>>()))
                .ReturnsAsync(new ClubMembers());

            // Act
            var actualReturn = _pdfService.BlankCreatePDFAsync(userId);

            // Assert
            _repository.Verify();
            Assert.IsInstanceOf<byte[]>(actualReturn.Result);
        }

        [TestCase("1")]
        [TestCase("546546")]
        public void BlankCreatePdfAsync_WithOnlyLastName_ReturnsByteArray(string userId)
        {
            // Arrange
            _repository
                .Setup(x => x.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(GetUserWithOnlyLastName(userId));
            _repository.Setup(x => x.UserProfile.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), null))
                .ReturnsAsync(new UserProfile());
            _repository
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>,
                        IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers());
            _repository
                .Setup(x => x.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubMembers>,
                        IIncludableQueryable<ClubMembers, object>>>()))
                .ReturnsAsync(new ClubMembers());

            // Act
            var actualReturn = _pdfService.BlankCreatePDFAsync(userId);

            // Assert
            _repository.Verify();
            Assert.IsInstanceOf<byte[]>(actualReturn.Result);
        }

        [TestCase("1")]
        [TestCase("546546")]
        public void BlankCreatePdfAsync_WithOnlyFatherName_ReturnsByteArray(string userId)
        {
            // Arrange
            _repository
                .Setup(x => x.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(GetUserWithOnlyFatherName(userId));
            _repository.Setup(x => x.UserProfile.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), null))
                .ReturnsAsync(new UserProfile());
            _repository
                .Setup(x => x.CityMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<CityMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<CityMembers>,
                        IIncludableQueryable<CityMembers, object>>>()))
                .ReturnsAsync(new CityMembers());
            _repository
                .Setup(x => x.ClubMembers.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<ClubMembers, bool>>>(),
                    It.IsAny<Func<IQueryable<ClubMembers>,
                        IIncludableQueryable<ClubMembers, object>>>()))
                .ReturnsAsync(new ClubMembers());

            // Act
            var actualReturn = _pdfService.BlankCreatePDFAsync(userId);

            // Assert
            _repository.Verify();
            Assert.IsInstanceOf<byte[]>(actualReturn.Result);
        }

        [TestCase("1")]
        [TestCase("546546")]
        public void BlankCreatePdfAsync_ReturnsNull_Test(string userId)
        {
            // Arrange
            _repository
                .Setup(x => x.UserProfile.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), null))
                .ReturnsAsync(new UserProfile());
            _repository
                .Setup(x => x.CityMembers.FindByCondition(It.IsAny<Expression<Func<CityMembers, bool>>>()))
                .Returns(new List<CityMembers>().AsQueryable());
            _repository
                .Setup(x => x.ClubMembers.FindByCondition(It.IsAny<Expression<Func<ClubMembers, bool>>>()))
                .Returns(new List<ClubMembers>().AsQueryable());

            // Act
            var actualReturn = _pdfService.BlankCreatePDFAsync(userId);

            // Assert
            _logger.Verify();
            Assert.Null(actualReturn.Result);
        }

        [TestCase(1)]
        [TestCase(546546)]
        public void DecisionCreatePDFAsync_ReturnsNull_Test(int decisionId)
        {
            var decisionRepository = new Mock<IDecesionRepository>();
            _repository.Setup(rep => rep.Decesion).Returns(decisionRepository.Object);
            var actualReturn = _pdfService.DecisionCreatePDFAsync(decisionId);
            _repository.Verify(rep => rep.Decesion, Times.Once);
            _logger.Verify();
            Assert.Null(actualReturn.Result);
        }

        [TestCase(1)]
        [TestCase(55)]
        [TestCase(101)]
        [TestCase(155)]
        public void MethodicDocumentCreatePdfAsync_ReturnsByteArray_Test(int methodicDocumentId)
        {
            // Arrange
            _repository.Setup(rep => rep.MethodicDocument.GetFirstAsync(It.IsAny<Expression<Func<MethodicDocument, bool>>>(),
                    It.IsAny<Func<IQueryable<MethodicDocument>, IIncludableQueryable<MethodicDocument, object>>>()))
                .ReturnsAsync(MethodicDocuments.FirstOrDefault(m => m.ID == methodicDocumentId));
            _decisionBlobStorage.Setup(blob => blob.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync("Blank");

            // Act
            var actualReturn = _pdfService.MethodicDocumentCreatePdfAsync(methodicDocumentId);

            // Assert
            _repository.Verify(rep => rep.MethodicDocument.GetFirstAsync(It.IsAny<Expression<Func<MethodicDocument, bool>>>(),
                It.IsAny<Func<IQueryable<MethodicDocument>, IIncludableQueryable<MethodicDocument, object>>>()), Times.Once);
            Assert.IsInstanceOf<byte[]>(actualReturn.Result);
        }


        [TestCase(155)]
        public void MethodicDocumentCreatePdfAsync_ReturnsNull_Test(int methodicDocumentId)
        {
            // Arrange
            var methodicDocumentRepository = new Mock<IMethodicDocumentRepository>();
            _repository.Setup(rep => rep.MethodicDocument).Returns(methodicDocumentRepository.Object);

            // Act
            var actualReturn = _pdfService.MethodicDocumentCreatePdfAsync(methodicDocumentId);

            // Assert
            _repository.Verify(rep => rep.MethodicDocument, Times.Once);
            _logger.Verify();
            Assert.Null(actualReturn.Result);
        }

        private static User GetUserWithFatherName(string userId)
        {
            return new User()
            {
                Id = userId,
                FirstName = "FirstName",
                LastName = "LastName",
                FatherName = "FatherName",
                ConfirmedUsers = listConfirmedUsers,
                UserPlastDegrees = new List<UserPlastDegree>()
                {
                    new UserPlastDegree()
                    {
                        IsCurrent = true
                    }
                }
            };
        }

        private static User GetUserWithoutFatherName(string userId)
        {
            return new User()
            {
                Id = userId,
                FirstName = "FirstName",
                LastName = "LastName",
                ConfirmedUsers = listConfirmedUsers
            };
        }

        private static User GetUserWithOnlyFirstName(string userId)
        {
            return new User()
            {
                Id = userId,
                FirstName = "FirstName",
                ConfirmedUsers = listConfirmedUsers
            };
        }

        private static User GetUserWithOnlyLastName(string userId)
        {
            return new User()
            {
                Id = userId,
                LastName = "LastName",
                ConfirmedUsers = listConfirmedUsers
            };
        }

        private static User GetUserWithOnlyFatherName(string userId)
        {
            return new User()
            {
                Id = userId,
                FatherName = "FatherName",
                ConfirmedUsers = listConfirmedUsers
            };
        }

        private static List<ConfirmedUser> listConfirmedUsers = new List<ConfirmedUser>()
        {
            new ConfirmedUser()
            {
                Approver = new Approver()
                {
                    User = new User()
                    {
                        FirstName = "FirstName",
                        LastName = "LastName"
                    }
                },
                isCityAdmin = true,
                isClubAdmin = false
            },
            new ConfirmedUser()
            {
                Approver = new Approver()
                {
                    User = new User()
                    {
                        FirstName = "FirstName"
                    }
                },
                isCityAdmin = false,
                isClubAdmin = true
            },
            new ConfirmedUser()
            {
                Approver = new Approver()
                {
                    User = new User()
                    {
                        LastName = "LastName"
                    }
                },
                isCityAdmin = false,
                isClubAdmin = false
            }
        };

        private static IQueryable<User> GetTestUsersQueryable()
        {
            return new List<User>
            {
                new User(){Id = "1",Approvers = new List<Approver>(){new Approver(){User =new User(),UserID = "1",ConfirmedUser = new ConfirmedUser()}}},
                new User(){Id = "546546",Approvers = new List<Approver>(){new Approver(){User =new User(), UserID = "546546", ConfirmedUser = new ConfirmedUser()}}}
            }.AsQueryable();
        }
        private static IQueryable<UserProfile> GetTestUserProfilesQueryable()
        {
            return new List<UserProfile>
            {
                new UserProfile(){ UserID ="1"},
                new UserProfile(){ UserID ="546546"}
            }.AsQueryable();
        }
        private static IQueryable<CityMembers> GetTestCityMembersQueryable()
        {
            return new List<CityMembers>
            {
                new CityMembers(){ UserId ="1",CityId=3,City = new DataAccess.Entities.City()},
                new CityMembers(){ UserId ="546546",CityId=3,City = new DataAccess.Entities.City()}
            }.AsQueryable();
        }
        private static IQueryable<ClubMembers> GetTestClubMembersQueryable()
        {
            return new List<ClubMembers>
            {
                new ClubMembers(){ UserId ="1",Club = new DataAccess.Entities.Club()},
                new ClubMembers(){ UserId ="546546", Club = new DataAccess.Entities.Club()}
            }.AsQueryable();
        }

        private IQueryable<Decesion> Decesions => new List<Decesion>()
        {
            new Decesion()
            {
                ID = 1, DecesionStatusType = DecesionStatusType.Confirmed, Date = new DateTime(), Description = "FDS",
                Name = "FS", FileName = "dsf", DecesionTarget = new DecesionTarget(), Organization = new Organization()
            },
            new Decesion()
            {
                ID = 546546, DecesionStatusType = DecesionStatusType.Confirmed, Date = new DateTime(),
                Description = "FDS", Name = "FS", FileName = "dsf", DecesionTarget = new DecesionTarget(),
                Organization = new Organization()
            }
        }.AsQueryable();

        private IQueryable<MethodicDocument> MethodicDocuments => new List<MethodicDocument>()
        {
            new MethodicDocument
            {
                ID = 1, Type = "legislation", Date = new DateTime(), Description = LongDescriptionGenerator(),
                Name = "Name", FileName = "dsf", Organization = new Organization()
            },
            new MethodicDocument
            {
                ID = 55, Type = "Methodics", Date = new DateTime(), Description = "Description55",
                Name = "Name55", FileName = "dsf", Organization = new Organization()
            },
            new MethodicDocument
            {
                ID = 101, Type = "Other", Date = new DateTime(), Description = "Description55",
                Name = "Name55", FileName = "dsf", Organization = new Organization()
            },            new MethodicDocument
            {
                ID = 155, Type = "None", Date = new DateTime(), Description = "Description55",
                Name = "Name55", FileName = "dsf", Organization = new Organization()
            }
        }.AsQueryable();

        private static string LongDescriptionGenerator()
        {
            var description = "Very long description ,";
            for (var i = 0; i < 8; i++)
            {
                description += description;
            }

            return description;
        }
    }
}
