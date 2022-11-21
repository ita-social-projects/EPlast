using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using Xunit;


namespace EPlast.XUnitTest.Services.TextFormatting
{

    public class TextFormatterTests
    {

        private readonly TextFormatter.LayoutOptions _layoutOptions;
        private readonly XGraphics _gfx;
        private readonly XFont font;

        public TextFormatterTests()
        {
            _layoutOptions = new TextFormatter.LayoutOptions()
            { SpacingMode = TextFormatter.SpacingMode.Relative, Spacing = 0 };

            var _document = new PdfDocument();
            _gfx = XGraphics.FromPdfPage(_document.AddPage());

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);
            font = new XFont("Arial", 20, XFontStyle.Bold, options);
        }


        [Fact]
        public void FontTest()
        {
            //Arrange

            //Act
            var actionManager = new TextFormatter(_gfx, _layoutOptions);

            var exception = Record.Exception(() => { actionManager.Font = font; });

            //Assert
            Assert.Null(exception);

        }

        [Fact]
        public void DrawStringTest()
        {
            //Arrange

            XRect rect = new XRect(0, 0, 100 / 2 - 2, 100 / 2 - 2);

            //Act
            var actionManager = new TextFormatter(_gfx);

            var exception = Record.Exception(() => {
                actionManager.DrawString("Hello", font, XBrushes.Black, rect); });

            //Assert
            Assert.Null(exception);

        }
    }
}
