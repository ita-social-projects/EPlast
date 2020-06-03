using System;
using System.Linq;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;

namespace EPlast.BussinessLayer
{
    public class BlankDocument : PdfDocument
    {
        private const string StatementPhoto = "wwwroot/images/pdf/Statement-photo.png";
        private readonly BlankModel blank;

        public BlankDocument(BlankModel blank, IPdfSettings settings) : base(settings)
        {
            this.blank = blank;
        }

        public override void SetDocumentBody(Section section)
        {
            SetHorizontalLine(section);

            SetParagraph(section, "Крайовому булавному УСП/УПС", SetParagraphFont(10),
                    rightIndent: "-1cm", spaceBefore: "2mm")
                .Format.Alignment = ParagraphAlignment.Right;
            SetParagraph(section, $"Від {blank.User.LastName} {blank.User.FirstName} {blank.User.FatherName}",
                    SetParagraphFont(10), rightIndent: "-1cm", spaceBefore: "1mm")
                .Format.Alignment = ParagraphAlignment.Right; // Тут ПІП

            SetImage(section);

            SetParagraph(section,
                    "Прошу прийняти мене в дійсні члени Пласту – Національної Скаутської Організації України, " +
                    "до Уладу Старших Пластунів / Уладу Пластунів Сеньйорів.Даю слово честі, що буду дотримуватися Трьох Головних Обов’язків пластуна та положень Статуту Пласту - НСОУ",
                    SetParagraphFont(10), rightIndent: "-1cm", leftIndent: "-1cm",
                    spaceBefore: "1mm")
                .Format.FirstLineIndent = "1.25cm";

            SetParagraph(section,
                    "Відповідно до Закону України «Про захист персональних даних» надаю згоду проводу Пласту на " +
                    "обробку та використання моїх персональних даних (прізвище, ім'я, по-батькові, паспортні дані, ідентифікаційний номер, дані " +
                    "про освіту, дата народження, місце проживання, громадянство, стать, склад сім'ї, номери телефонів, електронні адреси, фотографії, " +
                    "інші персональні дані) з метою забезпечення реалізації відносин в сфері громадської діяльності. Також посвідчую, що повідомлення про " +
                    "включення даних про мене до бази персональних даних членів Пласту отримав/ла, із правами, які я маю відповідно до змісту ст. 8 Закону " +
                    "України «Про захист персональних даних» ознайомлений/на.", SetParagraphFont(10),
                    rightIndent: "-1cm", leftIndent: "-1cm")
                .Format.FirstLineIndent = "1.25cm";

            SetParagraph(section, $"{blank.User.RegistredOn} {blank.CityMembers.City.Name}",
                SetParagraphFont(12), "2mm", rightIndent: "9cm"); // Дата реги та осередок

            SetParagraph(section, "ПІБ", SetParagraphFont(12), "-0.5cm",
                leftIndent: "10cm"); // Тут ПІБ

            SetParagraph(section, "Анкета заявника", SetParagraphFont(14), "1.3cm",
                paragraphAlignment: ParagraphAlignment.Center);

            SetParagraph(section, "ПІБ:", SetParagraphFont(bold: true), rightIndent: "14cm",
                spaceBefore: "4mm", paragraphAlignment: ParagraphAlignment.Right);

            SetParagraph(section, $"{blank.User.LastName} {blank.User.FirstName} {blank.User.FatherName}",
                SetParagraphFont(bold: true), leftIndent: "2.2cm", spaceBefore: "-4.8mm"); // тут ПІБ

            SetParagraph(section, "Дата народження:", SetParagraphFont(bold: true), leftIndent: "-2cm",
                rightIndent: "14cm", spaceBefore: "5mm", paragraphAlignment: ParagraphAlignment.Center);

            SetParagraph(section, $"{blank.UserProfile.Birthday}", SetParagraphFont(bold: true),
                leftIndent: "2.2cm", spaceBefore: "-5mm"); // тут Дата народження

            SetParagraph(section, "E-mail:", SetParagraphFont(bold: true), rightIndent: "5cm",
                spaceBefore: "-4.9mm", paragraphAlignment: ParagraphAlignment.Right);

            SetParagraph(section, $"{blank.User.Email}", SetParagraphFont(bold: true),
                leftIndent: "11.2cm", spaceBefore: "-4.9mm"); // тут Email

            SetParagraph(section, "Телефон:", SetParagraphFont(bold: true), leftIndent: "-2cm",
                rightIndent: "14cm", spaceBefore: "4.6mm", paragraphAlignment: ParagraphAlignment.Right);

            SetParagraph(section, $"{blank.User.PhoneNumber}", SetParagraphFont(bold: true),
                leftIndent: "2.2cm", spaceBefore: "-4.8mm");

            SetParagraph(section, "Адреса:", SetParagraphFont(bold: true), leftIndent: "-2cm",
                rightIndent: "14cm", spaceBefore: "4mm", paragraphAlignment: ParagraphAlignment.Right);

            SetParagraph(section, $"{blank.UserProfile.Address}", SetParagraphFont(bold: true),
                leftIndent: "2.2cm", spaceBefore: "-4.8mm"); // тут адреса проживання

            SetParagraph(section, "Праця/навчання:", SetParagraphFont(bold: true), leftIndent: "-2cm",
                rightIndent: "14cm", spaceBefore: "4.8mm", paragraphAlignment: ParagraphAlignment.Right);

            SetParagraph(section, $"{blank.UserProfile.Education} {blank.UserProfile.Work}",
                SetParagraphFont(bold: true), leftIndent: "2.2cm",
                spaceBefore: "-4.8cm"); // тут праця\навчання

            SetParagraph(section, "Вишколи:", SetParagraphFont(bold: true), leftIndent: "-2cm",
                rightIndent: "14cm", spaceBefore: "4.8mm", paragraphAlignment: ParagraphAlignment.Right);

            SetParagraph(section, $"{blank.User.Events}", SetParagraphFont(bold: true),
                leftIndent: "2.2cm", spaceBefore: "-4.8mm"); // тут вишколи

            SetParagraph(section, "Поручення дійсних членів Пласту:", SetParagraphFont(14),
                "8mm", "4mm", paragraphAlignment: ParagraphAlignment.Center);

            foreach (var approver in blank.User.Approvers)
                SetApprove(section,
                    $"${approver.User.UserPlastDegrees}, {approver.User.FirstName}, {approver.User.LastName}, {approver.User.ClubMembers}\t" +
                    $"{approver.ConfirmedUser.ConfirmDate}\t" +
                    $"{approver.User.LastName} {approver.User.FirstName} {approver.User.FatherName}");

            SetParagraph(section, "Поручення станичного або референта УСП/УПС",
                SetParagraphFont(14), "8mm", "4mm",
                paragraphAlignment: ParagraphAlignment.Center);

            SetParagraph(section,
                    $"Я, {blank.CityAdmin.UserPlastDegrees}, {blank.CityAdmin.FirstName}, {blank.CityAdmin.FatherName}, {blank.ClubMembers.Club.ClubName} {blank.CityAdmin.UserPlastDegrees} " +
                    "поручаюся за кандидата в дійсні члени Пласту.Підверджую, що за час випробувального терміну " +
                    "кандидатом було виказано гідну пластову поставу, відповідність пластовим цінностям та " +
                    "проявлено відповідальність у взятих на себе пластових обов'язках @[ ] виховника; " + // ????
                    "[ ] інструктора; [ ] адміністратора; [ ] суспільника@. Рекомендую надати " +
                    "кандидатустатус дійсного члена та право на складання пластової присяги.",
                    SetParagraphFont(10), leftIndent: "-1cm", rightIndent: "-1cm")
                .Format.FirstLineIndent = "1.25cm";

            SetParagraph(section,
                $"{blank.User.Approvers.First(x => x.UserID.Equals(blank.CityAdmin.Id)).ConfirmedUser.ConfirmDate}",
                SetParagraphFont(12), "2mm", rightIndent: "9cm");

            SetParagraph(section,
                $"{blank.CityAdmin.LastName} {blank.CityAdmin.FirstName} {blank.CityAdmin.FatherName}",
                SetParagraphFont(12), "-0.5cm", leftIndent: "10cm"); //Тут ПІБ

            SetParagraph(section, $"Дата заприсяження, іменування: {DateTime.Now:dd/MM/yyyy}",
                SetParagraphFont(bold: true), rightIndent: "-1cm", leftIndent: "-1cm",
                spaceBefore: "5mm");
        }

