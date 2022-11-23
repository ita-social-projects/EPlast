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

        private Dictionary<string, string> inMemorySettings;
        private IConfiguration configuration;

        public AzureBlobConnectionFactoryTests()
        {
            inMemorySettings = new Dictionary<string, string>();
        }


        [Fact]
        public async Task GetBlobContainerTest_DoesntThrowExeption()
        {
            // Arrange
            const int ID_CONTAINER = 1;

            FillMemorySettings(ID_CONTAINER);
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Act
            var actionManager = new AzureBlobConnectionFactory(configuration);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => actionManager.GetBlobContainer(ID_CONTAINER.ToString()));

        }

        private void FillMemorySettings(int ID_CONTAINER)
        {
            inMemorySettings.Add($"BlobContainerNames:{ID_CONTAINER}", "value");
            inMemorySettings.Add("BlobStorage", "value");
        }


    }
}
