using EPlast.BLL.DTO;
using EPlast.BLL.Services.PDF.Documents;
using EPlast.DataAccess.Entities;
using PdfSharpCore.Drawing;
using System;
using Xunit;

namespace EPlast.XUnitTest.Services.PDF.Documents
{
    public class DecisionDocumentTests
    {
        private Decesion _decision;
        private PdfSharpCore.Pdf.PdfPage _pdfpage;
        private DecisionDocument _decisionDoc;

        public DecisionDocumentTests()
        {
            _decision = new Decesion();
            _decisionDoc = new DecisionDocument(_decision);
            _pdfpage = new PdfSharpCore.Pdf.PdfPage(new PdfSharpCore.Pdf.PdfDocument());
        }

        [Fact]
        public void SetDocumentBodySuccessTest()
        {
            //Arrange
            setDecision();
            //Act
            var result = Record.Exception(() => _decisionDoc.SetDocumentBody(_pdfpage, XGraphics.FromPdfPage(_pdfpage)));
            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void SetDocumentBodyNullReferenceExceptionTest()
        {
            //Arrange
            Type expected = new NullReferenceException().GetType();
            Type actual;
            //Act
            _decisionDoc = new DecisionDocument(null);
            actual = Assert.Throws<NullReferenceException>(() => _decisionDoc.SetDocumentBody(_pdfpage, XGraphics.FromPdfPage(_pdfpage))).GetType();
            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetDocumentBodyArgumentNullExceptionTest()
        {
            //Arrange
            Type expected = new ArgumentNullException().GetType();
            Type actual;
            //Act
            actual = Assert.Throws<ArgumentNullException>(() => _decisionDoc.SetDocumentBody(_pdfpage, XGraphics.FromPdfPage(_pdfpage))).GetType();
            //Assert
            Assert.Equal(expected, actual);
        }

        private void setDecision()
        {
            _decision.ID = 1;
            _decision.Name = "name";
            _decision.Date = DateTime.Now;
            _decision.Description = "description";
            _decision.DecesionStatusType = DecesionStatusType.Confirmed;
        }
    }
}
