using EPlast.BLL.Services.GoverningBodies.Announcement;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.Tests.Services.GoverningBody.Announcement
{
    public class HtmlServiceTests
    {
        private HtmlService _htmlService;

        [SetUp]
        public void SetUp()
        {
            _htmlService = new HtmlService();
        }

        [Test]
        public void IsHtmlTextEmpty_StringIsNull_ReturnTrue()
        {
            //Arrange
            string testHtmlString = null;
            //Act
            var result = _htmlService.IsHtmlTextEmpty(testHtmlString);

            //Assert
            Assert.IsTrue(result);
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public void IsHtmlTextEmpty_StringIsEmpty_ReturnTrue()
        {
            //Arrange
            string testHtmlString = "<p></p>";
            //Act
            var result = _htmlService.IsHtmlTextEmpty(testHtmlString);

            //Assert
            Assert.IsTrue(result);
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public void IsHtmlTextEmpty_StringIsValid_ReturnFalse()
        {
            //Arrange
            string testHtmlString = "<p>Some Text</p>";
            //Act
            var result = _htmlService.IsHtmlTextEmpty(testHtmlString);

            //Assert
            Assert.IsFalse(result);
            Assert.IsInstanceOf<bool>(result);
        }


    }
}
