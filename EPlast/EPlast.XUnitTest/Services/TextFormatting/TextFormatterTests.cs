using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using Xunit;


namespace EPlast.XUnitTest.Services.TextFormatting
{

    public class TextFormatterTests
    {

        private TextFormatter.LayoutOptions _layoutOptions;
        private XGraphics _gfx;
        private XFont font;

        public TextFormatterTests()
        {
            _layoutOptions = CreateLayoutOptions();
            font = CreateXFont();
            _gfx = CreateXGraphics();

        }


        [Fact]
        public void FontTest_DoesntThrowExeption()
        {
            //Arrange

            //Act
            var actionManager = new TextFormatter(_gfx, _layoutOptions);

            var exception = Record.Exception(() => { actionManager.Font = font; });

            //Assert
            Assert.Null(exception);

        }

        [Fact]
        public void DrawStringTest_DoesntThrowExeption()
        {
            //Arrange
            const string STRING_TO_DRAW = "Hello";
            const int X = 0, Y = 0, WIDTH = 48, HEIGHT = 48;

            XRect rect = CreateXRect(X, Y, WIDTH, HEIGHT);

            //Act
            var actionManager = new TextFormatter(_gfx);

            var exception = Record.Exception(() => {
                actionManager.DrawString(STRING_TO_DRAW, font, XBrushes.Black, rect); });

            //Assert
            Assert.Null(exception);

        }

        private XRect CreateXRect(int X, int Y, int WIDTH, int HEIGHT)
        {
            return new XRect(X, Y, WIDTH, HEIGHT);
        }

        private XGraphics CreateXGraphics()
        {
            var _document = new PdfDocument();
            return XGraphics.FromPdfPage(_document.AddPage());
        }

        private TextFormatter.LayoutOptions CreateLayoutOptions()
        {
            return new TextFormatter.LayoutOptions()
            { SpacingMode = TextFormatter.SpacingMode.Relative, Spacing = 0 };
        }

        private XFont CreateXFont()
        {
            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);
            return new XFont("Arial", 20, XFontStyle.Bold, options);
        }
    }
}
