using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;

namespace EPlast.BussinessLayer
{
    public class BlankDocument : PdfDocument
    {
        private const string StatementPhoto = "wwwroot/images/pdf/Statement-photo.png";
        private readonly BlankModel blank;

        public BlankDocument(BlankModel blank, IPDFSettings settings) : base(settings)
        {
            this.blank = blank;
        }

        public override void SetDocumentBody(Section section)
        {
            SetHorizontalLine(section);

            SetParagraph(section, "Крайовому булавному УСП/УПС", 10, rightIndent: "-1cm", spaceBefore: "2mm")
                .Format.Alignment = ParagraphAlignment.Right;
            SetParagraph(section, "Від ТУТ ПІП", 10, rightIndent: "-1cm", spaceBefore: "1mm").Format.Alignment
                = ParagraphAlignment.Right; // Тут ПІП

            SetImage(section);

            SetParagraph(section,
                    "Прошу прийняти мене в дійсні члени Пласту – Національної Скаутської Організації України, " +
                    "до Уладу Старших Пластунів / Уладу Пластунів Сеньйорів.Даю слово честі, що буду дотримуватися Трьох Головних Обов’язків пластуна та положень Статуту Пласту - НСОУ",
                    10, rightIndent: "-1cm", leftIndent: "-1cm", spaceBefore: "1mm")
                .Format.FirstLineIndent = "1.25cm";

            SetParagraph(section,
                    "Відповідно до Закону України «Про захист персональних даних» надаю згоду проводу Пласту на " +
                    "обробку та використання моїх персональних даних (прізвище, ім'я, по-батькові, паспортні дані, ідентифікаційний номер, дані " +
                    "про освіту, дата народження, місце проживання, громадянство, стать, склад сім'ї, номери телефонів, електронні адреси, фотографії, " +
                    "інші персональні дані) з метою забезпечення реалізації відносин в сфері громадської діяльності. Також посвідчую, що повідомлення про " +
                    "включення даних про мене до бази персональних даних членів Пласту отримав/ла, із правами, які я маю відповідно до змісту ст. 8 Закону " +
                    "України «Про захист персональних даних» ознайомлений/на.", 10, rightIndent: "-1cm",
                    leftIndent: "-1cm")
                .Format.FirstLineIndent = "1.25cm";

            SetParagraph(section, "реєстрації в системі + осередокa", 12, "2mm",
                rightIndent: "9cm"); // Дата реги та осередок

            SetParagraph(section, "ПІБ", 12, "-0.5cm", leftIndent: "10cm"); // Тут ПІБ

            SetParagraph(section, "Анкета заявника", 14, "1.3cm",
                paragraphAlignment: ParagraphAlignment.Center);

            SetParagraph(section, "ПІБ:", rightIndent: "14cm", spaceBefore: "4mm", bold: true,
                paragraphAlignment: ParagraphAlignment.Right);

            SetParagraph(section, "Qwert QWErt Qwerty", leftIndent: "2.2cm", spaceBefore: "-4.8mm",
                bold: true); // тут ПІБ

            SetParagraph(section, "Дата народження:", leftIndent: "-2cm", rightIndent: "14cm", spaceBefore: "5mm",
                bold: true, paragraphAlignment: ParagraphAlignment.Center);

            SetParagraph(section, "21.01.1999", leftIndent: "2.2cm", spaceBefore: "-5mm",
                bold: true); // тут Дата народження

            SetParagraph(section, "E-mail:", rightIndent: "5cm", spaceBefore: "-4.9mm", bold: true,
                paragraphAlignment: ParagraphAlignment.Right);

            SetParagraph(section, "some@mail.com", leftIndent: "11.2cm", spaceBefore: "-4.9mm",
                bold: true); // тут Email

            SetParagraph(section, "Телефон:", leftIndent: "-2cm", rightIndent: "14cm", spaceBefore: "4.6mm", bold: true,
                paragraphAlignment: ParagraphAlignment.Right);

            SetParagraph(section, "+380666666", leftIndent: "2.2cm", spaceBefore: "-4.8mm", bold: true);

            SetParagraph(section, "Адреса:", leftIndent: "-2cm", rightIndent: "14cm", spaceBefore: "4mm", bold: true,
                paragraphAlignment: ParagraphAlignment.Right);

            SetParagraph(section, "м. Львів РНР", leftIndent: "2.2cm", spaceBefore: "-4.8mm",
                bold: true); // тут адреса проживання

            SetParagraph(section, "Праця/навчання:", leftIndent: "-2cm", rightIndent: "14cm", spaceBefore: "4.8mm",
                bold: true, paragraphAlignment: ParagraphAlignment.Right);

            SetParagraph(section, "тут багато тексту малоб бути, але я не знаю що писати", leftIndent: "2.2cm",
                spaceBefore: "-4.8cm", bold: true); // тут праця\навчання

            SetParagraph(section, "Вишколи:", leftIndent: "-2cm", rightIndent: "14cm", spaceBefore: "4.8mm", bold: true,
                paragraphAlignment: ParagraphAlignment.Right);

            SetParagraph(section, "Всі вишколи користувача", leftIndent: "2.2cm", spaceBefore: "-4.8mm",
                bold: true); // тут вишколи

            SetParagraph(section, "Поручення дійсних членів Пласту:", 14, "8mm",
                "4mm", paragraphAlignment: ParagraphAlignment.Center);

            SetApprove(section, "ступінь, ім’я, прізвище, курінь		дата		підпис");

            SetApprove(section, "ступінь, ім’я, прізвище, курінь		дата		підпис");

            SetApprove(section, "ступінь, ім’я, прізвище, курінь		дата		підпис");

            SetParagraph(section, "Поручення станичного або референта УСП/УПС", 14, "8mm",
                "4mm", paragraphAlignment: ParagraphAlignment.Center);

            SetParagraph(section,
                    "Я, @Ступінь, ім’я, по - батькові, курінь посада станиці@ " +
                    "поручаюся за кандидата в дійсні члени Пласту.Підверджую, що за час випробувального терміну " +
                    "кандидатом було виказано гідну пластову поставу, відповідність пластовим цінностям та " +
                    "проявлено відповідальність у взятих на себе пластових обов'язках @[ ] виховника; " +
                    "[ ] інструктора; [ ] адміністратора; [ ] суспільника@. Рекомендую надати " +
                    "кандидатустатус дійсного члена та право на складання пластової присяги.", 10,
                    leftIndent: "-1cm", rightIndent: "-1cm")
                .Format.FirstLineIndent = "1.25cm";

            SetParagraph(section, "дата поручення", 12, "2mm", rightIndent: "9cm");

            SetParagraph(section, "ПІБ", 12, "-0.5cm", leftIndent: "10cm"); //Тут ПІБ

            SetParagraph(section, "Поручення куреня УСП/УПС", 14, "8mm", "4mm",
                paragraphAlignment: ParagraphAlignment.Center);

            SetApprove(section, "Курінь УСП/УПС, пластовий ступінь, ім’я, прізвище, посада поручника, дата, підпис");

            SetParagraph(section, "Дата заприсяження, іменування: 66.66.6666", rightIndent: "-1cm", leftIndent: "-1cm",
                spaceBefore: "5mm", bold: true);
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

        private static Paragraph SetParagraph(Section section, string paragraphText, int fontSize = 11,
            string spaceBefore = "", string spaceAfter = "", string leftIndent = "",
            string rightIndent = "", bool bold = false, ParagraphAlignment paragraphAlignment = ParagraphAlignment.Left)
        {
            var paragraph = section.AddParagraph(paragraphText);
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = fontSize,
                    Bold = bold
                },
                Alignment = paragraphAlignment,
                LeftIndent = leftIndent,
                RightIndent = rightIndent,
                SpaceBefore = spaceBefore,
                SpaceAfter = spaceAfter
            };
            return paragraph;
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