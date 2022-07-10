using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.Commands.Decision;
using EPlast.BLL.DTO;
using EPlast.BLL.Handlers.DecisionHandlers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Handlers.Decision
{
    public class UpdateHandlerTest
    {
        private Mock<IRepositoryWrapper> _repository;
        private Mock<IMapper> _mockMapper;
        private UpdateHandler _handler;
        private UpdateCommand _query;
        
        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateHandler(_repository.Object, _mockMapper.Object);
        }
        [TestCase("new name", "new text")]
        [TestCase("", "new text")]
        [TestCase("new name", "")]
        [TestCase("", "")]
        public async Task ChangeDecision_Valid_Test(string decisionNewName, string decisionNewDescription)
        {
            //Arrange
            _repository.Setup(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                    It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()))
                .ReturnsAsync(GetTestDecesionQueryable().FirstOrDefault());

            //Act
            var changingDecisionDto = new DecisionDto();
            changingDecisionDto.Name = decisionNewName;
            changingDecisionDto.Description = decisionNewDescription;
            _query = new UpdateCommand(changingDecisionDto);
            await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            _repository.Verify(rep => rep.Decesion.GetFirstAsync(It.IsAny<Expression<Func<Decesion, bool>>>(),
                   It.IsAny<Func<IQueryable<Decesion>, IIncludableQueryable<Decesion, object>>>()), Times.Once);
        }
        private static IQueryable<Decesion> GetTestDecesionQueryable()
        {
            return new List<Decesion>
            {
                new Decesion  {ID = 1,Description = "old"},
                new Decesion  {ID = 2,Description = "old"},
                new Decesion  {ID = 3,Description = "old"},
                new Decesion  {ID = 4,Description = "old"}
            }.AsQueryable();
        }
    }
}
