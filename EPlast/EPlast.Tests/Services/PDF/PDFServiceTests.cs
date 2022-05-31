using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPlast.BLL.Interfaces.AzureStorage;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.PDF;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.GoverningBody;
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
            _pdfService = new PdfService(_repository.Object, _logger.Object, _decisionBlobStorage.Object);
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
            _repository.Setup(x =>
                    x.UserProfile.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), null))
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
            _repository.Setup(x =>
                    x.UserProfile.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), null))
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
        public void BlankCreatePdfAsync_WithoutFirstName_ReturnsByteArray(string userId)
        {
            // Arrange
            _repository.Setup(x => x.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(GetUserWithoutFirstName(userId));
            _repository.Setup(x =>
                    x.UserProfile.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), null))
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
        public void BlankCreatePdfAsync_WithoutListConfirmedUser_ReturnsByteArray(string userId)
        {
            // Arrange
            _repository
                .Setup(x => x.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(GetUserWithoutListConfirmedUser(userId));
            _repository.Setup(x =>
                    x.UserProfile.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), null))
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
        [TestCase(546546)]
        public void DecisionCreatePDFAsync_ThrowsException_CallsLogErrorMethod(int decisionId)
        {
            //Arrange
            _repository.Setup(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(Decesions.FirstOrDefault());
            _decisionBlobStorage.Setup(blob => blob.GetBlobBase64Async(It.IsAny<string>()))
                .Throws(new Exception("Test"));

            //Act
            var actualReturn = _pdfService.DecisionCreatePDFAsync(decisionId);

            //Assert
            _logger.Verify(x => x.LogError($"Exception: Test"));
            Assert.Null(actualReturn.Result);
        }

        [TestCase(1)]
        [TestCase(55)]
        [TestCase(101)]
        [TestCase(155)]
        public void MethodicDocumentCreatePdfAsync_ReturnsByteArray_Test(int methodicDocumentId)
        {
            // Arrange
            _repository.Setup(rep => rep.MethodicDocument.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<MethodicDocument, bool>>>(),
                    It.IsAny<Func<IQueryable<MethodicDocument>, IIncludableQueryable<MethodicDocument, object>>>()))
                .ReturnsAsync(MethodicDocuments.FirstOrDefault(m => m.Id == methodicDocumentId));
            _decisionBlobStorage.Setup(blob => blob.GetBlobBase64Async(It.IsAny<string>())).ReturnsAsync("Blank");

            // Act
            var actualReturn = _pdfService.MethodicDocumentCreatePdfAsync(methodicDocumentId);

            // Assert
            _repository.Verify(rep => rep.MethodicDocument.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<MethodicDocument, bool>>>(),
                    It.IsAny<Func<IQueryable<MethodicDocument>, IIncludableQueryable<MethodicDocument, object>>>()),
                Times.Once);
            Assert.IsInstanceOf<byte[]>(actualReturn.Result);
        }

        [TestCase(155)]
        public void MethodicDocumentCreatePdfAsync_ThrowsException_CallsLogErrorMethod(int methodicDocumentId)
        {
            // Arrange
            _repository.Setup(rep => rep.MethodicDocument)
                .Throws(new ArgumentNullException("Test"));

            // Act
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Assert
                await _pdfService.MethodicDocumentCreatePdfAsync(methodicDocumentId);
                _repository.Verify(rep => rep.MethodicDocument, Times.Once);
                _logger.Verify(x => x.LogError("Exception: Test"));
            });
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
                UserPlastDegrees = new UserPlastDegree()
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

        private static User GetUserWithoutFirstName(string userId)
        {
            return new User()
            {
                Id = userId,
                LastName = "LastName",
                ConfirmedUsers = listConfirmedUsers
            };
        }

        private static User GetUserWithoutListConfirmedUser(string userId)
        {
            return new User()
            {
                Id = userId,
                FirstName = "FirstName",
                LastName = "LastName"
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
            },
            new ConfirmedUser()
            {
                Approver = new Approver()
                {
                    User = new User()
                },
                isCityAdmin = false,
                isClubAdmin = false
            },
            new ConfirmedUser()
            {
                Approver = new Approver(),
                isCityAdmin = false,
                isClubAdmin = false
            },
            new ConfirmedUser()
            {
                isCityAdmin = false,
                isClubAdmin = false
            }
        };

        private static IQueryable<User> GetTestUsersQueryable()
        {
            return new List<User>
            {
                new User()
                {
                    Id = "1",
                    Approvers = new List<Approver>()
                        {new Approver() {User = new User(), UserID = "1", ConfirmedUser = new ConfirmedUser()}}
                },
                new User()
                {
                    Id = "546546",
                    Approvers = new List<Approver>()
                        {new Approver() {User = new User(), UserID = "546546", ConfirmedUser = new ConfirmedUser()}}
                }
            }.AsQueryable();
        }

        private static IQueryable<UserProfile> GetTestUserProfilesQueryable()
        {
            return new List<UserProfile>
            {
                new UserProfile() {UserID = "1"},
                new UserProfile() {UserID = "546546"}
            }.AsQueryable();
        }

        private static IQueryable<CityMembers> GetTestCityMembersQueryable()
        {
            return new List<CityMembers>
            {
                new CityMembers() {UserId = "1", CityId = 3, City = new DataAccess.Entities.City()},
                new CityMembers() {UserId = "546546", CityId = 3, City = new DataAccess.Entities.City()}
            }.AsQueryable();
        }

        private static IQueryable<ClubMembers> GetTestClubMembersQueryable()
        {
            return new List<ClubMembers>
            {
                new ClubMembers() {UserId = "1", Club = new DataAccess.Entities.Club()},
                new ClubMembers() {UserId = "546546", Club = new DataAccess.Entities.Club()}
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

        private MembersStatistic _fakeMembersStatistic()
        {
            return new MembersStatistic()
            {
                Id = 1,
                AnnualReportId = 1,
                NumberOfNovatstva = 1,
                NumberOfPtashata = 1,
                NumberOfSeigneurMembers = 1,
                NumberOfSeigneurSupporters = 1,
                NumberOfSeniorPlastynMembers = 1,
                NumberOfSeniorPlastynSupporters = 1,
                NumberOfUnatstvaMembers = 1,
                NumberOfUnatstvaNoname = 1,
                NumberOfUnatstvaProspectors = 1,
                NumberOfUnatstvaSkobVirlyts = 1,
                NumberOfUnatstvaSupporters = 1,
            };
        }

        private IQueryable<AnnualReport> AnnualReports => new List<AnnualReport>()
        {
            new AnnualReport()
            {
                ID = 1,
                CityId = 1,
                City = new DataAccess.Entities.City()
                {
                    Name = "CityName"
                },
                Date = DateTime.Now,
                NewCityAdmin = new User() {FirstName = "FName", LastName = "LName"},
                NumberOfAdministrators = 1,
                NumberOfBeneficiaries = 1,
                NumberOfClubs = 1,
                NumberOfHonoraryMembers = 1,
                NumberOfIndependentGroups = 1,
                NumberOfIndependentRiy = 1,
                NumberOfPlastpryiatMembers = 1,
                NumberOfSeatsPtashat = 1,
                NumberOfTeacherAdministrators = 1,
                NumberOfTeachers = 1,
                MembersStatistic = _fakeMembersStatistic(),
                ListProperty = null,
                ImprovementNeeds = null,
            },
            new AnnualReport()
            {
                ID = 2,
                CityId = 2,
                City = new DataAccess.Entities.City()
                {
                    Name = "SecondCityName"
                },
                Date = DateTime.Now,
                NumberOfAdministrators = 2,
                NumberOfBeneficiaries = 2,
                NumberOfClubs = 2,
                NumberOfHonoraryMembers = 2,
                NumberOfIndependentGroups = 2,
                NumberOfIndependentRiy = 2,
                NumberOfPlastpryiatMembers = 2,
                NumberOfSeatsPtashat = 2,
                NumberOfTeacherAdministrators = 2,
                NumberOfTeachers = 2,
                MembersStatistic = _fakeMembersStatistic(),
                ListProperty = "LProperty",
                ImprovementNeeds = null,
            },
            new AnnualReport()
            {
                ID = 3,
                CityId = 3,
                City = new DataAccess.Entities.City()
                {
                    Name = "CityName"
                },
                Date = DateTime.Now,
                NewCityAdmin = new User()
                    {FirstName = "FName", LastName = "LName", Email = "email@email.com", PhoneNumber = "0123456"},
                NumberOfAdministrators = 3,
                NumberOfBeneficiaries = 3,
                NumberOfClubs = 3,
                NumberOfHonoraryMembers = 3,
                NumberOfIndependentGroups = 3,
                NumberOfIndependentRiy = 3,
                NumberOfPlastpryiatMembers = 3,
                NumberOfSeatsPtashat = 3,
                NumberOfTeacherAdministrators = 3,
                NumberOfTeachers = 3,
                MembersStatistic = _fakeMembersStatistic(),
                ListProperty = null,
                ImprovementNeeds = "INeeds",
            },
            new AnnualReport()
            {
                ID = 4,
                CityId = 4,
                City = new DataAccess.Entities.City()
                {
                    Name = "CityName"
                },
                Date = DateTime.Now,
                NewCityAdmin = new User()
                    {FirstName = "FName", LastName = "LName", Email = "email@email.com", PhoneNumber = "0124456"},
                NumberOfAdministrators = 4,
                NumberOfBeneficiaries = 4,
                NumberOfClubs = 4,
                NumberOfHonoraryMembers = 4,
                NumberOfIndependentGroups = 4,
                NumberOfIndependentRiy = 4,
                NumberOfPlastpryiatMembers = 4,
                NumberOfSeatsPtashat = 4,
                NumberOfTeacherAdministrators = 4,
                NumberOfTeachers = 4,
                MembersStatistic = _fakeMembersStatistic(),
                ListProperty = "LProperty",
                ImprovementNeeds = "INeeds",
            },
        }.AsQueryable();

        private IQueryable<MethodicDocument> MethodicDocuments => new List<MethodicDocument>()
        {
            new MethodicDocument
            {
                Id = 1, Type = "legislation", Date = new DateTime(), Description = LongDescriptionGenerator(),
                Name = "Name", FileName = "dsf", Organization = new Organization()
            },
            new MethodicDocument
            {
                Id = 55, Type = "Methodics", Date = new DateTime(), Description = "Description55",
                Name = "Name55", FileName = "dsf", Organization = new Organization()
            },
            new MethodicDocument
            {
                Id = 101, Type = "Other", Date = new DateTime(), Description = "Description55",
                Name = "Name55", FileName = "dsf", Organization = new Organization()
            },
            new MethodicDocument
            {
                Id = 155, Type = "None", Date = new DateTime(), Description = "Description55",
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