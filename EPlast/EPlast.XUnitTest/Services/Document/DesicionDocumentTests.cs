using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using EPlast.DataAccess.Entities;
using EPlast.BLL.Services.PDF.Documents;

namespace EPlast.XUnitTest.Services.Document
{
    public class DesicionDocumentTests
    {
        private Decesion _decesion;
        private PdfPage _pdfPage;

        public DesicionDocumentTests()
        {
            _decesion = new Decesion();
            _pdfPage = new PdfPage();
        }

        private void Setup()
        {
            _decesion.Name = "DocName";
            _decesion.Description = "DocDescription";
            _decesion.Date = DateTime.Now;
            _decesion.FileName = "DocFileName";
            _decesion.UserId = "userId";
        }

        [Fact]
        public void SetDocumentBodyDescriptionNull_ArgumentNullException()
        {
            //Arrange
            Setup();
            _decesion.Description = null;
            var expected = new ArgumentException().GetType();

            // Act
            var desicionDocument = new DecisionDocument(_decesion);
            Type actual = Assert.Throws<ArgumentException>(() => desicionDocument.SetDocumentBody(_pdfPage, XGraphics.FromPdfPage(_pdfPage))).GetType();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
