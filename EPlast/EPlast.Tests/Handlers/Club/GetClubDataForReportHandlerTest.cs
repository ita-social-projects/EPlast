using AutoMapper;
using EPlast.BLL.DTO.Admin;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Handlers.ClubHandlers;
using EPlast.BLL.Queries.Club;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DataAccessClub = EPlast.DataAccess.Entities;

namespace EPlast.Tests.Handlers.Club
{
    public class GetClubDataForReportHandlerTest
    {
        private Mock<IRepositoryWrapper> _repoWrapper;
        private Mock<IMapper> _mapper;
        private Mock<IMediator> _mediator;
        private GetClubDataForReportHandler _handler;

        private int Count => 2;

        [SetUp]
        public void SetUp()
        {
            _repoWrapper = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _mediator = new Mock<IMediator>();
            _handler = new GetClubDataForReportHandler(_repoWrapper.Object, _mapper.Object, _mediator.Object);
        }

        [Test]
        public async Task GetClubDataForReportHandlerTest_ReturnsClubReportDataDTO()
        {
            // Arrange
            _repoWrapper
                .Setup(r => r.Club.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DataAccessClub.Club, bool>>>(), null))
                .ReturnsAsync(new DataAccessClub.Club());

            _mapper
                .Setup(m => m.Map<DataAccessClub.Club, ClubDTO>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithExAdmin());

            _mediator
                .Setup(m => m.Send(It.IsAny<GetClubAdministrationsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listItems);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetClubHistoryMembersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ClubMemberHistoryDTO>());

            _mediator
               .Setup(m => m.Send(It.IsAny<GetClubHistoryFollowersQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<ClubMemberHistoryDTO>());

            _mediator
                .Setup(m => m.Send(It.IsAny<GetCountUsersPerYearQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Count);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetCountDeletedUsersPerYearQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Count);

            // Act
            var result = await _handler.Handle(query, It.IsAny<CancellationToken>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ClubReportDataDTO>(result);
        }

        private GetClubDataForReportQuery query = new GetClubDataForReportQuery(GetFakeNumber()); 

        private static int GetFakeNumber()
        {
            return 1;
        }

        private List<ClubAdministration> listItems = new List<ClubAdministration>
        {
            new ClubAdministration
            {
                UserId = "a124e48a-e83a-4e1c-a222-a3e654ac09ad",
                User = new DataAccessClub.User(),
                Status=true,
                ClubId=1,
                AdminType = new AdminType
                {
                    AdminTypeName = Roles.KurinHead
                },
                EndDate = DateTime.Now.AddMonths(-3)
            }
        };

        private ClubDTO CreateFakeClubDtoWithExAdmin()
        {
            var clubDto = GetClubDto();
            clubDto.ClubAdministration = new List<ClubAdministrationDTO>
            {
                new ClubAdministrationDTO
                {
                    UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                    User = new ClubUserDTO(),
                    AdminType = new AdminTypeDTO
                    {
                        AdminTypeName = Roles.KurinHead
                    },
                    EndDate = DateTime.Now.AddMonths(-3)
                },
                new ClubAdministrationDTO
                {
                    UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                    User = new ClubUserDTO(),
                    AdminType = new AdminTypeDTO
                    {
                        AdminTypeName = "----------",
                    },
                    EndDate = DateTime.Now.AddMonths(-3)
                }
            };

            return clubDto;
        }

        private ClubDTO GetClubDto()
        {
            var club = GetClubDtoWithoutMembers();
            club.ClubMembers = new List<ClubMembersDTO>
                {
                    new ClubMembersDTO
                    {
                        UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                        User = new ClubUserDTO(),
                        StartDate = new Random().Next(0,1) ==1 ? DateTime.Today : (DateTime?) null
                    }
                };
            return club;
        }

        private ClubDTO GetClubDtoWithoutMembers()
        {
            return new ClubDTO
            {
                ClubAdministration = GetClubAdministrationDTO(),
                ClubDocuments = new List<ClubDocumentsDTO>
                    {
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO(),
                        new ClubDocumentsDTO()
                    }
            };
        }

        private List<ClubAdministrationDTO> GetClubAdministrationDTO()
        {
            return new List<ClubAdministrationDTO>
            {
                 new ClubAdministrationDTO
                 {
                      UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                      User = new ClubUserDTO(),
                      AdminType = new AdminTypeDTO
                      {
                           AdminTypeName = Roles.KurinHead
                      }
                 },
                 new ClubAdministrationDTO
                 {
                      UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                      User = new ClubUserDTO(),
                      AdminType = new AdminTypeDTO
                      {
                           AdminTypeName = "----------"
                      }
                 }
            };
        }
    }
}
