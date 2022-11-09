using EPlast.BLL.Handlers.DistinctionHandlers;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.Tests.Handlers.Distinction
{
    public class CheckIfAdminHandlerTest
    {
        private Mock<UserManager<User>> _userManager;
        private CheckIfAdminQuery _query;

        private User _user;

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _user = new User();
            _query = new CheckIfAdminQuery(_user);
        }

        private IList<string> GetRolesWithoutAdmin()
        {
            return new List<string>
            {
                "Htos",
                "Nixto"
            };
        }

        private IList<string> GetRolesWithAdmin()
        {
            return new List<string>
            {
                Roles.Admin,
                "Htos",
                "Nixto"
            };
        }
    }
}
