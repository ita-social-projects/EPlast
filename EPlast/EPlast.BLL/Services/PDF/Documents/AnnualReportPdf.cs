using EPlast.BLL.ExtensionMethods;
using EPlast.DataAccess.Entities;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;

namespace EPlast.BLL.Services.PDF.Documents
{
    public class AnnualReportPdf : PdfDocument
    {
        private readonly AnnualReport _annualReport;
        private const int TextWidth = 510;
        private const int LeftIndent = 60;
        private const string FontName = "Calibri";
        private const int BaseFontSize = 10;
        
        public AnnualReportPdf(AnnualReport annualReport, IPdfSettings settings) : base(settings)
        {
            _annualReport = annualReport;
        }

        public override void SetDocumentBody(PdfPage page, XGraphics gfx)
        {
            DrawText(gfx, page);
        }

        protected void DrawText(XGraphics gfx, PdfPage page)
        {
            int topRectY = 200;
            int currentRowY = 240;
            int lineStart = 25;
            int lineEnd = 560;
            int column1_X = 40;
            int column2_X = 200;
            int column3_X = 500;
            int topRectHeight = 100;
            int additionalHeight = 0;
            int textAreaWidth = 500;
            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);
            XFont font = new XFont(FontName, BaseFontSize, XFontStyle.Regular, options);
            XPen pen = new XPen(XColors.Black);
            XStringFormat format = new XStringFormat();


            XRect topRect = new XRect(LeftIndent, topRectY, TextWidth, topRectHeight);

            format.Alignment = XStringAlignment.Center;
            

            gfx.DrawString($"Звіт станиці {_annualReport.City.Name} за {_annualReport.Date.Year}", new XFont(FontName, 18, XFontStyle.Regular, options), XBrushes.Black, topRect, format);
            format.Alignment = XStringAlignment.Near;
            gfx.DrawString("Голова новообраної старшини:", font, XBrushes.Black, column1_X, currentRowY, format);
            if (_annualReport.NewCityAdmin == null)
                gfx.DrawString("відсутній", new XFont(FontName, BaseFontSize, XFontStyle.Italic, options), XBrushes.Black, column2_X, currentRowY, format);
            else
            {
                gfx.DrawString($"{_annualReport.NewCityAdmin.FirstName} {_annualReport.NewCityAdmin.LastName}", font, XBrushes.Black, column2_X, currentRowY, format);
                format.Alignment = XStringAlignment.Far;
                if (_annualReport.NewCityAdmin.Email!=null) 
                    gfx.DrawString($"{_annualReport.NewCityAdmin.Email}", font, XBrushes.Black, lineEnd, currentRowY, format);
                if (_annualReport.NewCityAdmin.PhoneNumber != null)
                {
                    currentRowY += 15;
                    gfx.DrawString($"{_annualReport.NewCityAdmin.PhoneNumber}", font, XBrushes.Black, lineEnd, currentRowY, format);
                }
                format.Alignment = XStringAlignment.Near;
            }

            currentRowY += 15;
            gfx.DrawLine(pen, lineStart, currentRowY, lineEnd, currentRowY);
            ///////////////////////////////////////////////////////////
            currentRowY += 15;
            gfx.DrawString("Правовий статус осередку:", font, XBrushes.Black, column1_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.NewCityLegalStatusType.GetDescription()}", font, XBrushes.Black, column2_X, currentRowY, format);

            currentRowY += 15;
            gfx.DrawLine(pen, lineStart, currentRowY, lineEnd, currentRowY);
            //////////////////////////////////////////////////////////
            currentRowY += 15;
            gfx.DrawString("УПП", font, XBrushes.Black, column1_X, currentRowY, format);
            gfx.DrawString("Кількість гніздечок пташат:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.NumberOfSeatsPtashat}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Кількість пташат:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.MembersStatistic.NumberOfPtashata}", font, XBrushes.Black, column3_X, currentRowY, format);

            currentRowY += 15;
            gfx.DrawLine(pen, lineStart, currentRowY, lineEnd, currentRowY);
            //////////////////////////////////////////////////////////
            currentRowY += 15;
            gfx.DrawString("УПН", font, XBrushes.Black, column1_X, currentRowY, format);
            gfx.DrawString("Кількість самостійних роїв:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.NumberOfIndependentRiy}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Кількість новацтва:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.MembersStatistic.NumberOfNovatstva}", font, XBrushes.Black, column3_X, currentRowY, format);

