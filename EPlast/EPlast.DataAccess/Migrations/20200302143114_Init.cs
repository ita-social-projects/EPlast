using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER DATABASE EPlast COLLATE SQL_Ukrainian_CP1251_CI_AS", suppressTransaction: true);

            migrationBuilder.CreateTable(
                name: "AdminTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdminTypeName = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AnnualReportStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnualReportStatuses", x => x.ID);
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
                name: "CityDocumentTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityDocumentTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClubName = table.Column<string>(maxLength: 50, nullable: false),
                    ClubURL = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DecesionStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DecesionStatusName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecesionStatuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DecesionTargets",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TargetName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecesionTargets", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Degrees",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DegreeName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degrees", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTemplates",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DocumentName = table.Column<string>(maxLength: 50, nullable: false),
                    DocumentFIleName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplates", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EventCategories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventCategoryName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EventStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventStatusName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStatuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Gallarys",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GenderName = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    NationalityName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationName = table.Column<string>(maxLength: 50, nullable: false)
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserEventStatusName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantStatuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RegionName = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: true)
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReligionName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Religions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlaceOfwork = table.Column<string>(maxLength: 50, nullable: false),
                    Position = table.Column<string>(maxLength: 50, nullable: false)
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                name: "Educations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlaceOfStudy = table.Column<string>(maxLength: 50, nullable: false),
                    Speciality = table.Column<string>(maxLength: 50, nullable: false),
                    DegreeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Educations_Degrees_DegreeID",
                        column: x => x.DegreeID,
                        principalTable: "Degrees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubEventCategories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubEventCategoryName = table.Column<string>(nullable: false),
                    EventCategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubEventCategories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SubEventCategories_EventCategories_EventCategoryID",
                        column: x => x.EventCategoryID,
                        principalTable: "EventCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    EventDateStart = table.Column<DateTime>(nullable: false),
                    EventDateEnd = table.Column<DateTime>(nullable: false),
                    Eventlocation = table.Column<string>(nullable: false),
                    EventCategoryID = table.Column<int>(nullable: false),
                    EventStatusID = table.Column<int>(nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Decesions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    DecesionStatusID = table.Column<int>(nullable: false),
                    OrganizationID = table.Column<int>(nullable: false),
                    DecesionTargetID = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decesions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Decesions_DecesionStatuses_DecesionStatusID",
                        column: x => x.DecesionStatusID,
                        principalTable: "DecesionStatuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
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
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 16, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    CityURL = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    Street = table.Column<string>(maxLength: 60, nullable: false),
                    HouseNumber = table.Column<string>(maxLength: 10, nullable: false),
                    OfficeNumber = table.Column<string>(maxLength: 10, nullable: true),
                    PostIndex = table.Column<string>(maxLength: 7, nullable: true),
                    RegionID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cities_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PhoneNumber = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    EducationID = table.Column<int>(nullable: true),
                    NationalityID = table.Column<int>(nullable: true),
                    ReligionID = table.Column<int>(nullable: true),
                    WorkID = table.Column<int>(nullable: true),
                    GenderID = table.Column<int>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Educations_EducationID",
                        column: x => x.EducationID,
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
                        name: "FK_UserProfiles_Nationalities_NationalityID",
                        column: x => x.NationalityID,
                        principalTable: "Nationalities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Religions_ReligionID",
                        column: x => x.ReligionID,
                        principalTable: "Religions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Works_WorkID",
                        column: x => x.WorkID,
                        principalTable: "Works",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
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
                name: "AnnualReports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    GovernmentFunds = table.Column<int>(nullable: false),
                    ContributionFunds = table.Column<int>(nullable: false),
                    PlastFunds = table.Column<int>(nullable: false),
                    SponsorFunds = table.Column<int>(nullable: false),
                    PropertyList = table.Column<string>(maxLength: 500, nullable: false),
                    ImprovementNeeds = table.Column<string>(maxLength: 500, nullable: false),
                    AnnualReportStatusId = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnualReports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AnnualReports_AnnualReportStatuses_AnnualReportStatusId",
                        column: x => x.AnnualReportStatusId,
                        principalTable: "AnnualReportStatuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnualReports_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubmitDate = table.Column<DateTime>(nullable: true),
                    DocumentURL = table.Column<string>(maxLength: 256, nullable: false),
                    CityDocumentTypeID = table.Column<int>(nullable: true),
                    CityID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityDocuments_CityDocumentTypes_CityDocumentTypeID",
                        column: x => x.CityDocumentTypeID,
                        principalTable: "CityDocumentTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CityDocuments_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    FatherName = table.Column<string>(maxLength: 50, nullable: false),
                    RegistredOn = table.Column<DateTime>(nullable: false),
                    UserProfileID = table.Column<int>(nullable: false),
                    NationalityID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Nationalities_NationalityID",
                        column: x => x.NationalityID,
                        principalTable: "Nationalities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_UserProfiles_UserProfileID",
                        column: x => x.UserProfileID,
                        principalTable: "UserProfiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Approvers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Approvers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Approvers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                name: "CityAdministrations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    CityID = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    AdminTypeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityAdministrations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityAdministrations_AdminTypes_AdminTypeID",
                        column: x => x.AdminTypeID,
                        principalTable: "AdminTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CityAdministrations_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CityAdministrations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CityMembers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    CityID = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityMembers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityMembers_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CityMembers_AspNetUsers_UserId",
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    ClubID = table.Column<int>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubMembers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubMembers_Clubs_ClubID",
                        column: x => x.ClubID,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Participants",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParticipantStatusId = table.Column<int>(nullable: false),
                    EventId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegionAdministrations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdminTypeID = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    RegionID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionAdministrations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RegionAdministrations_AdminTypes_AdminTypeID",
                        column: x => x.AdminTypeID,
                        principalTable: "AdminTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionAdministrations_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegionAdministrations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UnconfirmedCityMember",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    CityID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnconfirmedCityMember", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UnconfirmedCityMember_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnconfirmedCityMember_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConfirmedUsers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ApproverID = table.Column<int>(nullable: true),
                    ConfirmDate = table.Column<DateTime>(nullable: false)
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
                        name: "FK_ConfirmedUsers_AspNetUsers_UserId",
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdminTypeID = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    ClubID = table.Column<int>(nullable: true),
                    ClubMembersID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubAdministrations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubAdministrations_AdminTypes_AdminTypeID",
                        column: x => x.AdminTypeID,
                        principalTable: "AdminTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubAdministrations_Clubs_ClubID",
                        column: x => x.ClubID,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubAdministrations_ClubMembers_ClubMembersID",
                        column: x => x.ClubMembersID,
                        principalTable: "ClubMembers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnualReports_AnnualReportStatusId",
                table: "AnnualReports",
                column: "AnnualReportStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualReports_CityId",
                table: "AnnualReports",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Approvers_UserId",
                table: "Approvers",
                column: "UserId");

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
                name: "IX_AspNetUsers_NationalityID",
                table: "AspNetUsers",
                column: "NationalityID");

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
                name: "IX_AspNetUsers_UserProfileID",
                table: "AspNetUsers",
                column: "UserProfileID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_RegionID",
                table: "Cities",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_CityAdministrations_AdminTypeID",
                table: "CityAdministrations",
                column: "AdminTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CityAdministrations_CityID",
                table: "CityAdministrations",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_CityAdministrations_UserId",
                table: "CityAdministrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CityDocuments_CityDocumentTypeID",
                table: "CityDocuments",
                column: "CityDocumentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CityDocuments_CityID",
                table: "CityDocuments",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_CityMembers_CityID",
                table: "CityMembers",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_CityMembers_UserId",
                table: "CityMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdministrations_AdminTypeID",
                table: "ClubAdministrations",
                column: "AdminTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdministrations_ClubID",
                table: "ClubAdministrations",
                column: "ClubID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdministrations_ClubMembersID",
                table: "ClubAdministrations",
                column: "ClubMembersID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubMembers_ClubID",
                table: "ClubMembers",
                column: "ClubID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubMembers_UserId",
                table: "ClubMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmedUsers_ApproverID",
                table: "ConfirmedUsers",
                column: "ApproverID",
                unique: true,
                filter: "[ApproverID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmedUsers_UserId",
                table: "ConfirmedUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Decesions_DecesionStatusID",
                table: "Decesions",
                column: "DecesionStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Decesions_DecesionTargetID",
                table: "Decesions",
                column: "DecesionTargetID");

            migrationBuilder.CreateIndex(
                name: "IX_Decesions_OrganizationID",
                table: "Decesions",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_DegreeID",
                table: "Educations",
                column: "DegreeID");

            migrationBuilder.CreateIndex(
                name: "IX_EventAdmin_UserID",
                table: "EventAdmin",
                column: "UserID");

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
                name: "IX_RegionAdministrations_AdminTypeID",
                table: "RegionAdministrations",
                column: "AdminTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_RegionAdministrations_RegionID",
                table: "RegionAdministrations",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_RegionAdministrations_UserId",
                table: "RegionAdministrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubEventCategories_EventCategoryID",
                table: "SubEventCategories",
                column: "EventCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_UnconfirmedCityMember_CityID",
                table: "UnconfirmedCityMember",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_UnconfirmedCityMember_UserId",
                table: "UnconfirmedCityMember",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_EducationID",
                table: "UserProfiles",
                column: "EducationID");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_GenderID",
                table: "UserProfiles",
                column: "GenderID");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_NationalityID",
                table: "UserProfiles",
                column: "NationalityID");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ReligionID",
                table: "UserProfiles",
                column: "ReligionID");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_WorkID",
                table: "UserProfiles",
                column: "WorkID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnualReports");

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
                name: "CityAdministrations");

            migrationBuilder.DropTable(
                name: "CityDocuments");

            migrationBuilder.DropTable(
                name: "CityMembers");

            migrationBuilder.DropTable(
                name: "ClubAdministrations");

            migrationBuilder.DropTable(
                name: "ConfirmedUsers");

            migrationBuilder.DropTable(
                name: "Decesions");

            migrationBuilder.DropTable(
                name: "DocumentTemplates");

            migrationBuilder.DropTable(
                name: "EventAdmin");

            migrationBuilder.DropTable(
                name: "EventGallarys");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "RegionAdministrations");

            migrationBuilder.DropTable(
                name: "SubEventCategories");

            migrationBuilder.DropTable(
                name: "UnconfirmedCityMember");

            migrationBuilder.DropTable(
                name: "AnnualReportStatuses");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CityDocumentTypes");

            migrationBuilder.DropTable(
                name: "ClubMembers");

            migrationBuilder.DropTable(
                name: "Approvers");

            migrationBuilder.DropTable(
                name: "DecesionStatuses");

            migrationBuilder.DropTable(
                name: "DecesionTargets");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "Gallarys");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "ParticipantStatuses");

            migrationBuilder.DropTable(
                name: "AdminTypes");

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
                name: "Regions");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Nationalities");

            migrationBuilder.DropTable(
                name: "Religions");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "Degrees");
        }
    }
}
