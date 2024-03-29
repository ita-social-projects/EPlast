﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Course;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Entities.GoverningBody.Sector;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess.ExtensionMethods
{
    public static class ModelBuilderExtensions
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UpuDegree>().HasData(
                new UpuDegree
                {
                    ID = 1,
                    Name = "не був/-ла в юнацтві"
                },
                new UpuDegree
                {
                    ID = 2,
                    Name = "пластун/-ка учасник/-ця"
                }, new UpuDegree
                {
                    ID = 3,
                    Name = "пластун/-ка розвідувач/-ка"
                },
                new UpuDegree
                {
                    ID = 4,
                    Name = "пластун скоб / пластунка вірлиця"
                });

            modelBuilder.Entity<AdminType>().HasData(
                new AdminType
                {
                    ID = 1,
                    AdminTypeName = "Admin"
                },
                new AdminType
                {
                    ID = 2,
                    AdminTypeName = "Прихильник"
                },
                new AdminType
                {
                    ID = 3,
                    AdminTypeName = "Дійсний член організації"
                },
                new AdminType
                {
                    ID = 4,
                    AdminTypeName = "Голова Пласту"
                },
                new AdminType
                {
                    ID = 5,
                    AdminTypeName = "Адміністратор подій"
                },
                new AdminType
                {
                    ID = 6,
                    AdminTypeName = "Голова Куреня"
                },
                new AdminType
                {
                    ID = 7,
                    AdminTypeName = "Діловод Куреня"
                },
                new AdminType
                {
                    ID = 8,
                    AdminTypeName = "Голова Округи"
                },
                new AdminType
                {
                    ID = 9,
                    AdminTypeName = "Діловод Округи"
                },
                new AdminType
                {
                    ID = 10,
                    AdminTypeName = "Голова Станиці"
                },
                new AdminType
                {
                    ID = 11,
                    AdminTypeName = "Діловод Станиці"
                },
                new AdminType
                {
                    ID = 12,
                    AdminTypeName = "Колишній член Пласту"
                },
                new AdminType
                {
                    ID = 13,
                    AdminTypeName = "Зареєстрований користувач"
                },
                new AdminType
                {
                    ID = 14,
                    AdminTypeName = "Зацікавлений"
                },
                new AdminType
                {
                    ID = 15,
                    AdminTypeName = "Заступник Голови Станиці"
                },
                new AdminType
                {
                    ID = 16,
                    AdminTypeName = "Заступник Голови Округи"
                },
                new AdminType
                {
                    ID = 17,
                    AdminTypeName = "Заступник Голови Куреня"
                },
                new AdminType
                {
                    ID = 18,
                    AdminTypeName = "Голова Краю"
                },
                new AdminType
                {
                    ID = 19,
                    AdminTypeName = "Крайовий Адмін"
                },
                new AdminType
                {
                    ID = 20,
                    AdminTypeName = "Голова Керівного Органу"
                },
                new AdminType
                {
                    ID = 21,
                    AdminTypeName = "Діловод Керівного Органу"
                },
                new AdminType
                {
                    ID = 22,
                    AdminTypeName = "Голова Напряму Керівного Органу"
                },
                new AdminType
                {
                    ID = 23,
                    AdminTypeName = "Діловод Напряму Керівного Органу"
                },
                new AdminType
                {
                    ID = 24,
                    AdminTypeName = "Референт/-ка УПС Округи"
                },
                new AdminType
                {
                    ID = 25,
                    AdminTypeName = "Референт/-ка УСП Округи"
                },
                new AdminType
                {
                    ID = 26,
                    AdminTypeName = "Референт дійсного членства Округи"
                },
                new AdminType
                {
                    ID = 27,
                    AdminTypeName = "Референт/-ка УПС Станиці"
                },
                new AdminType
                {
                    ID = 28,
                    AdminTypeName = "Референт/-ка УСП Станиці"
                },
                new AdminType
                {
                    ID = 29,
                    AdminTypeName = "Референт дійсного членства Станиці"
                });

            modelBuilder.Entity<Nationality>().HasData(
                new Nationality
                {
                    ID = 1,
                    Name = "Українка"
                },
                new Nationality
                {
                    ID = 2,
                    Name = "Українець"
                });

            modelBuilder.Entity<Gender>().HasData(
                new Gender
                {
                    ID = 1,
                    Name = "Чоловік"
                },
                new Gender
                {
                    ID = 2,
                    Name = "Жінка"
                },
                new Gender
                {
                    ID = 7,
                    Name = "Не маю бажання вказувати"
                });

            modelBuilder.Entity<Region>().HasData(
                new Region
                {
                    ID = 1,
                    RegionName = "Крайовий Провід Пласту",
                    Description =
                        "Пласт — українська скаутська організація. Метою Пласту є сприяти всебічному, патріотичному вихованню та самовихованню української молоді. Пласт виховує молодь як свідомих, відповідальних і повновартісних громадян місцевої, національної та світової спільноти, провідників суспільства.  Також Пласт є неполітичною та позаконфесійною організацією.  Пласт був створений у 1911 році, невдовзі після заснування скаутського руху Робертом Бейден-Пауелом в 1907 р.,  а вже 12 квітня 1912 року у Львові пластуни склали першу Пластову присягу. Серед засновників організації були д-р. Олександр Тисовський, Петро Франко (син Івана Франка) та Іван Чмола.  В основі назви “Пласт” лежить відповідник англійського Scout (розвідник), взятий за прикладом пластунів-козаків-розвідників. Гербом Пласту є трилиста квітка лілії — символ скаутського руху відомий як Fleur-de-lis — та тризуб, сплетенів одну гармонійну цілісність.  Для досягнення виховних цілей Пласт застосовує власну унікальну методу виховання.",

                    City = "Львів",
                    PhoneNumber = null,
                    Email = null,
                    Link = null,
                    Logo = null,
                    IsActive = true,
                    Street = null,
                    HouseNumber = null,
                    OfficeNumber = null,
                    PostIndex = 0,
                    Status = RegionsStatusType.RegionBoard
                }
            );

            modelBuilder.Entity<EducatorsStaffTypes>().HasData(
                new EducatorsStaffTypes
                {
                    ID = 1,
                    Name = "КВ1(УПН)"
                },
                new EducatorsStaffTypes
                {
                    ID = 2,
                    Name = "КВ1(УПЮ)"
                }, new EducatorsStaffTypes
                {
                    ID = 3,
                    Name = "КВ2(УПН)"
                }, new EducatorsStaffTypes
                {
                    ID = 4,
                    Name = "КВ2(УПЮ)"
                }
            );

            modelBuilder.Entity<Terms>().HasData(
                new Terms
                {
                    TermsId = 1,
                    TermsTitle = "Політика конфіденційності",
                    TermsText =
                        "<p><em><u>A. Вступ</u></em></p><ul><li><em><u>Конфіденційність користувачів нашого веб-сайту має велике значення для нас, і ми докладаємо всіх зусиль для забезпечення її захисту. Дані правила пояснюють, як ми використовуємо вашу персональну інформацію.</u></em></li><li><em><u>Погоджуючись із використанням файлів-cookie при першому відвіданні нашого веб-сайту, відповідно до положень даних Правил, ви надаєте нам дозвіл на використання файлів-cookie при кожному вашому наступному візиті.</u></em></li></ul><p><em><u>Б. Права інтелектуальної власності</u></em></p><p>Цей документ був створений з використанням шаблона із сайту SEQ Legal (seqlegal.com) та модифікований веб-сайтом Website Planet (<a href=\"https://www.websiteplanet.com/\" rel=\"noopener noreferrer\" target=\"_blank\" style=\"color: rgb(90, 50, 174);\">www.websiteplanet.com</a>)</p><p><strong>В. Збір персональних даних</strong></p><p>Збору, зберіганню та використанню підлягають наступні типи персональних даних:</p><ol><li>Інформація про ваш комп’ютер, включаючи вашу IP-адресу, географічне розташування, тип і версію браузера та операційну систему;</li><li><strong><em>Інформація про ваші відвідування та використання цього веб-сайту, включаючи реферальні джерела, протяжність візитів, переглянуті сторінки та шляхи навігації по сайту;</em></strong></li><li><strong><em>Інформація про адресу вашої електронної пошти, яку ви використали для реєстрації на нашому веб-сайті;</em></strong></li><li><strong><em>Інформація, яку ви ввели під час створення профілю на нашому веб-сайті – наприклад, ваше ім’я, зображення у вашому профілі, стать, дата народження, сімейний статус, хобі та інтереси, освіта та місце роботи;</em></strong></li><li>Інформація, така, як ваше ім’я та адреса електронної пошти, які ви вказали під час оформлення підписок на наші поштові повідомлення та/або розсилки;</li><li>Інформація, яку ви ввели під час використання сервісів нашого веб-сайту;</li><li>Інформація, яка генерується при використанні нашого веб-сайту, включаючи інформацію про час, частоту та умови його використання;</li><li>Інформація стосовно будь-яких ваших покупок, використаних сервісів або транзакцій, які ви провели через наш веб-сайт, включаючи ваше ім’я, адресу, номер телефону, адресу електронної поштової скриньки та інформацію про кредитну карту;</li><li>Інформація, яку ви розмістили на нашому сайті з метою публікації її в Інтернеті, включаючи ваше ім’я користувача, зображення профілю та зміст вашої публікації;</li><li>Інформація, що міститься в будь-яких повідомленнях, які ви надсилали нам електронною поштою або через наш веб-сайт, включаючи зміст повідомлення та мета дані;</li><li>Будь-яка інша персональна інформація, яку ви надіслали нам.</li><li>Перед тим, як розкрити для нас персональну інформацію третьої особи, ви маєте отримати згоду цієї особи як на розкриття, так і на обробку цієї інформації у відповідності до даних правил.</li></ol><p>Перед тим, як розкрити для нас персональну інформацію третьої особи, ви маєте отримати згоду цієї особи як на розкриття, так і на обробку цієї інформації у відповідності до даних правил.</p><p><strong>Г. Використання вашої персональної інформації</strong></p><p>Персональна інформація, яку ви передані нам через наш веб-сайт, буде використовуватися задля цілей, зазначених в цих правилах або на відповідних сторінках веб-сайту. Ми можемо використовувати вашу персональну інформацію в наступних цілях:</p><ol><li>адміністрування нашого веб-сайту та бізнесу;</li><li>персоналізація нашого веб-сайту для вас;</li><li>надання вам можливості користуватися сервісами, доступними на нашому веб-сайті;</li><li>надсилання вам товарів, придбаних через наш-веб-сайт;</li><li>надання вам послуг, придбаних через наш веб-сайт;</li><li>надсилання вам повідомлень, рахунків та нагадувань про сплату, та отримання платежів від вас;</li><li>надсилання вам немаркетингових комерційних повідомлень;</li><li>надсилання вам електронною поштою повідомлень, які ви спеціально запросили;</li><li>надсилання вам електронної розсилки, якщо ви її замовили (ви в будь-який момент можете повідомити нас, що більше не бажаєте отримувати електронні розсилки від нас);</li><li>надсилання вам маркетингових повідомлень стосовно нашої ділової активності або ділової активності старанно відібраних сторонніх компаній, яка, на нашу думку, може вас зацікавити, шляхом публікацій або, якщо ви конкретно надали на це згоду – шляхом надсилання електронної пошти або за рахунок використання подібних технологій (ви в будь-який момент можете повідомити нас, що більше не бажаєте отримувати маркетингові повідомлення);</li><li>надання стороннім компаніям статистичної інформації про наших користувачів (проте, ці сторонні компанії не матимуть змоги ідентифікувати жодного окремого користувача з цих даних);</li><li>обробка запитів та скарг, зроблених вами або на вас, і які стосуються нашого веб-сайту;</li><li>з метою забезпечення безпеки нашого сайту та попередження шахрайства;</li><li>з метою перевірки відповідності умовам та правилам, які регламентують використання нашого веб-сайту (включаючи моніторинг приватних повідомлень, надісланих через сервіс приватних повідомлень нашого веб-сайту); та</li><li>в інших цілях.</li></ol><p>Якщо ви надали персональну інформацію для публікації на нашому веб-сайті, ми опублікуємо її. В іншому випадку, ми використовуватимемо цю інформацію у відповідності до ліцензії, яку ви нам надали.</p><p>Ваші налаштування конфіденційності можуть використовуватись для обмеження публікації ваших персональних даних на нашому веб-сайті, і можуть регулюватися за допомогою засобів управління конфіденційністю на веб-сайті.</p><p>Без вашої чітко вираженої згоди ми не будемо передавати вашу персональну інформацію жодній сторонній компанії для прямого маркетингового використання цією, або будь-якою іншою сторонньою компанією.</p><p><strong>Д. Розкриття персональної інформації</strong></p><p>Ми лишаємо за собою право розкрити вашу персональну інформацію для будь-якого з наших працівників, керівників, страхувальників, професійних радників, агентів, постачальників або субпідрядників, в об’ємі та цілях, визначених в цих правилах.</p><p>Ми за собою право розкрити вашу персональну інформацію для будь-якого члена нашої групи компаній (сюди входять наші дочірні компанії, наша кінцева холдингова компанія та всі її дочірні компанії) в об’ємі та цілях, визначених в цих правилах.</p><p>Ми лишаємо за собою право розкрити вашу персональну інформацію:</p><ol><li>в тих випадках, в яких цього від нас вимагає закон;</li><li>у зв’язку з будь-яким поточними або майбутніми судовими процесами;</li><li>з метою встановлення, реалізації або захисту наших законних прав (включаючи надання інформації іншим сторонам задля запобігання шахрайству або зниження кредитних ризиків);</li><li>покупцеві (або потенційному покупцеві) будь-якого бізнесу або активів, які ми продаємо (або збираємося продати); та</li><li>будь-якій особі, яка, як ми обґрунтовано вважаємо, може подати запит до суду або іншого уповноваженого органу про розкриття цих персональних даних і, на нашу обґрунтовану думку, цей суд або уповноважений орган видасть розпорядження на розкриття цих персональних даних.</li></ol><p>Ми не будемо розкривати вашу персональну інформацію третім особам, за виключенням випадків, зазначених в цих правилах.</p><p><strong>Е. Міжнародні передачі персональної інформації</strong></p><ol><li>Інформація, яку ми збираємо, може зберігатися, оброблятися та передаватися між будь-якими країнами, в яких ми здійснюємо свою діяльність, з метою надання нам можливості використовувати цю інформацію у відповідності з цими правилами.</li><li>Інформація, яку ми збираємо, може бути передана в наступні країни, де немає законів із захисту даних, аналогічних тим, що діють на території Європейської Економічної Зони: США, Росія, Японія, Китай та Індія.</li><li>Персональна інформація, які ви публікуєте на нашому веб-сайті, через Інтернет, може бути доступна в усьому світі. Ми не можемо перешкодити її використанню, або неправомірному використанню в злочинних цілях, сторонніми особами.</li><li>Погоджуючись з цими правилами, ви надаєте згоду на передачу вашої персональної інформації, зазначеної в розділі Е.</li></ol><p><strong>Є. Збереження персональної інформації</strong></p><ol><li>Розділ Є встановлює правила та процедури компанії щодо збереження персональної інформації. Дані правила та процедури призначені для забезпечення виконання нами наших юридичних зобов’язань щодо збереження та видалення персональної інформації.</li><li>Персональна інформація, яку ми обробляємо з певною метою або в певних цілях не повинна зберігатися довше, ніж потрібно для досягнення цієї мети або цих цілей.</li><li>Без обмежень положень, зазначених в пункті Є-2, ми зазвичай видаляємо персональну інформацію, що підпадає у визначені нижче категорії, в дні та час, що визначені нижче:</li><li class=\"ql-indent-1\">персональна інформація буде видалена {ВКАЖІТЬ ДАТУ/ЧАС}; та</li><li class=\"ql-indent-1\">{ВКАЖІТЬ ДОДАТКОВУ ДАТУ/ЧАС}.</li><li>Незважаючи на інші положення Розділу Є, ми будемо зберігати документи (включаючи електронні документи), які містять персональну інформацію:</li><li class=\"ql-indent-1\">в тих випадках, в яких цього від нас вимагає закон;</li><li class=\"ql-indent-1\">якщо ми вважатимемо, що ці документи можуть мати відношення до будь-якого поточного або майбутнього судового розгляду; та</li><li class=\"ql-indent-1\">з метою встановлення, реалізації або захисту наших законних прав (включаючи надання інформації іншим сторонам задля запобігання шахрайству або зниження кредитних ризиків).</li></ol><p><strong>Ж. Захист вашої персональної інформації</strong></p><ol><li>Ми будемо вживати достатні технічні та організаційні заходи для попередження втрати, протиправного використання чи підробки вашої персональної інформації.</li><li>Всю надану вами персональну інформацію ми будемо зберігати на наших захищених (як паролем, так і фаєрволами) серверах.</li><li>Всі електронні фінансові транзакції, здійснені за допомогою нашого сайту, будуть захищені технологією шифрування даних.</li><li>Ви підтверджуєте своє ознайомлення з тим фактом, що передача інформації через Інтернет є по суті є незахищеною, і ми не можемо гарантувати захист даних, надісланих через всесвітню мережу.</li><li>Ви несете повну відповідальність за збереження свого пароля для доступу на наш веб-сайт в таємниці. Ми ніколи не будемо запитувати ваш пароль (за виключенням випадків, коли ви намагаєтесь увійти до свого облікового запису на нашому сайті).</li></ol><p><strong>З. Зміни та поправки</strong></p><p>Ми лишаємо за собою право періодично вносити зміни та поправки в ці правила, та публікувати їх нову редакцію на нашому сайті. Ви повинні періодично перевіряти цю веб-сторінку, щоб пересвідчитись, що розумієте зміст змін, внесених до цих правил. Ми також можемо проінформувати вас про внесення змін до цих правил шляхом надсилання електронної пошти або через систему передачі приватних повідомлень нашого сайту.</p><p><strong>И. Ваші права</strong></p><p>Ви можете надати нам вказівку надавати вам будь-яку персональну інформацію про вас, яку ми маємо; надання такої інформації буде здійснюватись в наступних випадках:</p><ol><li>оплата зборів {ВВЕДІТЬ НАЗВУ ЗБОРУ,ЯКЩО ЗАСТОСОВУЄТЬСЯ}; та</li><li>надання відповідних підтверджень вашої особи ({ВВЕДІТЬ ТЕКСТ ДЛЯ ВІДОБРАЖЕННЯ ВАШИХ ПРАВИЛ, ми зазвичай приймаємо фотокопію вашого паспорта, завірену нотаріусом, та оригінальну копію рахунку на сплату за комунальні послуги для підтвердження вашої поточної адреси}).</li></ol><p>Ми лишаємо за собою відмовити в наданні інформації за вашим запитом, в межах чинного законодавства.</p><p>Ви маєте право надати нам вказівку не обробляти вашу персональну інформацію в маркетингових цілях.</p><p>На практиці, ви, зазвичай, або завчасно погоджуєтесь з тим, щоб ви використовували вашу персональну інформацію в маркетингових цілях, або ми надамо вам можливість відмовитися від використання вашої інформації в маркетингових цілях.</p><p><strong>І. Сторонні веб-сайти</strong></p><p>Наш веб-сайт містить гіперпосилання на, та деталі про веб-сайти сторонніх компаній та осіб. Ми не маємо інструментів керування, та не несемо відповідальності за політику конфіденційності й практики сторонніх осіб та компаній в цій галузі.</p><p><strong>Ї. Оновлення інформації</strong></p><p>Будь-ласка, своєчасно повідомляйте нас, якщо ваша персональна інформація, яка знаходиться у нас, потребує оновлення чи виправлень.</p><p><strong>Й. Файли-Cookies</strong></p><p>Наш веб-сайт використовує файли-cookies. Cookie — це файл, що містить ідентифікатор (стрічку, яка складається з літер та цифр), і який надсилається веб-сервером до веб-браузеру, та зберігається браузером. В подальшому, ідентифікатор надсилається назад на сервер кожного разу, коли браузер запитує веб-сторінку з серверу. Файли-cookies можуть бути або «постійними» або «сеансові»: постійні будуть зберігатися браузером та будуть придатними до завершення терміну дії, якщо тільки не будуть видалені користувачем завчасно; «сеансові» навпаки – будуть видалятися після завершення сеансу роботи з сайтом або після закриття браузеру. Файли-cookies зазвичай не містять жодної інформації, яка ідентифікує особу користувача. Проте, ваша персональна інформація, яку ми маємо, може бути пов’язана з інформацією, що зберігається та отримана від файлів-cookies. {ОБЕРІТЬ ВІРНУ ФРАЗУ На нашому веб-сайті ми використовуємо лише сеансові файли-cookies / лише постійні файли-cookies / як постійні, так і сеансові файли-cookies.}</p><ol><li>Назви файлів-cookies, які ми використовуємо на нашому веб-сайті, та цілі, задля яких вони використовуються, зазначені нижче:</li><li class=\"ql-indent-1\">На нашому веб-сайті ми використовуємо Google Analytics та Adwords для розпізнавання комп’ютера, коли користувач {ЗАЗНАЧТЕ ВСІ ВИПАДКИ ВИКОРИСТАННЯ ФАЙЛІВ-COOKIES НА САЙТІ відвідує веб-сайт / відстеження навігацію користувачів по веб-сайту/ дозволяє використовувати кошик користувача на сайті / вдосконалення зручності користування сайтом / аналіз використання веб-сайту / адміністрування сайту / попередження шахрайства та вдосконалення безпеки сайту / персоналізація сайту для кожного користувача / цільова реклама, яка може бути цікава окремим користувачам / вкажіть мету (цілі)};</li><li>Більшість браузерів надають вам можливість відмовитися від використання файлів-cookies, наприклад:</li><li class=\"ql-indent-1\">в Internet Explorer (версія 10) ви можете заблокувати використовуючи налаштування керування файлами-cookie, доступними в меню «Інструменти» – «Опції Інтернету» – «Конфіденційність» – «Розширені» ( “Tools,” “Internet Options,” “Privacy,” “Advanced”);</li><li class=\"ql-indent-1\">у Firefox (версія 24) ви можете заблокувати всі файли-cookie, натиснувши «Інструменти» – «Опції» – «Конфіденційність»: у випадаючому меню оберіть пункт «Використовувати користувацькі налаштування журналу» та зніміть виділення з пункту «Прийняти файли-cookie від сайтів»; та нарешті</li><li class=\"ql-indent-1\">в Chrome (версія 29) ви можете заблокувати всі файли-cookie увійшовши до меню «Налаштування та управління», та обравши «Налаштування» – «Відобразити розширені налаштування» та «Налаштування контенту», а потім обравши «Заборонити сайтам надсилати будь-які дані» під заголовком «Cookies».</li></ol><p>Блокування всіх файлів-cookiе матиме негативні наслідки на зручність користування багатьма веб-сайтами. Якщо ви заблокуєте файли-cookie, ви не зможете користуватися багатьма функціями нашого веб-сайту.</p><ol><li>Ви можете видалити файли-cookie, які вже зберігаються на вашому комп’ютері, наприклад:</li><li class=\"ql-indent-1\">в Internet Explorer (версія 10), ви маєте видаляти файли-cookie вручну (інструкцію, як це зробити, можна знайти за адресою&nbsp;<a href=\"http://support.microsoft.com/kb/278835\" rel=\"noopener noreferrer\" target=\"_blank\" style=\"color: rgb(90, 50, 174);\">http://support.microsoft.com/kb/278835</a>&nbsp;);</li><li class=\"ql-indent-1\">у Firefox (версія 24), файли-cookie можна видалити перейшовши в меню «Інструменти» – «Опції» – «Конфіденційність»: у випадаючому меню оберіть пункт «Використовувати користувацькі налаштування журналу», натисніть “Показати Cookies,” а потім – “Видалити всі Cookies”;</li><li class=\"ql-indent-1\">в Chrome (версія 29) ви можете видалити всі файли-cookie увійшовши до меню «Налаштування та управління», та обравши «Налаштування» – «Відобразити розширені налаштування» та «Очистити дані перегляду», а перед тим оберіть пункт «Видалити файли-cookie та інші дані й плагіни сайтів».</li><li>Видалення файлів-cookiе матиме негативні наслідки на зручність користування багатьма веб-сайтами.</li></ol><p><em>Website Planet не несе жодної відповідальності і радить вам отримати консультацію у професійного юриста перш, ніж використовувати наведений вище шаблон на вашому веб-сайті.</em></p>",
                    DatePublication = DateTime.Parse("2022-02-04 12:32:03.1720000")
                });

            modelBuilder.Entity<EventType>().HasData(
                new EventType
                {
                    ID = 1,
                    EventTypeName = "Акція"
                },
                new EventType
                {
                    ID = 2,
                    EventTypeName = "Вишкіл"
                },
                new EventType
                {
                    ID = 3,
                    EventTypeName = "Табір"
                }
            );

            modelBuilder.Entity<Precaution>().HasData(
                new Precaution
                {
                    Id = 1,
                    Name = "І пересторога",
                    MonthsPeriod = 3
                },
                new Precaution
                {
                    Id = 2,
                    Name = "ІІ пересторога",
                    MonthsPeriod = 6
                }, new Precaution
                {
                    Id = 3,
                    Name = "ІІІ пересторога",
                    MonthsPeriod = 12
                });

            modelBuilder.Entity<CityDocumentType>().HasData(
                new CityDocumentType
                {
                    ID = 1,
                    Name = "Протокол Загального Збору Станиці"

                },
                new CityDocumentType
                {
                    ID = 2,
                    Name = "Протокол сходин Старшої Пластової Старшини"

                });

            modelBuilder.Entity<ClubDocumentType>().HasData(
                new ClubDocumentType
                {
                    ID = 1,
                    Name = "Протокол Загального Збору Куреня"

                },
                new ClubDocumentType
                {
                    ID = 2,
                    Name = "Протокол сходин Старшої Пластової Старшини"

                });

            modelBuilder.Entity<SectorDocumentType>().HasData(
                new SectorDocumentType
                {
                    Id = 1,
                    Name = "Протокол Загального Збору"

                },
                new SectorDocumentType
                {
                    Id = 2,
                    Name = "Протокол Старшої Пластової Ради"

                });

            modelBuilder.Entity<GoverningBodyDocumentType>().HasData(
                new GoverningBodyDocumentType
                {
                    Id = 1,
                    Name = "Протокол Загального Збору"

                },
                new GoverningBodyDocumentType
                {
                    Id = 2,
                    Name = "Протокол Старшої Пластової Ради"

                });
            modelBuilder.Entity<NotificationType>().HasData(
                new NotificationType
                {
                    Id = 1,
                    Name = "Default"

                },
                new NotificationType
                {
                    Id = 2,
                    Name = "Створення події"

                },
                new NotificationType
                {
                    Id = 3,
                    Name = "Додавання користувача"

                });

            modelBuilder.Entity<EventAdministrationType>().HasData(
                new EventAdministrationType
                {
                    ID = 1,
                    EventAdministrationTypeName = "Комендант"

                },
                new EventAdministrationType
                {
                    ID = 2,
                    EventAdministrationTypeName = "Заступник коменданта"

                },
                new EventAdministrationType
                {
                    ID = 3,
                    EventAdministrationTypeName = "Писар"

                },
                new EventAdministrationType
                {
                    ID = 4,
                    EventAdministrationTypeName = "Бунчужний"

                });

            modelBuilder.Entity<EventSection>().HasData(
                new EventSection
                {
                    ID = 1,
                    EventSectionName = "УПЮ"

                },
                new EventSection
                {
                    ID = 2,
                    EventSectionName = "УПН"

                },
                new EventSection
                {
                    ID = 3,
                    EventSectionName = "УСП/УПС"

                },
                new EventSection
                {
                    ID = 4,
                    EventSectionName = "Інші"
                });

            modelBuilder.Entity<EventStatus>().HasData(
                new EventStatus
                {
                    ID = 1,
                    EventStatusName = "Завершено"

                },
                new EventStatus
                {
                    ID = 2,
                    EventStatusName = "Не затверджено"

                },
                new EventStatus
                {
                    ID = 3,
                    EventStatusName = "Затверджено"

                });

            modelBuilder.Entity<ParticipantStatus>().HasData(
                new ParticipantStatus
                {
                    ID = 1,
                    ParticipantStatusName = "Учасник"

                },
                new ParticipantStatus
                {
                    ID = 2,
                    ParticipantStatusName = "Відмовлено"

                },
                new ParticipantStatus
                {
                    ID = 3,
                    ParticipantStatusName = "Розглядається"

                });

            modelBuilder.Entity<PlastDegree>().HasData(
                new PlastDegree
                {
                    Id = 1,
                    Name = "Старший пластун прихильник / Старша пластунка прихильниця"

                },
                new PlastDegree
                {
                    Id = 2,
                    Name = "Старший пластун / Старша пластунка"

                },
                new PlastDegree
                {
                    Id = 3,
                    Name = "Старший пластун скоб / Cтарша пластунка вірлиця"

                },
                new PlastDegree
                {
                    Id = 4,
                    Name = "Старший пластун гетьманський скоб / Старша пластунка гетьманська вірлиця"

                }, new PlastDegree
                {
                    Id = 5,
                    Name = "Старший пластун скоб гребець / Старша пластунка  вірлиця гребець"

                }, new PlastDegree
                {
                    Id = 6,
                    Name = "Старший пластун скоб обсерватор / Старша пластунка  вірлиця обсерватор"

                }, new PlastDegree
                {
                    Id = 7,
                    Name = "Пластун сеніор прихильник / Пластунка сеніорка прихильниця"

                }, new PlastDegree
                {
                    Id = 8,
                    Name = "Пластун сеніор керівництва / Пластунка сеніорка керівництва"

                }, new PlastDegree
                {
                    Id = 9,
                    Name = "Пластприят"

                });

            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    ID = 1,
                    Name = "Vumonline курс",
                    Link = "https://vumonline.ua/search/?search=%D0%BF%D0%BB%D0%B0%D1%81%D1%82"
                });
        }
    }
}