            currentRowY += 15;
            gfx.DrawLine(pen, lineStart, currentRowY, lineEnd, currentRowY);
            //////////////////////////////////////////////////////////
            currentRowY += 15;
            gfx.DrawString("УПЮ", font, XBrushes.Black, column1_X, currentRowY, format);
            gfx.DrawString("Кількість куренів у станиці/паланці (окрузі/регіоні):", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.NumberOfClubs}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Кількість самостійних гуртків:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.NumberOfIndependentGroups}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Кількість неіменованих разом:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.MembersStatistic.NumberOfUnatstvaNoname}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Кількість прихильників/ць:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.MembersStatistic.NumberOfUnatstvaSupporters}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Кількість учасників/ць:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.MembersStatistic.NumberOfUnatstvaMembers}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Кількість розвідувачів:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.MembersStatistic.NumberOfUnatstvaProspectors}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Кількість скобів/вірлиць:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.MembersStatistic.NumberOfUnatstvaSkobVirlyts}", font, XBrushes.Black, column3_X, currentRowY, format);

            currentRowY += 15;
            gfx.DrawLine(pen, lineStart, currentRowY, lineEnd, currentRowY);
            //////////////////////////////////////////////////////////
            currentRowY += 15;
            gfx.DrawString("УСП", font, XBrushes.Black, column1_X, currentRowY, format);
            gfx.DrawString("Кількість старших пластунів прихильників:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.MembersStatistic.NumberOfSeigneurSupporters}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Кількість старших пластунів:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.MembersStatistic.NumberOfSeigneurMembers}", font, XBrushes.Black, column3_X, currentRowY, format);

            currentRowY += 15;
            gfx.DrawLine(pen, lineStart, currentRowY, lineEnd, currentRowY);
            //////////////////////////////////////////////////////////
            currentRowY += 15;
            gfx.DrawString("УПС", font, XBrushes.Black, column1_X, currentRowY, format);
            gfx.DrawString("Кількість сеньйорів пластунів прихильників:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.MembersStatistic.NumberOfSeniorPlastynSupporters}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Кількість сеньйорів пластунів:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.MembersStatistic.NumberOfSeniorPlastynMembers}", font, XBrushes.Black, column3_X, currentRowY, format);

            currentRowY += 15;
            gfx.DrawLine(pen, lineStart, currentRowY, lineEnd, currentRowY);
            //////////////////////////////////////////////////////////
            currentRowY += 15;
            gfx.DrawString("Адміністрування та виховництво", font, XBrushes.Black, column1_X, currentRowY, format);
            gfx.DrawString("Кількість діючих виховників (з усіх членів УСП, УПС):", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.NumberOfTeachers}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Кількість адміністраторів (в проводах будь якого рівня):", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.NumberOfAdministrators}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Кількість тих, хто поєднує виховництво та адміністрування:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.NumberOfTeacherAdministrators}", font, XBrushes.Black, column3_X, currentRowY, format);

            currentRowY += 15;
            gfx.DrawLine(pen, lineStart, currentRowY, lineEnd, currentRowY);
            //////////////////////////////////////////////////////////
            currentRowY += 15;
            gfx.DrawString("Пластприят", font, XBrushes.Black, column1_X, currentRowY, format);
            gfx.DrawString("Кількість пільговиків:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.NumberOfBeneficiaries}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Кількість членів Пластприяту:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.NumberOfPlastpryiatMembers}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Кількість почесних членів:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.NumberOfHonoraryMembers}", font, XBrushes.Black, column3_X, currentRowY, format);

            currentRowY += 15;
            gfx.DrawLine(pen, lineStart, currentRowY, lineEnd, currentRowY);
            //////////////////////////////////////////////////////////
            currentRowY += 15;
            gfx.DrawString("Залучені кошти", font, XBrushes.Black, column1_X, currentRowY, format);
            gfx.DrawString("Державні кошти:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.ContributionFunds}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Внески:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.PublicFunds}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Пластовий заробіток:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.PlastSalary}", font, XBrushes.Black, column3_X, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString("Спонсорські кошти:", font, XBrushes.Black, column2_X, currentRowY, format);
            gfx.DrawString($"{_annualReport.SponsorshipFunds}", font, XBrushes.Black, column3_X, currentRowY, format);

            page = document.AddPage();
            // Also missing:
            gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);
            currentRowY = 40;

            gfx.DrawString("Майно та потреби станиці", font, XBrushes.Black, 250, currentRowY, format);
            currentRowY += 15;
            gfx.DrawString(
                "Вкажіть, що вам допоможе ефективніше залучати волонтерів та створювати виховні частини (гнізда, курені):",
                new XFont(FontName, BaseFontSize, XFontStyle.Italic, options), XBrushes.Black, column1_X,
                currentRowY, format);
            currentRowY += 15;

            additionalHeight = _annualReport.ImprovementNeeds == null
                ? 0
                : (_annualReport.ImprovementNeeds.Split('\n').Length - 1) * 10;
            var textArea = new XRect(column1_X, currentRowY, textAreaWidth, 10 + additionalHeight);
            tf.DrawString((_annualReport.ListProperty != null ? $"{_annualReport.ListProperty}" : "Інформація відсутня"), font, XBrushes.Black, textArea, XStringFormats.TopLeft);

            currentRowY += 20 + additionalHeight;
            gfx.DrawLine(pen, lineStart, currentRowY, lineEnd, currentRowY);
            //////////////////////////////////////////////////////////
            currentRowY += 15;
            gfx.DrawString("Вкажіть перелік майна, що є в станиці:", new XFont(FontName, BaseFontSize, XFontStyle.Italic, options), XBrushes.Black, column1_X, currentRowY, format);
            currentRowY += 20;
            additionalHeight = _annualReport.ImprovementNeeds == null
                ? 0
                : (_annualReport.ImprovementNeeds.Split('\n').Length - 1) * 10;
            textArea = new XRect(column1_X, currentRowY, textAreaWidth, 10 + additionalHeight);
            tf.DrawString((_annualReport.ImprovementNeeds != null ? $"{_annualReport.ImprovementNeeds}" : "Інформація відсутня"), font, XBrushes.Black, textArea, format);
        }
    }
}
