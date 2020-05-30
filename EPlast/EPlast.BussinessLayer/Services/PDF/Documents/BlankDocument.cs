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

        private Paragraph SetParagraph(Section section, string paragraphText, int fontSize = 11,
            string spaceBefore = "", string spaceAfter = "", string leftIndent = "", string rightIndent = "")
        {
            var paragraph = section.AddParagraph(paragraphText);
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = fontSize
                },
                Alignment = ParagraphAlignment.Right,
                LeftIndent = leftIndent,
                RightIndent = rightIndent,
                SpaceBefore = spaceBefore,
                SpaceAfter = spaceAfter
            };
            return paragraph;
        }

        public override void SetDocumentBody(Section section)
        {
            SetHorizontalLine(section);

            var paragraph = section.AddParagraph("Крайовому булавному УСП/УПС");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 10
                },
                Alignment = ParagraphAlignment.Right,
                RightIndent = "-1cm",
                SpaceBefore = "2mm"
            };
            paragraph = section.AddParagraph("від	тут ПІП"); // Тут ПІП
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 10
                },
                Alignment = ParagraphAlignment.Right,
                RightIndent = "-1cm",
                SpaceBefore = "1mm"
            };
            SetImage(section);
            paragraph = section.AddParagraph(
                "Прошу прийняти мене в дійсні члени Пласту – Національної Скаутської Організації України, " +
                "до Уладу Старших Пластунів / Уладу Пластунів Сеньйорів.Даю слово честі, що буду дотримуватися Трьох Головних Обов’язків пластуна та положень Статуту Пласту - НСОУ");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 10
                },
                RightIndent = "-1cm",
                LeftIndent = "-1cm",
                SpaceBefore = "1mm",
                FirstLineIndent = "1.25cm"
            };
            paragraph = section.AddParagraph(
                "Відповідно до Закону України «Про захист персональних даних» надаю згоду проводу Пласту на " +
                "обробку та використання моїх персональних даних (прізвище, ім'я, по-батькові, паспортні дані, ідентифікаційний номер, дані " +
                "про освіту, дата народження, місце проживання, громадянство, стать, склад сім'ї, номери телефонів, електронні адреси, фотографії, " +
                "інші персональні дані) з метою забезпечення реалізації відносин в сфері громадської діяльності. Також посвідчую, що повідомлення про " +
                "включення даних про мене до бази персональних даних членів Пласту отримав/ла, із правами, які я маю відповідно до змісту ст. 8 Закону " +
                "України «Про захист персональних даних» ознайомлений/на.");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 10
                },
                RightIndent = "-1cm",
                LeftIndent = "-1cm",
                FirstLineIndent = "1.25cm"
            };
            paragraph = section.AddParagraph("дата реєстрації в системі +осередокa"); // Дата реги та осередок
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 12
                },
                SpaceBefore = "2mm",
                RightIndent = "9cm"
            };
            paragraph = section.AddParagraph("ПІБ"); // Тут ПІБ
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 12
                },
                SpaceBefore = "-.5cm",
                LeftIndent = "10cm"
            };
            paragraph = section.AddParagraph("Анкета заявника");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 14
                },
                Alignment = ParagraphAlignment.Center,
                SpaceBefore = "1.3cm"
            };

            paragraph = section.AddParagraph("ПІБ:");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                Alignment = ParagraphAlignment.Right,
                RightIndent = "14cm",
                SpaceBefore = "4mm"
            };
            paragraph = section.AddParagraph("Qwert QWErt Qwerty"); // тут ПІБ
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                Alignment = ParagraphAlignment.Left,
                LeftIndent = "2.2cm",
                SpaceBefore = "-4.8mm"
            };
            paragraph = section.AddParagraph("Дата народження:");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                Alignment = ParagraphAlignment.Right,
                RightIndent = "14cm",
                LeftIndent = "-2cm",
                SpaceBefore = "5mm"
            };
            paragraph = section.AddParagraph("21.01.1999"); // тут Дата народження
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                Alignment = ParagraphAlignment.Left,
                LeftIndent = "2.2cm",
                SpaceBefore = "-5mm"
            };
            paragraph = section.AddParagraph("E-mail:");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                Alignment = ParagraphAlignment.Right,
                RightIndent = "5cm",
                SpaceBefore = "-4.9mm"
            };
            paragraph = section.AddParagraph("some@mail.com"); // тут Email
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                Alignment = ParagraphAlignment.Left,
                LeftIndent = "11.2cm",
                SpaceBefore = "-4.9mm"
            };
            paragraph = section.AddParagraph("Телефон:");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                Alignment = ParagraphAlignment.Right,
                RightIndent = "14cm",
                LeftIndent = "-2cm",
                SpaceBefore = "4.6mm"
            };
            paragraph = section.AddParagraph("+380666666"); // тут телефон
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                Alignment = ParagraphAlignment.Left,
                LeftIndent = "2.2cm",
                SpaceBefore = "-4.8mm"
            };
            paragraph = section.AddParagraph("Адреса:");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                Alignment = ParagraphAlignment.Right,
                RightIndent = "14cm",
                LeftIndent = "-2cm",
                SpaceBefore = "4mm"
            };
            paragraph = section.AddParagraph("м. Львів РНР"); // тут адреса проживання
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                Alignment = ParagraphAlignment.Left,
                LeftIndent = "2.2cm",
                SpaceBefore = "-4.8mm"
            };
            paragraph = section.AddParagraph("Праця/навчання:");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                Alignment = ParagraphAlignment.Right,
                RightIndent = "14cm",
                LeftIndent = "-2cm",
                SpaceBefore = "4.8mm"
            };
            paragraph = section.AddParagraph(
                "тут багато тексту малоб бути, але я не знаю що писати"); // тут праця\навчання
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                Alignment = ParagraphAlignment.Left,
                LeftIndent = "2.2cm",
                SpaceBefore = "-4.8mm"
            };
            paragraph = section.AddParagraph("Вишколи:");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                Alignment = ParagraphAlignment.Right,
                RightIndent = "14cm",
                LeftIndent = "-2cm",
                SpaceBefore = "4.8mm"
            };
            paragraph = section.AddParagraph("Всі вишколи користувача"); // тут вишколи
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                Alignment = ParagraphAlignment.Left,
                LeftIndent = "2.2cm",
                SpaceBefore = "-4.8mm"
            };
            paragraph = section.AddParagraph("Поручення дійсних членів Пласту:");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 14
                },
                Alignment = ParagraphAlignment.Center,
                SpaceBefore = "8mm",
                SpaceAfter = "4mm",
            };

            SetApprove(section, "ступінь, ім’я, прізвище, курінь		дата		підпис");
            SetApprove(section, "ступінь, ім’я, прізвище, курінь		дата		підпис");
            SetApprove(section, "ступінь, ім’я, прізвище, курінь		дата		підпис");

            paragraph = section.AddParagraph("Поручення станичного або референта УСП/УПС");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 14
                },
                Alignment = ParagraphAlignment.Center,
                SpaceBefore = "8mm",
                SpaceAfter = "4mm",
            };

            paragraph = section.AddParagraph("Я, @Ступінь, ім’я, по - батькові, курінь посада станиці@ " +
                "поручаюся за кандидата в дійсні члени Пласту.Підверджую, що за час випробувального терміну " +
                "кандидатом було виказано гідну пластову поставу, відповідність пластовим цінностям та " +
                "проявлено відповідальність у взятих на себе пластових обов'язках @[ ] виховника; " +
                "[ ] інструктора; [ ] адміністратора; [ ] суспільника@. Рекомендую надати " +
                "кандидатустатус дійсного члена та право на складання пластової присяги.");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 10
                },
                RightIndent = "-1cm",
                LeftIndent = "-1cm",
                FirstLineIndent = "1.25cm"
            };
            paragraph = section.AddParagraph("дата поручення"); // Дата реги та осередок
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 12
                },
                SpaceBefore = "2mm",
                RightIndent = "9cm"
            };
            paragraph = section.AddParagraph("ПІБ"); // Тут ПІБ
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 12
                },
                SpaceBefore = "-.5cm",
                LeftIndent = "10cm"
            };
            paragraph = section.AddParagraph("Поручення куреня УСП/УПС");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 14
                },
                Alignment = ParagraphAlignment.Center,
                SpaceBefore = "8mm",
                SpaceAfter = "4mm",
            };
            SetApprove(section, "Курінь УСП/УПС, пластовий ступінь, ім’я, прізвище, посада поручника, дата, підпис");

            paragraph = section.AddParagraph("Дата заприсяження, іменування: 66.66.6666");
            paragraph.Format = new ParagraphFormat
            {
                Font = new Font
                {
                    Name = "Calibri",
                    Size = 11,
                    Bold = true
                },
                RightIndent = "-1cm",
                LeftIndent = "-1cm",
                SpaceBefore = "5mm"
            };
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
                    Size = 11,
                },
                Alignment = ParagraphAlignment.Left,
            };
        }
    }
}