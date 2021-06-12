using System;
using System.Linq;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;
using QRCoder;
using System.Drawing;

namespace EPlast.BLL
{
    public class BlankDocument : PdfDocument
    {
        private readonly BlankModel blank;


        public BlankDocument(BlankModel blank, IPdfSettings settings) : base(settings)
        {
            this.blank = blank;
        }


        public override void SetDocumentBody(PdfPage page, XGraphics gfx)
        {
            SetText(gfx, "Для осіб, які хочуть стати дійсними членами Пласту", XFontStyle.Regular, 180, 20);
            SetDashLine(gfx, 40, 40, 560, 40);
            SetText(gfx, "форма №1", XFontStyle.Regular, 50, 50);
            SetText(gfx, "Крайовому коменданту УСП / УПС", XFontStyle.Regular, 380, 55);
            SetText(gfx, $"від {blank?.User?.FirstName} {blank?.User?.LastName}", XFontStyle.Underline, 380, 65);
            SetText(gfx, "Заява", XFontStyle.Bold, 280, 90);
            SetText(gfx, "Прошу прийняти мене в дійсні члени Пласту – Національної Скаутської Організації України, до Уладу Старших",
                XFontStyle.Regular, 70, 110);
            SetText(gfx, "Пластунів / Уладу Пластунів Сеньйорів. Даю слово честі, що буду дотримуватися Трьох Головних Обов’язків пластуна",
                XFontStyle.Regular, 50, 120);
            SetText(gfx, "та положень Статуту Пласту НСОУ.", XFontStyle.Regular, 50, 130);
            SetText(gfx, "Відповідно до Закону України „Про захист персональних даних“ надаю згоду проводу Пласту на обробку та ",
                XFontStyle.Regular, 60, 140);
            SetText(gfx, "використання моїх персональних даних (прізвище, ім'я, по-батькові, паспортні дані, ідентифікаційний номер, дані",
                XFontStyle.Regular, 50, 150);
            SetText(gfx, "про освіту, дата народження, місце проживання, громадянство, стать, склад сім'ї, номери телефонів, електронні",
                XFontStyle.Regular, 50, 160);
            SetText(gfx, "адреси, фотографії, інші персональні дані) з метою забезпечення реалізації відносин в сфері громадської діяльності.",
                XFontStyle.Regular, 50, 170);
            SetText(gfx, "Також посвідчую, що повідомлення про включення даних про мене до бази персональних даних членів Пласту ",
                XFontStyle.Regular, 60, 180);
            SetText(gfx, "отримав/ла, із правами, які я маю відповідно до змісту ст. 8 Закону України „Про захист персональних даних“,",
                XFontStyle.Regular, 50, 190);
            SetText(gfx, "ознайомлений/на.", XFontStyle.Regular, 50, 200);

            SetText(gfx, "Поручення дійсних членів Пласту:", XFontStyle.Bold, 50, 230);
            int count = blank.User?.ConfirmedUsers != null ? blank.User.ConfirmedUsers.Count : 0;
            int countMember = 0;
            for (int i = 0, coordinates = 230; i < count; i++)
            {
                if (blank.User.ConfirmedUsers.ElementAt(i).isCityAdmin)
                {
                    SetText(gfx, $"{blank.User.ConfirmedUsers.ElementAt(i).Approver?.User?.FirstName} {blank.User.ConfirmedUsers?.ElementAt(i).Approver?.User?.LastName} ",
                    XFontStyle.Regular, 150, 643);
                    SetText(gfx, $" {blank.User.ConfirmedUsers?.ElementAt(i)?.ConfirmDate:dd.MM.yyyy}", XFontStyle.Italic, 435, 643);
                }
                else if (blank.User.ConfirmedUsers.ElementAt(i).isClubAdmin)
                {
                    SetText(gfx, $"{blank.User.ConfirmedUsers?.ElementAt(i).Approver?.User?.ClubMembers?.FirstOrDefault().Club?.Name}, {blank.User.ConfirmedUsers?.ElementAt(i).Approver?.User?.FirstName} {blank.User.ConfirmedUsers?.ElementAt(i).Approver?.User?.LastName} ",
                    XFontStyle.Regular, 180, 543);
                    SetText(gfx, $" {blank.User.ConfirmedUsers?.ElementAt(i)?.ConfirmDate:dd.MM.yyyy}", XFontStyle.Italic, 435, 568);
                }
                else
                {
                    SetText(gfx, $"{countMember + 1}. {blank.User.ConfirmedUsers?.ElementAt(i).Approver?.User?.FirstName} {blank.User.ConfirmedUsers?.ElementAt(i).Approver?.User?.LastName} " +
                    $" {blank.User.ConfirmedUsers?.ElementAt(i)?.ConfirmDate:dd.MM.yyyy}",
                    XFontStyle.Regular, 50, coordinates += 20);
                    countMember += 1;
                }
            }


            SetText(gfx, $"{DateTime.Now:dd.MM.yyyy}, {blank?.CityMembers?.City?.Name}", XFontStyle.Underline, 80, 310);
            SetLine(gfx, 370, 310, 460, 310);
            SetText(gfx, $"({blank.User.LastName} {blank.User.FirstName[0]}. {(blank.User.FatherName != null ? blank.User.FatherName[0] + "." : "")})", XFontStyle.Italic, 463, 300);
            SetText(gfx, "Підпис Заявника", XFontStyle.Italic, 410, 310);

            SetDashLine(gfx, 40, 330, 560, 330);

            SetText(gfx, "Анкета заявника", XFontStyle.Bold, 60, 340);
            SetText(gfx, "(точно заповнює заявник та місцевий референт УСП / УПС)", XFontStyle.Italic, 133, 340);
            SetText(gfx, "Прізвище", XFontStyle.Regular, 50, 360);
            SetLine(gfx, 110, 370, 290, 370);
            SetText(gfx, $"{blank.User.LastName}", XFontStyle.Italic, 160, 358);
            SetText(gfx, "Ім’я", XFontStyle.Regular, 320, 360);
            SetLine(gfx, 360, 370, 550, 370);
            SetText(gfx, $"{blank.User.FirstName}", XFontStyle.Italic, 420, 358);
            SetText(gfx, "По-батькові", XFontStyle.Regular, 50, 375);
            SetLine(gfx, 110, 385, 290, 385);
            SetText(gfx, $"{blank.User.FatherName}", XFontStyle.Italic, 160, 373);
            SetText(gfx, "Дата народження", XFontStyle.Regular, 300, 375);
            SetLine(gfx, 400, 385, 550, 385);
            SetText(gfx, $"{blank?.UserProfile?.Birthday:dd.MM.yyyy}", XFontStyle.Italic, 440, 373);
            SetText(gfx, "Домашня адреса, індекс", XFontStyle.Regular, 50, 390);
            SetLine(gfx, 160, 400, 550, 400);
            SetText(gfx, $"{blank?.UserProfile?.Address}", XFontStyle.Italic, 280, 388);
            SetText(gfx, "Дом. тел", XFontStyle.Regular, 50, 405);
            SetLine(gfx, 110, 415, 290, 415);
            SetText(gfx, $"{blank?.User?.PhoneNumber}", XFontStyle.Italic, 160, 403);
            SetText(gfx, "Ел. пошта", XFontStyle.Regular, 300, 405);
            SetLine(gfx, 360, 415, 550, 415);
            SetText(gfx, $"{blank?.User?.Email}", XFontStyle.Italic, 390, 403);
            SetText(gfx, "Місце навчання", XFontStyle.Regular, 50, 420);
            SetLine(gfx, 130, 430, 550, 430);
            SetText(gfx, $"{blank?.UserProfile?.Education?.PlaceOfStudy}", XFontStyle.Italic, 150, 418);
            SetText(gfx, "Місце праці", XFontStyle.Regular, 50, 435);
            SetLine(gfx, 130, 445, 550, 445);
            SetText(gfx, $"{blank?.UserProfile?.Work?.PlaceOfwork}", XFontStyle.Italic, 150, 433);
            SetText(gfx, "Вишкіл виховників", XFontStyle.Regular, 50, 450);
            SetText(gfx, $"УПЮ/УПН", XFontStyle.Italic, 50, 460);

            var participantsUPU = blank?.User?.Participants?.Where(c => c.Event?.EventDateEnd < DateTime.Now &&
            c?.ParticipantStatusId == 1 &&
            (c.Event.EventCategory.EventSection.EventSectionName == "УПЮ" ||
            c.Event.EventCategory.EventSection.EventSectionName == "УПН")).Select(c => c.Event.EventName).ToList();
            var participantsUSP = blank?.User?.Participants?.Where(c => c.Event?.EventDateEnd < DateTime.Now &&
            c?.ParticipantStatusId == 1 && c.Event.EventCategory.EventSection.EventSectionName == "УСП/УПС").Select(c => c.Event.EventName).ToList();
            if (participantsUPU != null)
            {
                var resultUPU = String.Join("; ", participantsUPU);
                SetText(gfx, $"{resultUPU}", XFontStyle.Regular, 190, 450);
            }
            if (participantsUSP != null)
            {
                var resultUSP = String.Join("; ", participantsUSP);
                SetText(gfx, $"{resultUSP}", XFontStyle.Regular, 190, 475);
            }
            SetLine(gfx, 165, 460, 550, 460);
            SetText(gfx, "Інші вишколи УСП/УПС", XFontStyle.Regular, 50, 475);
            SetText(gfx, "(назва, тип, число, дати)", XFontStyle.Regular, 50, 485);
            SetLine(gfx, 165, 485, 550, 485);
            SetText(gfx, "Орієнтація до куреня УСП /", XFontStyle.Regular, 50, 500);
            SetText(gfx, "УПС", XFontStyle.Regular, 50, 510);
            SetText(gfx, $"{blank?.ClubMembers?.Club?.Name}", XFontStyle.Italic, 180, 498);
            SetLine(gfx, 165, 510, 550, 510);

            SetDashLine(gfx, 40, 530, 560, 530);

            SetText(gfx, "Поручення куреня УСП /", XFontStyle.Regular, 50, 540);
            SetText(gfx, "УПС (назва куреня і підпис", XFontStyle.Regular, 50, 550);
            SetText(gfx, "його представника)", XFontStyle.Regular, 50, 560);
            SetLine(gfx, 165, 555, 550, 555);
            SetLine(gfx, 50, 580, 390, 580);
            SetText(gfx, "Дата", XFontStyle.Regular, 400, 570);
            SetLine(gfx, 430, 580, 500, 580);

            SetDashLine(gfx, 40, 595, 560, 595);

            SetText(gfx, "Поручення для прийняття в дійсні члени Пласту – НСОУ, на основі виконаних вимог кандидата.", XFontStyle.Bold, 50, 600);
            SetText(gfx, "Осередковий", XFontStyle.Regular, 50, 615);
            SetText(gfx, "УСП / УПС", XFontStyle.Regular, 50, 625);
            SetLine(gfx, 120, 630, 390, 630);
            SetText(gfx, "Дата", XFontStyle.Regular, 400, 620);
            SetLine(gfx, 430, 630, 500, 630);

            SetText(gfx, "Станичний", XFontStyle.Regular, 50, 645);
            SetLine(gfx, 120, 655, 390, 655);
            SetText(gfx, "Дата", XFontStyle.Regular, 400, 645);
            SetLine(gfx, 430, 655, 500, 655);

            SetDashLine(gfx, 40, 670, 560, 670);

            SetText(gfx, "Дата рішення Крайового органу про прийняття в дійсні члени", XFontStyle.Regular, 50, 680);
            var plastDegree = blank.User?.UserPlastDegrees?.FirstOrDefault(c => c.IsCurrent);
            SetText(gfx, $"{plastDegree?.DateStart:dd.MM.yyyy}", XFontStyle.Italic, 380, 680);

            SetLine(gfx, 350, 690, 500, 690);
            SetText(gfx, "Дата заприсяження, іменування", XFontStyle.Regular, 50, 700);
            if (blank?.User?.UserMembershipDates?.FirstOrDefault()?.DateOath == DateTime.MinValue)
            {
                SetText(gfx, $"Без присяги", XFontStyle.Italic, 260, 698);
            }
            else
            {
                SetText(gfx, $"{blank?.User?.UserMembershipDates?.FirstOrDefault()?.DateOath.ToString("dd.MM.yyyy")}", XFontStyle.Italic, 260, 698);
            }
            SetLine(gfx, 230, 710, 500, 710);

            SetDashLine(gfx, 40, 717, 560, 717);

            SetText(gfx, $"Номер користувача в системі - {blank?.UserProfile?.ID}", XFontStyle.Regular, 50, 727);

            DrawQRCode(gfx);
        }

        private static void SetText(XGraphics gfx, string text, XFontStyle style, double x, double y)
        {
            const string facename = "Calibri";

            XStringFormat format = new XStringFormat();
            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);
            XFont font = new XFont(facename, 8, style, options);

            gfx.DrawString(text, font, XBrushes.Black, x, y, format);
        }

        private static void SetDashLine(XGraphics gfx, double x1, double y1, double x2, double y2)
        {
            XPen pen = new XPen(XColors.Black, 0.5);
            pen.DashStyle = XDashStyle.Dash;
            gfx.DrawLine(pen, x1, y1, x2, y2);
        }
        private static void SetLine(XGraphics gfx, double x1, double y1, double x2, double y2)
        {
            XPen pen = new XPen(XColors.Black);
            gfx.DrawLine(pen, x1, y1, x2, y2);
        }

        private void DrawQRCode(XGraphics gfx)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("https://eplast.westeurope.cloudapp.azure.com/userpage/main/" + $"{blank.User.Id}", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(1);
            using var ms = new MemoryStream();
            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            XImage xImage = XImage.FromStream(() => new MemoryStream(ms.ToArray()));
            gfx.DrawImage(xImage, 480, 720);
        }

    }
}