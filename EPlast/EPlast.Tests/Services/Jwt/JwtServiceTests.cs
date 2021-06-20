using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Services.Interfaces;
using EPlast.BLL.Services.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services.Jwt
{
    class JwtServiceTests
    {
        private Mock<IOptions<JwtOptions>> _jwtOptionsMock;
        private Mock<IUserManagerService> _userManagerServiceMock;
        private Mock<IUniqueIdService> _uniqueIdServiceMock;
        private JwtService _jwtService;

        [SetUp]
        public void SetUp()
        {
            _jwtOptionsMock = new Mock<IOptions<JwtOptions>>();
            _jwtOptionsMock.SetupGet(x => x.Value)
                .Returns(new JwtOptions
                {
                    Key = "2af4ff57-4ca0-4b3a-804b-178ad27aaf88",
                    Audience = "https://localhost:3000/",
                    Issuer = "https://localhost:44350/",
                    Time = 120
                });
            _userManagerServiceMock = new Mock<IUserManagerService>();
            _uniqueIdServiceMock = new Mock<IUniqueIdService>();
            _jwtService = new JwtService(
                _jwtOptionsMock.Object,
                _userManagerServiceMock.Object,
                _uniqueIdServiceMock.Object);
        }

        [Test]
        public async Task GetEventAdmininistrationByUserIdAsync_ReturnsCorrect()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var uniqueId = Guid.NewGuid();
            var userDto = new UserDTO
            {
                Email = "test@test.com",
                Id = userId
            };

            var roles = new string[]
            {
                "testRole1",
                "testRole2"
            };

            _uniqueIdServiceMock.Setup(x => x.GetUniqueId()).Returns(uniqueId);
            _userManagerServiceMock.Setup(x => x.GetRolesAsync(userDto))
                .ReturnsAsync(roles);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDto.Email),
                new Claim(JwtRegisteredClaimNames.NameId, userDto.Id),
                new Claim(JwtRegisteredClaimNames.FamilyName, userDto.Id),
                new Claim(JwtRegisteredClaimNames.Jti, uniqueId.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roles[0]),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roles[1])
            };



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("2af4ff57-4ca0-4b3a-804b-178ad27aaf88"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:44350/",
                audience: "https://localhost:3000/",
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: creds);

            var tokenHandler = new JwtSecurityTokenHandler();
            var result = tokenHandler.WriteToken(token);

            // Act
            var result1 = await _jwtService.GenerateJWTTokenAsync(userDto);
            // Assert
            //Assert.IsInstanceOf<IEnumerable<EventAdministration>>(result);
            Assert.AreEqual(result.Length, result1.Length);
        }
    }
}

