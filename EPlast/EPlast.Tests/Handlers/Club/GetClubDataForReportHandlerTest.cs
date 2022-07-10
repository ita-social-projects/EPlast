using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
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
                .Setup(m => m.Map<DataAccessClub.Club, ClubDto>(It.IsAny<DataAccessClub.Club>()))
                .Returns(CreateFakeClubDtoWithExAdmin());

            _mediator
                .Setup(m => m.Send(It.IsAny<GetClubAdministrationsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listItems);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetClubHistoryMembersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ClubMemberHistoryDto>());

            _mediator
               .Setup(m => m.Send(It.IsAny<GetClubHistoryFollowersQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<ClubMemberHistoryDto>());

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
            Assert.IsInstanceOf<ClubReportDataDto>(result);
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

        private ClubDto CreateFakeClubDtoWithExAdmin()
        {
            var clubDto = GetClubDto();
            clubDto.ClubAdministration = new List<ClubAdministrationDto>
            {
                new ClubAdministrationDto
                {
                    UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                    User = new ClubUserDto(),
                    AdminType = new AdminTypeDto
                    {
                        AdminTypeName = Roles.KurinHead
                    },
                    EndDate = DateTime.Now.AddMonths(-3)
                },
                new ClubAdministrationDto
                {
                    UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                    User = new ClubUserDto(),
                    AdminType = new AdminTypeDto
                    {
                        AdminTypeName = "----------",
                    },
                    EndDate = DateTime.Now.AddMonths(-3)
                }
            };

            return clubDto;
        }

        private ClubDto GetClubDto()
        {
            var club = GetClubDtoWithoutMembers();
            club.ClubMembers = new List<ClubMembersDto>
                {
                    new ClubMembersDto
                    {
                        UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                        User = new ClubUserDto(),
                        StartDate = new Random().Next(0,1) ==1 ? DateTime.Today : (DateTime?) null
                    }
                };
            return club;
        }

        private ClubDto GetClubDtoWithoutMembers()
        {
            return new ClubDto
            {
                ClubAdministration = GetClubAdministrationDTO(),
                ClubDocuments = new List<ClubDocumentsDto>
                    {
                        new ClubDocumentsDto(),
                        new ClubDocumentsDto(),
                        new ClubDocumentsDto(),
                        new ClubDocumentsDto(),
                        new ClubDocumentsDto()
                    }
            };
        }

        private List<ClubAdministrationDto> GetClubAdministrationDTO()
        {
            return new List<ClubAdministrationDto>
            {
                 new ClubAdministrationDto
                 {
                      UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                      User = new ClubUserDto(),
                      AdminType = new AdminTypeDto
                      {
                           AdminTypeName = Roles.KurinHead
                      }
                 },
                 new ClubAdministrationDto
                 {
                      UserId = "a124e48a - e83a - 4e1c - a222 - a3e654ac09ad",
                      User = new ClubUserDto(),
                      AdminType = new AdminTypeDto
                      {
                           AdminTypeName = "----------"
                      }
                 }
            };
        }
    }
}
