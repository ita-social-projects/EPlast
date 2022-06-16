using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminTypeName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AnnualReportTableObjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    CityName = table.Column<string>(nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    CanManage = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 25, nullable: true),
                    LastName = table.Column<string>(maxLength: 25, nullable: true),
                    FatherName = table.Column<string>(maxLength: 25, nullable: true),
                    RegistredOn = table.Column<DateTime>(nullable: true),
                    EmailSendedOnRegister = table.Column<DateTime>(nullable: true),
                    EmailSendedOnForgotPassword = table.Column<DateTime>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    SocialNetworking = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CityDocumentTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityDocumentTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CityObjects",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityObjects", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ClubAnnualReportTableObjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ClubId = table.Column<int>(nullable: false),
                    ClubName = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    CanManage = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ClubDocumentTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubDocumentTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 18, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    ClubURL = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    Slogan = table.Column<string>(maxLength: 500, nullable: true),
                    Logo = table.Column<string>(maxLength: 2147483647, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DecesionTargets",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecesionTargets", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DecisionTableObject",
                columns: table => new
                {
                    Total = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DecisionStatusType = table.Column<int>(nullable: false),
                    GoverningBody = table.Column<string>(nullable: true),
                    DecisionTarget = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Degrees",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degrees", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Distinctions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distinctions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTemplates",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentName = table.Column<string>(maxLength: 50, nullable: false),
                    DocumentFIleName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplates", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceOfStudy = table.Column<string>(maxLength: 100, nullable: true),
                    Speciality = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EducatorsStaffTableObjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Subtotal = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    DateOfGranting = table.Column<DateTime>(nullable: false),
                    NumberInRegister = table.Column<int>(nullable: false),
                    KadraVykhovnykivTypeId = table.Column<int>(nullable: false),
                    KadraName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EventAdministrationType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventAdministrationTypeName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAdministrationType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EventSection",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventSectionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSection", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EventStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventStatusName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStatuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventTypeName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Gallarys",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GalaryFileName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gallarys", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GoverningBodyDocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodyDocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoverningBodySectorDocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodySectorDocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KVTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KVTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MethodicDocumentTableObjects",
                columns: table => new
                {
                    Count = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    ID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    GoverningBody = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationName = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 18, nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsMainStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParticipantStatusName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantStatuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PictureFileName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PlastDegrees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlastDegrees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Precautions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Precautions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionAnnualReportTableObjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    RegionId = table.Column<int>(nullable: false),
                    RegionName = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    CanManage = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RegionMembersInfoTableObjects",
                columns: table => new
                {
                    Total = table.Column<int>(nullable: true),
                    CityAnnualReportId = table.Column<int>(nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    CityName = table.Column<string>(nullable: true),
                    ReportStatus = table.Column<int>(nullable: true),
                    NumberOfSeatsPtashat = table.Column<int>(nullable: true),
                    NumberOfIndependentRiy = table.Column<int>(nullable: true),
                    NumberOfClubs = table.Column<int>(nullable: true),
                    NumberOfIndependentGroups = table.Column<int>(nullable: true),
                    NumberOfTeachers = table.Column<int>(nullable: true),
                    NumberOfAdministrators = table.Column<int>(nullable: true),
                    NumberOfTeacherAdministrators = table.Column<int>(nullable: true),
                    NumberOfBeneficiaries = table.Column<int>(nullable: true),
                    NumberOfPlastpryiatMembers = table.Column<int>(nullable: true),
                    NumberOfHonoraryMembers = table.Column<int>(nullable: true),
                    NumberOfPtashata = table.Column<int>(nullable: true),
                    NumberOfNovatstva = table.Column<int>(nullable: true),
                    NumberOfUnatstvaNoname = table.Column<int>(nullable: true),
                    NumberOfUnatstvaSupporters = table.Column<int>(nullable: true),
                    NumberOfUnatstvaMembers = table.Column<int>(nullable: true),
                    NumberOfUnatstvaProspectors = table.Column<int>(nullable: true),
                    NumberOfUnatstvaSkobVirlyts = table.Column<int>(nullable: true),
                    NumberOfSeniorPlastynSupporters = table.Column<int>(nullable: true),
                    NumberOfSeniorPlastynMembers = table.Column<int>(nullable: true),
                    NumberOfSeigneurSupporters = table.Column<int>(nullable: true),
                    NumberOfSeigneurMembers = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RegionNamesObjects",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionNamesObjects", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RegionObjects",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionName = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionObjects", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionName = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    City = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Street = table.Column<string>(nullable: true),
                    HouseNumber = table.Column<string>(nullable: true),
                    OfficeNumber = table.Column<string>(nullable: true),
                    PostIndex = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Religions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Religions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Terms",
                columns: table => new
                {
                    TermsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermsTitle = table.Column<string>(maxLength: 255, nullable: true),
                    TermsText = table.Column<string>(maxLength: 40000, nullable: true),
                    DatePublication = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terms", x => x.TermsId);
                });

            migrationBuilder.CreateTable(
                name: "UpuDegrees",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpuDegrees", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserDistinctionsTableObject",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    DistinctionName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Reporter = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UserRenewalsTableObjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Subtotal = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    CityName = table.Column<string>(nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Approved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UserTableObjects",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    CityName = table.Column<string>(nullable: true),
                    ClubName = table.Column<string>(nullable: true),
                    PlastDegree = table.Column<string>(nullable: true),
                    Roles = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    UPUDegree = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    UserSystemId = table.Column<int>(nullable: false),
                    RegionId = table.Column<int>(nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    ClubId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceOfwork = table.Column<string>(maxLength: 50, nullable: true),
                    Position = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AchievementDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlobName = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AchievementDocuments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Approvers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Approvers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Approvers_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlankBiographyDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlobName = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlankBiographyDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BlankBiographyDocuments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtractFromUPUDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlobName = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtractFromUPUDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExtractFromUPUDocuments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMembershipDates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateEntry = table.Column<DateTime>(nullable: false),
                    DateOath = table.Column<DateTime>(nullable: false),
                    DateEnd = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMembershipDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMembershipDates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClubAdministrations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    ClubId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    AdminTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubAdministrations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubAdministrations_AdminTypes_AdminTypeId",
                        column: x => x.AdminTypeId,
                        principalTable: "AdminTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubAdministrations_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubAdministrations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClubAnnualReports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CurrentClubMembers = table.Column<int>(nullable: false),
                    CurrentClubFollowers = table.Column<int>(nullable: false),
                    ClubEnteredMembersCount = table.Column<int>(nullable: false),
                    ClubLeftMembersCount = table.Column<int>(nullable: false),
                    ClubCenters = table.Column<string>(maxLength: 200, nullable: true),
                    KbUSPWishes = table.Column<string>(maxLength: 500, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 18, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    ClubURL = table.Column<string>(maxLength: 256, nullable: true),
                    Street = table.Column<string>(maxLength: 60, nullable: true),
                    ClubId = table.Column<int>(nullable: false),
                    ClubName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubAnnualReports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubAnnualReports_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClubDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmitDate = table.Column<DateTime>(nullable: true),
                    BlobName = table.Column<string>(maxLength: 64, nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    ClubDocumentTypeId = table.Column<int>(nullable: false),
                    ClubId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubDocuments_ClubDocumentTypes_ClubDocumentTypeId",
                        column: x => x.ClubDocumentTypeId,
                        principalTable: "ClubDocumentTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubDocuments_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClubMemberHistory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    ClubId = table.Column<int>(nullable: false),
                    IsFollower = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubMemberHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubMemberHistory_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubMemberHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClubMembers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsApproved = table.Column<bool>(nullable: false),
                    ClubId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubMembers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubMembers_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDistinctions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistinctionId = table.Column<int>(nullable: false),
                    Reporter = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDistinctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDistinctions_Distinctions_DistinctionId",
                        column: x => x.DistinctionId,
                        principalTable: "Distinctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDistinctions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventCategories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventCategoryName = table.Column<string>(nullable: true),
                    EventSectionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EventCategories_EventSection_EventSectionId",
                        column: x => x.EventSectionId,
                        principalTable: "EventSection",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KVs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    KadraVykhovnykivTypeId = table.Column<int>(nullable: false),
                    DateOfGranting = table.Column<DateTime>(nullable: false),
                    NumberInRegister = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KVs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KVs_KVTypes_KadraVykhovnykivTypeId",
                        column: x => x.KadraVykhovnykivTypeId,
                        principalTable: "KVTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KVs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerUserId = table.Column<string>(nullable: false),
                    NotificationTypeId = table.Column<int>(nullable: false),
                    Checked = table.Column<bool>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CheckedAt = table.Column<DateTime>(nullable: true),
                    SenderLink = table.Column<string>(nullable: true),
                    SenderName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNotifications_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Decesions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 60, nullable: false),
                    DecesionStatusType = table.Column<int>(nullable: false),
                    OrganizationID = table.Column<int>(nullable: false),
                    DecesionTargetID = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decesions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Decesions_DecesionTargets_DecesionTargetID",
                        column: x => x.DecesionTargetID,
                        principalTable: "DecesionTargets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Decesions_Organization_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Decesions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GoverningBodyAdministrations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    GoverningBodyId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    AdminTypeId = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    WorkEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodyAdministrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoverningBodyAdministrations_AdminTypes_AdminTypeId",
                        column: x => x.AdminTypeId,
                        principalTable: "AdminTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoverningBodyAdministrations_Organization_GoverningBodyId",
                        column: x => x.GoverningBodyId,
                        principalTable: "Organization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoverningBodyAdministrations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoverningBodyDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmitDate = table.Column<DateTime>(nullable: true),
                    BlobName = table.Column<string>(maxLength: 64, nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    GoverningBodyDocumentTypeId = table.Column<int>(nullable: false),
                    GoverningBodyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodyDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoverningBodyDocuments_GoverningBodyDocumentTypes_GoverningBodyDocumentTypeId",
                        column: x => x.GoverningBodyDocumentTypeId,
                        principalTable: "GoverningBodyDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoverningBodyDocuments_Organization_GoverningBodyId",
                        column: x => x.GoverningBodyId,
                        principalTable: "Organization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoverningBodySectors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoverningBodyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 12, nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodySectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoverningBodySectors_Organization_GoverningBodyId",
                        column: x => x.GoverningBodyId,
                        principalTable: "Organization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MethodicDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    OrganizationID = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodicDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MethodicDocuments_Organization_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPlastDegrees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlastDegreeId = table.Column<int>(nullable: false),
                    DateStart = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlastDegrees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPlastDegrees_PlastDegrees_PlastDegreeId",
                        column: x => x.PlastDegreeId,
                        principalTable: "PlastDegrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPlastDegrees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPrecautions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrecautionId = table.Column<int>(nullable: false),
                    Reporter = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrecautions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPrecautions_Precautions_PrecautionId",
                        column: x => x.PrecautionId,
                        principalTable: "Precautions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPrecautions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 18, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    CityURL = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    Street = table.Column<string>(maxLength: 60, nullable: false),
                    HouseNumber = table.Column<string>(maxLength: 10, nullable: false),
                    OfficeNumber = table.Column<string>(maxLength: 10, nullable: true),
                    PostIndex = table.Column<string>(maxLength: 7, nullable: true),
                    Logo = table.Column<string>(maxLength: 2147483647, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    RegionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cities_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegionAdministrations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminTypeId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    RegionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionAdministrations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RegionAdministrations_AdminTypes_AdminTypeId",
                        column: x => x.AdminTypeId,
                        principalTable: "AdminTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionAdministrations_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionAdministrations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegionAnnualReports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false),
                    NumberOfSeatsPtashat = table.Column<int>(nullable: false),
                    NumberOfPtashata = table.Column<int>(nullable: false),
                    NumberOfNovatstva = table.Column<int>(nullable: false),
                    NumberOfUnatstvaNoname = table.Column<int>(nullable: false),
                    NumberOfUnatstvaSupporters = table.Column<int>(nullable: false),
                    NumberOfUnatstvaMembers = table.Column<int>(nullable: false),
                    NumberOfUnatstvaProspectors = table.Column<int>(nullable: false),
                    NumberOfUnatstvaSkobVirlyts = table.Column<int>(nullable: false),
                    NumberOfSeniorPlastynSupporters = table.Column<int>(nullable: false),
                    NumberOfSeniorPlastynMembers = table.Column<int>(nullable: false),
                    NumberOfSeigneurSupporters = table.Column<int>(nullable: false),
                    NumberOfSeigneurMembers = table.Column<int>(nullable: false),
                    NumberOfIndependentRiy = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    NumberOfClubs = table.Column<int>(nullable: false),
                    NumberOfIndependentGroups = table.Column<int>(nullable: false),
                    NumberOfTeachers = table.Column<int>(nullable: false),
                    NumberOfAdministrators = table.Column<int>(nullable: false),
                    NumberOfTeacherAdministrators = table.Column<int>(nullable: false),
                    NumberOfBeneficiaries = table.Column<int>(nullable: false),
                    NumberOfPlastpryiatMembers = table.Column<int>(nullable: false),
                    NumberOfHonoraryMembers = table.Column<int>(nullable: false),
                    RegionId = table.Column<int>(nullable: false),
                    RegionName = table.Column<string>(nullable: true),
                    StateOfPreparation = table.Column<string>(nullable: false),
                    Characteristic = table.Column<string>(nullable: false),
                    StatusOfStrategy = table.Column<string>(nullable: false),
                    InvolvementOfVolunteers = table.Column<string>(nullable: false),
                    TrainedNeeds = table.Column<string>(nullable: false),
                    PublicFunding = table.Column<string>(nullable: false),
                    ChurchCooperation = table.Column<string>(nullable: false),
                    Fundraising = table.Column<string>(nullable: false),
                    SocialProjects = table.Column<string>(nullable: false),
                    ProblemSituations = table.Column<string>(nullable: false),
                    ImportantNeeds = table.Column<string>(nullable: false),
                    SuccessStories = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionAnnualReports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RegionAnnualReports_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegionDocs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmitDate = table.Column<DateTime>(nullable: true),
                    BlobName = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    RegionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionDocs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RegionDocs_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegionFollowers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    Appeal = table.Column<string>(maxLength: 1024, nullable: false),
                    CityName = table.Column<string>(maxLength: 50, nullable: false),
                    CityDescription = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    RegionId = table.Column<int>(nullable: false),
                    Street = table.Column<string>(maxLength: 50, nullable: false),
                    HouseNumber = table.Column<string>(maxLength: 10, nullable: false),
                    OfficeNumber = table.Column<string>(maxLength: 10, nullable: false),
                    PostIndex = table.Column<string>(maxLength: 10, nullable: false),
                    СityURL = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionFollowers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RegionFollowers_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionFollowers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subsections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    SectionId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subsections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subsections_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Birthday = table.Column<DateTime>(nullable: true),
                    EducationId = table.Column<int>(nullable: true),
                    DegreeId = table.Column<int>(nullable: true),
                    NationalityId = table.Column<int>(nullable: true),
                    ReligionId = table.Column<int>(nullable: true),
                    WorkId = table.Column<int>(nullable: true),
                    GenderID = table.Column<int>(nullable: true),
                    UpuDegreeID = table.Column<int>(nullable: false),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    Pseudo = table.Column<string>(maxLength: 30, nullable: true),
                    PublicPoliticalActivity = table.Column<string>(maxLength: 50, nullable: true),
                    FacebookLink = table.Column<string>(nullable: true),
                    TwitterLink = table.Column<string>(nullable: true),
                    InstagramLink = table.Column<string>(nullable: true),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Degrees_DegreeId",
                        column: x => x.DegreeId,
                        principalTable: "Degrees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Educations_EducationId",
                        column: x => x.EducationId,
                        principalTable: "Educations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Genders_GenderID",
                        column: x => x.GenderID,
                        principalTable: "Genders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Nationalities_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Nationalities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "Religions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_UpuDegrees_UpuDegreeID",
                        column: x => x.UpuDegreeID,
                        principalTable: "UpuDegrees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConfirmedUsers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(nullable: false),
                    ApproverID = table.Column<int>(nullable: true),
                    ConfirmDate = table.Column<DateTime>(nullable: false),
                    isClubAdmin = table.Column<bool>(nullable: false),
                    isCityAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmedUsers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ConfirmedUsers_Approvers_ApproverID",
                        column: x => x.ApproverID,
                        principalTable: "Approvers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConfirmedUsers_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClubReportAdmins",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubAnnualReportId = table.Column<int>(nullable: false),
                    ClubAdministrationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubReportAdmins", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubReportAdmins_ClubAdministrations_ClubAdministrationId",
                        column: x => x.ClubAdministrationId,
                        principalTable: "ClubAdministrations",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ClubReportAdmins_ClubAnnualReports_ClubAnnualReportId",
                        column: x => x.ClubAnnualReportId,
                        principalTable: "ClubAnnualReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClubReportPlastDegrees",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubAnnualReportId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    PlastDegreeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubReportPlastDegrees", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubReportPlastDegrees_ClubAnnualReports_ClubAnnualReportId",
                        column: x => x.ClubAnnualReportId,
                        principalTable: "ClubAnnualReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubReportPlastDegrees_PlastDegrees_PlastDegreeId",
                        column: x => x.PlastDegreeId,
                        principalTable: "PlastDegrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubReportPlastDegrees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClubReportMember",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubAnnualReportId = table.Column<int>(nullable: false),
                    ClubMemberHistoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubReportMember", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubReportMember_ClubAnnualReports_ClubAnnualReportId",
                        column: x => x.ClubAnnualReportId,
                        principalTable: "ClubAnnualReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubReportMember_ClubMemberHistory_ClubMemberHistoryId",
                        column: x => x.ClubMemberHistoryId,
                        principalTable: "ClubMemberHistory",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "EventCategoryTypes",
                columns: table => new
                {
                    EventTypeId = table.Column<int>(nullable: false),
                    EventCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategoryTypes", x => new { x.EventTypeId, x.EventCategoryId });
                    table.ForeignKey(
                        name: "FK_EventCategoryTypes_EventCategories_EventCategoryId",
                        column: x => x.EventCategoryId,
                        principalTable: "EventCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventCategoryTypes_EventTypes_EventTypeId",
                        column: x => x.EventTypeId,
                        principalTable: "EventTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: false),
                    Questions = table.Column<string>(maxLength: 200, nullable: true),
                    EventDateStart = table.Column<DateTime>(nullable: false),
                    EventDateEnd = table.Column<DateTime>(nullable: false),
                    Eventlocation = table.Column<string>(nullable: false),
                    EventTypeID = table.Column<int>(nullable: false),
                    EventCategoryID = table.Column<int>(nullable: false),
                    EventStatusID = table.Column<int>(nullable: false),
                    FormOfHolding = table.Column<string>(nullable: false),
                    ForWhom = table.Column<string>(maxLength: 50, nullable: false),
                    NumberOfPartisipants = table.Column<int>(maxLength: 6, nullable: false),
                    Rating = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Events_EventCategories_EventCategoryID",
                        column: x => x.EventCategoryID,
                        principalTable: "EventCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_EventStatuses_EventStatusID",
                        column: x => x.EventStatusID,
                        principalTable: "EventStatuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_EventTypes_EventTypeID",
                        column: x => x.EventTypeID,
                        principalTable: "EventTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoverningBodyAnnouncement",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    GoverningBodyId = table.Column<int>(nullable: true),
                    SectorId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    IsPined = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodyAnnouncement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoverningBodyAnnouncement_Organization_GoverningBodyId",
                        column: x => x.GoverningBodyId,
                        principalTable: "Organization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoverningBodyAnnouncement_GoverningBodySectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "GoverningBodySectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoverningBodyAnnouncement_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GoverningBodySectorAdministrations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    SectorId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    AdminTypeId = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    WorkEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodySectorAdministrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoverningBodySectorAdministrations_AdminTypes_AdminTypeId",
                        column: x => x.AdminTypeId,
                        principalTable: "AdminTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoverningBodySectorAdministrations_GoverningBodySectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "GoverningBodySectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoverningBodySectorAdministrations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoverningBodySectorDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmitDate = table.Column<DateTime>(nullable: true),
                    BlobName = table.Column<string>(maxLength: 64, nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    SectorDocumentTypeId = table.Column<int>(nullable: false),
                    SectorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodySectorDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoverningBodySectorDocuments_GoverningBodySectorDocumentTypes_SectorDocumentTypeId",
                        column: x => x.SectorDocumentTypeId,
                        principalTable: "GoverningBodySectorDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoverningBodySectorDocuments_GoverningBodySectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "GoverningBodySectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnnualReports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    NumberOfSeatsPtashat = table.Column<int>(nullable: false),
                    NumberOfIndependentRiy = table.Column<int>(nullable: false),
                    NumberOfClubs = table.Column<int>(nullable: false),
                    NumberOfIndependentGroups = table.Column<int>(nullable: false),
                    NumberOfTeachers = table.Column<int>(nullable: false),
                    NumberOfAdministrators = table.Column<int>(nullable: false),
                    NumberOfTeacherAdministrators = table.Column<int>(nullable: false),
                    NumberOfBeneficiaries = table.Column<int>(nullable: false),
                    NumberOfPlastpryiatMembers = table.Column<int>(nullable: false),
                    NumberOfHonoraryMembers = table.Column<int>(nullable: false),
                    PublicFunds = table.Column<decimal>(nullable: false),
                    ContributionFunds = table.Column<decimal>(nullable: false),
                    PlastSalary = table.Column<decimal>(nullable: false),
                    SponsorshipFunds = table.Column<decimal>(nullable: false),
                    ListProperty = table.Column<string>(maxLength: 2000, nullable: true),
                    ImprovementNeeds = table.Column<string>(maxLength: 2000, nullable: true),
                    CreatorId = table.Column<string>(nullable: true),
                    NewCityAdminId = table.Column<string>(nullable: true),
                    NewCityLegalStatusType = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    ClubID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnualReports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AnnualReports_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnualReports_Clubs_ClubID",
                        column: x => x.ClubID,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnnualReports_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnnualReports_AspNetUsers_NewCityAdminId",
                        column: x => x.NewCityAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CityAdministrations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    AdminTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityAdministrations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityAdministrations_AdminTypes_AdminTypeId",
                        column: x => x.AdminTypeId,
                        principalTable: "AdminTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityAdministrations_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityAdministrations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmitDate = table.Column<DateTime>(nullable: true),
                    BlobName = table.Column<string>(maxLength: 64, nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    CityDocumentTypeId = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityDocuments_CityDocumentTypes_CityDocumentTypeId",
                        column: x => x.CityDocumentTypeId,
                        principalTable: "CityDocumentTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityDocuments_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityLegalStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateStart = table.Column<DateTime>(nullable: false),
                    DateFinish = table.Column<DateTime>(nullable: true),
                    LegalStatusType = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityLegalStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityLegalStatuses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityMembers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsApproved = table.Column<bool>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityMembers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityMembers_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClubLegalStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateStart = table.Column<DateTime>(nullable: false),
                    DateFinish = table.Column<DateTime>(nullable: true),
                    LegalStatusType = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    ClubID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubLegalStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClubLegalStatuses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubLegalStatuses_Clubs_ClubID",
                        column: x => x.ClubID,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClubReportCities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubAnnualReportId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubReportCities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubReportCities_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubReportCities_ClubAnnualReports_ClubAnnualReportId",
                        column: x => x.ClubAnnualReportId,
                        principalTable: "ClubAnnualReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubReportCities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRenewals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    Approved = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRenewals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRenewals_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRenewals_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubsectionsPictures",
                columns: table => new
                {
                    SubsectionID = table.Column<int>(nullable: false),
                    PictureID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubsectionsPictures", x => new { x.SubsectionID, x.PictureID });
                    table.ForeignKey(
                        name: "FK_SubsectionsPictures_Pictures_PictureID",
                        column: x => x.PictureID,
                        principalTable: "Pictures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubsectionsPictures_Subsections_SubsectionID",
                        column: x => x.SubsectionID,
                        principalTable: "Subsections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventAdmin",
                columns: table => new
                {
                    EventID = table.Column<int>(nullable: false),
                    UserID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAdmin", x => new { x.EventID, x.UserID });
                    table.ForeignKey(
                        name: "FK_EventAdmin_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventAdmin_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventAdministration",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventAdministrationTypeID = table.Column<int>(nullable: false),
                    EventID = table.Column<int>(nullable: false),
                    UserID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAdministration", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EventAdministration_EventAdministrationType_EventAdministrationTypeID",
                        column: x => x.EventAdministrationTypeID,
                        principalTable: "EventAdministrationType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventAdministration_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventAdministration_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventGallarys",
                columns: table => new
                {
                    EventID = table.Column<int>(nullable: false),
                    GallaryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventGallarys", x => new { x.EventID, x.GallaryID });
                    table.ForeignKey(
                        name: "FK_EventGallarys_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventGallarys_Gallarys_GallaryID",
                        column: x => x.GallaryID,
                        principalTable: "Gallarys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParticipantStatusId = table.Column<int>(nullable: false),
                    EventId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Estimate = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Participants_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_ParticipantStatuses_ParticipantStatusId",
                        column: x => x.ParticipantStatusId,
                        principalTable: "ParticipantStatuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoverningBodyAnnouncementImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(nullable: false),
                    GoverningBodyAnnouncementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodyAnnouncementImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoverningBodyAnnouncementImages_GoverningBodyAnnouncement_GoverningBodyAnnouncementId",
                        column: x => x.GoverningBodyAnnouncementId,
                        principalTable: "GoverningBodyAnnouncement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MembersStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfPtashata = table.Column<int>(nullable: false),
                    NumberOfNovatstva = table.Column<int>(nullable: false),
                    NumberOfUnatstvaNoname = table.Column<int>(nullable: false),
                    NumberOfUnatstvaSupporters = table.Column<int>(nullable: false),
                    NumberOfUnatstvaMembers = table.Column<int>(nullable: false),
                    NumberOfUnatstvaProspectors = table.Column<int>(nullable: false),
                    NumberOfUnatstvaSkobVirlyts = table.Column<int>(nullable: false),
                    NumberOfSeniorPlastynSupporters = table.Column<int>(nullable: false),
                    NumberOfSeniorPlastynMembers = table.Column<int>(nullable: false),
                    NumberOfSeigneurSupporters = table.Column<int>(nullable: false),
                    NumberOfSeigneurMembers = table.Column<int>(nullable: false),
                    AnnualReportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembersStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembersStatistics_AnnualReports_AnnualReportId",
                        column: x => x.AnnualReportId,
                        principalTable: "AnnualReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AchievementDocuments_UserId",
                table: "AchievementDocuments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualReports_CityId",
                table: "AnnualReports",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualReports_ClubID",
                table: "AnnualReports",
                column: "ClubID");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualReports_CreatorId",
                table: "AnnualReports",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualReports_NewCityAdminId",
                table: "AnnualReports",
                column: "NewCityAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Approvers_UserID",
                table: "Approvers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BlankBiographyDocuments_UserId",
                table: "BlankBiographyDocuments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_RegionId",
                table: "Cities",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_CityAdministrations_AdminTypeId",
                table: "CityAdministrations",
                column: "AdminTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CityAdministrations_CityId",
                table: "CityAdministrations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CityAdministrations_UserId",
                table: "CityAdministrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CityDocuments_CityDocumentTypeId",
                table: "CityDocuments",
                column: "CityDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CityDocuments_CityId",
                table: "CityDocuments",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CityLegalStatuses_CityId",
                table: "CityLegalStatuses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CityMembers_CityId",
                table: "CityMembers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CityMembers_UserId",
                table: "CityMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdministrations_AdminTypeId",
                table: "ClubAdministrations",
                column: "AdminTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdministrations_ClubId",
                table: "ClubAdministrations",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdministrations_UserId",
                table: "ClubAdministrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAnnualReports_ClubId",
                table: "ClubAnnualReports",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubDocuments_ClubDocumentTypeId",
                table: "ClubDocuments",
                column: "ClubDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubDocuments_ClubId",
                table: "ClubDocuments",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubLegalStatuses_CityId",
                table: "ClubLegalStatuses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubLegalStatuses_ClubID",
                table: "ClubLegalStatuses",
                column: "ClubID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubMemberHistory_ClubId",
                table: "ClubMemberHistory",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubMemberHistory_UserId",
                table: "ClubMemberHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubMembers_ClubId",
                table: "ClubMembers",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubMembers_UserId",
                table: "ClubMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportAdmins_ClubAdministrationId",
                table: "ClubReportAdmins",
                column: "ClubAdministrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportAdmins_ClubAnnualReportId",
                table: "ClubReportAdmins",
                column: "ClubAnnualReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportCities_CityId",
                table: "ClubReportCities",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportCities_ClubAnnualReportId",
                table: "ClubReportCities",
                column: "ClubAnnualReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportCities_UserId",
                table: "ClubReportCities",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportMember_ClubAnnualReportId",
                table: "ClubReportMember",
                column: "ClubAnnualReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportMember_ClubMemberHistoryId",
                table: "ClubReportMember",
                column: "ClubMemberHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportPlastDegrees_ClubAnnualReportId",
                table: "ClubReportPlastDegrees",
                column: "ClubAnnualReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportPlastDegrees_PlastDegreeId",
                table: "ClubReportPlastDegrees",
                column: "PlastDegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportPlastDegrees_UserId",
                table: "ClubReportPlastDegrees",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmedUsers_ApproverID",
                table: "ConfirmedUsers",
                column: "ApproverID",
                unique: true,
                filter: "[ApproverID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmedUsers_UserID",
                table: "ConfirmedUsers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Decesions_DecesionTargetID",
                table: "Decesions",
                column: "DecesionTargetID");

            migrationBuilder.CreateIndex(
                name: "IX_Decesions_OrganizationID",
                table: "Decesions",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_Decesions_UserId",
                table: "Decesions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventAdmin_UserID",
                table: "EventAdmin",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_EventAdministration_EventAdministrationTypeID",
                table: "EventAdministration",
                column: "EventAdministrationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_EventAdministration_EventID",
                table: "EventAdministration",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_EventAdministration_UserID",
                table: "EventAdministration",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_EventCategories_EventSectionId",
                table: "EventCategories",
                column: "EventSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_EventCategoryTypes_EventCategoryId",
                table: "EventCategoryTypes",
                column: "EventCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EventGallarys_GallaryID",
                table: "EventGallarys",
                column: "GallaryID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventCategoryID",
                table: "Events",
                column: "EventCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventStatusID",
                table: "Events",
                column: "EventStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventTypeID",
                table: "Events",
                column: "EventTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ExtractFromUPUDocuments_UserId",
                table: "ExtractFromUPUDocuments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyAdministrations_AdminTypeId",
                table: "GoverningBodyAdministrations",
                column: "AdminTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyAdministrations_GoverningBodyId",
                table: "GoverningBodyAdministrations",
                column: "GoverningBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyAdministrations_UserId",
                table: "GoverningBodyAdministrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyAnnouncement_GoverningBodyId",
                table: "GoverningBodyAnnouncement",
                column: "GoverningBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyAnnouncement_SectorId",
                table: "GoverningBodyAnnouncement",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyAnnouncement_UserId",
                table: "GoverningBodyAnnouncement",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyAnnouncementImages_GoverningBodyAnnouncementId",
                table: "GoverningBodyAnnouncementImages",
                column: "GoverningBodyAnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyDocuments_GoverningBodyDocumentTypeId",
                table: "GoverningBodyDocuments",
                column: "GoverningBodyDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyDocuments_GoverningBodyId",
                table: "GoverningBodyDocuments",
                column: "GoverningBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodySectorAdministrations_AdminTypeId",
                table: "GoverningBodySectorAdministrations",
                column: "AdminTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodySectorAdministrations_SectorId",
                table: "GoverningBodySectorAdministrations",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodySectorAdministrations_UserId",
                table: "GoverningBodySectorAdministrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodySectorDocuments_SectorDocumentTypeId",
                table: "GoverningBodySectorDocuments",
                column: "SectorDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodySectorDocuments_SectorId",
                table: "GoverningBodySectorDocuments",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodySectors_GoverningBodyId",
                table: "GoverningBodySectors",
                column: "GoverningBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_KVs_KadraVykhovnykivTypeId",
                table: "KVs",
                column: "KadraVykhovnykivTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_KVs_UserId",
                table: "KVs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MembersStatistics_AnnualReportId",
                table: "MembersStatistics",
                column: "AnnualReportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MethodicDocuments_OrganizationID",
                table: "MethodicDocuments",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_EventId",
                table: "Participants",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_ParticipantStatusId",
                table: "Participants",
                column: "ParticipantStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_UserId",
                table: "Participants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionAdministrations_AdminTypeId",
                table: "RegionAdministrations",
                column: "AdminTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionAdministrations_RegionId",
                table: "RegionAdministrations",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionAdministrations_UserId",
                table: "RegionAdministrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionAnnualReports_RegionId",
                table: "RegionAnnualReports",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionDocs_RegionId",
                table: "RegionDocs",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionFollowers_RegionId",
                table: "RegionFollowers",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionFollowers_UserId",
                table: "RegionFollowers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subsections_SectionId",
                table: "Subsections",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubsectionsPictures_PictureID",
                table: "SubsectionsPictures",
                column: "PictureID");

            migrationBuilder.CreateIndex(
                name: "IX_UserDistinctions_DistinctionId",
                table: "UserDistinctions",
                column: "DistinctionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDistinctions_UserId",
                table: "UserDistinctions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMembershipDates_UserId",
                table: "UserMembershipDates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_NotificationTypeId",
                table: "UserNotifications",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlastDegrees_PlastDegreeId",
                table: "UserPlastDegrees",
                column: "PlastDegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlastDegrees_UserId",
                table: "UserPlastDegrees",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPrecautions_PrecautionId",
                table: "UserPrecautions",
                column: "PrecautionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrecautions_UserId",
                table: "UserPrecautions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_DegreeId",
                table: "UserProfiles",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_EducationId",
                table: "UserProfiles",
                column: "EducationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_GenderID",
                table: "UserProfiles",
                column: "GenderID");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_NationalityId",
                table: "UserProfiles",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ReligionId",
                table: "UserProfiles",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UpuDegreeID",
                table: "UserProfiles",
                column: "UpuDegreeID");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserID",
                table: "UserProfiles",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_WorkId",
                table: "UserProfiles",
                column: "WorkId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRenewals_CityId",
                table: "UserRenewals",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRenewals_UserId",
                table: "UserRenewals",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AchievementDocuments");

            migrationBuilder.DropTable(
                name: "AnnualReportTableObjects");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BlankBiographyDocuments");

            migrationBuilder.DropTable(
                name: "CityAdministrations");

            migrationBuilder.DropTable(
                name: "CityDocuments");

            migrationBuilder.DropTable(
                name: "CityLegalStatuses");

            migrationBuilder.DropTable(
                name: "CityMembers");

            migrationBuilder.DropTable(
                name: "CityObjects");

            migrationBuilder.DropTable(
                name: "ClubAnnualReportTableObjects");

            migrationBuilder.DropTable(
                name: "ClubDocuments");

            migrationBuilder.DropTable(
                name: "ClubLegalStatuses");

            migrationBuilder.DropTable(
                name: "ClubMembers");

            migrationBuilder.DropTable(
                name: "ClubReportAdmins");

            migrationBuilder.DropTable(
                name: "ClubReportCities");

            migrationBuilder.DropTable(
                name: "ClubReportMember");

            migrationBuilder.DropTable(
                name: "ClubReportPlastDegrees");

            migrationBuilder.DropTable(
                name: "ConfirmedUsers");

            migrationBuilder.DropTable(
                name: "Decesions");

            migrationBuilder.DropTable(
                name: "DecisionTableObject");

            migrationBuilder.DropTable(
                name: "DocumentTemplates");

            migrationBuilder.DropTable(
                name: "EducatorsStaffTableObjects");

            migrationBuilder.DropTable(
                name: "EventAdmin");

            migrationBuilder.DropTable(
                name: "EventAdministration");

            migrationBuilder.DropTable(
                name: "EventCategoryTypes");

            migrationBuilder.DropTable(
                name: "EventGallarys");

            migrationBuilder.DropTable(
                name: "ExtractFromUPUDocuments");

            migrationBuilder.DropTable(
                name: "GoverningBodyAdministrations");

            migrationBuilder.DropTable(
                name: "GoverningBodyAnnouncementImages");

            migrationBuilder.DropTable(
                name: "GoverningBodyDocuments");

            migrationBuilder.DropTable(
                name: "GoverningBodySectorAdministrations");

            migrationBuilder.DropTable(
                name: "GoverningBodySectorDocuments");

            migrationBuilder.DropTable(
                name: "KVs");

            migrationBuilder.DropTable(
                name: "MembersStatistics");

            migrationBuilder.DropTable(
                name: "MethodicDocuments");

            migrationBuilder.DropTable(
                name: "MethodicDocumentTableObjects");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "RegionAdministrations");

            migrationBuilder.DropTable(
                name: "RegionAnnualReports");

            migrationBuilder.DropTable(
                name: "RegionAnnualReportTableObjects");

            migrationBuilder.DropTable(
                name: "RegionDocs");

            migrationBuilder.DropTable(
                name: "RegionFollowers");

            migrationBuilder.DropTable(
                name: "RegionMembersInfoTableObjects");

            migrationBuilder.DropTable(
                name: "RegionNamesObjects");

            migrationBuilder.DropTable(
                name: "RegionObjects");

            migrationBuilder.DropTable(
                name: "SubsectionsPictures");

            migrationBuilder.DropTable(
                name: "Terms");

            migrationBuilder.DropTable(
                name: "UserDistinctions");

            migrationBuilder.DropTable(
                name: "UserDistinctionsTableObject");

            migrationBuilder.DropTable(
                name: "UserMembershipDates");

            migrationBuilder.DropTable(
                name: "UserNotifications");

            migrationBuilder.DropTable(
                name: "UserPlastDegrees");

            migrationBuilder.DropTable(
                name: "UserPrecautions");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "UserRenewals");

            migrationBuilder.DropTable(
                name: "UserRenewalsTableObjects");

            migrationBuilder.DropTable(
                name: "UserTableObjects");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CityDocumentTypes");

            migrationBuilder.DropTable(
                name: "ClubDocumentTypes");

            migrationBuilder.DropTable(
                name: "ClubAdministrations");

            migrationBuilder.DropTable(
                name: "ClubMemberHistory");

            migrationBuilder.DropTable(
                name: "ClubAnnualReports");

            migrationBuilder.DropTable(
                name: "Approvers");

            migrationBuilder.DropTable(
                name: "DecesionTargets");

            migrationBuilder.DropTable(
                name: "EventAdministrationType");

            migrationBuilder.DropTable(
                name: "Gallarys");

            migrationBuilder.DropTable(
                name: "GoverningBodyAnnouncement");

            migrationBuilder.DropTable(
                name: "GoverningBodyDocumentTypes");

            migrationBuilder.DropTable(
                name: "GoverningBodySectorDocumentTypes");

            migrationBuilder.DropTable(
                name: "KVTypes");

            migrationBuilder.DropTable(
                name: "AnnualReports");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "ParticipantStatuses");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "Subsections");

            migrationBuilder.DropTable(
                name: "Distinctions");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "PlastDegrees");

            migrationBuilder.DropTable(
                name: "Precautions");

            migrationBuilder.DropTable(
                name: "Degrees");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Nationalities");

            migrationBuilder.DropTable(
                name: "Religions");

            migrationBuilder.DropTable(
                name: "UpuDegrees");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "AdminTypes");

            migrationBuilder.DropTable(
                name: "GoverningBodySectors");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "EventCategories");

            migrationBuilder.DropTable(
                name: "EventStatuses");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "EventSection");
        }
    }
}