        private static void SetHorizontalLine(Section section)
        {
            const double height = 0.1;
            var hrFillColor = Colors.DarkSlateGray;
            var hrBorderColor = Colors.DarkSlateGray;

            var paragraph = section.AddParagraph();
            var newBorder = new Border { Style = BorderStyle.Single, Color = hrBorderColor, Width = height };

            paragraph.Format = new ParagraphFormat
            {
                Font = new Font("Courier New", Unit.FromMillimeter(height)),
                Shading = new Shading { Visible = true, Color = hrFillColor },
                Borders = new Borders
                {
                    Color = Colors.Gray,
                    Bottom = newBorder,
                    Left = newBorder.Clone(),
                    Right = newBorder.Clone(),
                    Top = newBorder.Clone()
                },
                SpaceBefore = "-7.2mm",
                LeftIndent = "2mm",
                RightIndent = "-1cm"
            };
        }

        private static Paragraph SetParagraph(Section section, string paragraphText, Font paragraphFont,
            string spaceBefore = "", string spaceAfter = "", string leftIndent = "",
            string rightIndent = "", ParagraphAlignment paragraphAlignment = ParagraphAlignment.Left)
        {
            var paragraph = section.AddParagraph(paragraphText);
            paragraph.Format = new ParagraphFormat
            {
                Font = paragraphFont,
                Alignment = paragraphAlignment,
                LeftIndent = leftIndent,
                RightIndent = rightIndent,
                SpaceBefore = spaceBefore,
                SpaceAfter = spaceAfter
            };
            return paragraph;
        }

        private static Font SetParagraphFont(int fontSize = 11, bool bold = false)
        {
            return new Font
            {
                Name = "Calibri",
                Size = fontSize,
                Bold = bold
            };
        }

        private static void SetImage(Section section)
        {
            var image = section.AddImage(StatementPhoto);
            image.Width = "2.03cm";
            image.Height = "0.62cm";
            image.Left = ShapePosition.Center;
        }

        private static void SetApprove(Section section, string text)
        {
            var paragraph = section.AddParagraph($"1. {text}"); // тут апрувери
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11
                },
                Alignment = ParagraphAlignment.Left
            };
        }
    }
}