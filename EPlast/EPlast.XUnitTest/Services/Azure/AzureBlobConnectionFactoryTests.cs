using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services.AzureStorage.Base;
using EPlast.BLL.Services.City.CityAccess;
using EPlast.BLL.Settings;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using DatabaseEntities = EPlast.DataAccess.Entities;


namespace EPlast.XUnitTest.Services.Azure
{
    public class AzureBlobConnectionFactoryTests
    {

        [Fact]
        public async Task GetBlobContainerTest()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string>();
            inMemorySettings.Add("BlobContainerNames:1", "value");
            inMemorySettings.Add("BlobStorage", "value");

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Act
            var actionManager = new AzureBlobConnectionFactory(configuration);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => actionManager.GetBlobContainer("1"));

        }


    }
}
