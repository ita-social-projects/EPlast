using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Entities.Decision;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Entities.GoverningBody.Announcement;
using EPlast.DataAccess.Entities.GoverningBody.Sector;
using EPlast.DataAccess.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess
{
    public class EPlastDBContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public EPlastDBContext(DbContextOptions<EPlastDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Religion> Religions { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<UpuDegree> UpuDegrees { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<ConfirmedUser> ConfirmedUsers { get; set; }
        public DbSet<Approver> Approvers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Gallary> Gallarys { get; set; }
        public DbSet<EventGallary> EventGallarys { get; set; }
        public DbSet<ParticipantStatus> ParticipantStatuses { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<EventCategoryType> EventCategoryTypes { get; set; }
        public DbSet<EventStatus> EventStatuses { get; set; }
        public DbSet<EventAdministration> EventAdministration { get; set; }
        public DbSet<EventAdministrationType> EventAdministrationType { get; set; }
        public DbSet<UserTableObject> UserTableObjects { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Subsection> Subsections { get; set; }
        public DbSet<AnnualReportTableObject> AnnualReportTableObjects { get; set; }
        public DbSet<ClubAnnualReportTableObject> ClubAnnualReportTableObjects { get; set; }
        public DbSet<RegionAnnualReportTableObject> RegionAnnualReportTableObjects { get; set; }
        public DbSet<UserDistinctionsTableObject> UserDistinctionsTableObject { get; set; }
        public DbSet<MethodicDocumentTableObject> MethodicDocumentTableObjects { get; set; }
        public DbSet<UserPrecautionsTableObject> UserPrecautionsTableObject { get; set; }
        public DbSet<DecisionTableObject> DecisionTableObject { get; set; }
        public DbSet<RegionMembersInfoTableObject> RegionMembersInfoTableObjects { get; set; }
        public DbSet<GoverningBodyAnnouncement> GoverningBodyAnnouncement { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTableObject>().HasNoKey();
            modelBuilder.Entity<AnnualReportTableObject>().HasNoKey();
            modelBuilder.Entity<ClubAnnualReportTableObject>().HasNoKey();
            modelBuilder.Entity<RegionAnnualReportTableObject>().HasNoKey();
            modelBuilder.Entity<UserDistinctionsTableObject>().HasNoKey();
            modelBuilder.Entity<UserPrecautionsTableObject>().HasNoKey();
            modelBuilder.Entity<DecisionTableObject>().HasNoKey();
            modelBuilder.Entity<RegionMembersInfoTableObject>().HasNoKey();
            modelBuilder.Entity<MethodicDocumentTableObject>().HasNoKey();


            modelBuilder.Entity<Event>()
                .HasKey(x => x.ID);

            modelBuilder.Entity<Gallary>()
                .HasKey(x => x.ID);

            modelBuilder.Entity<EventGallary>()
                .HasKey(x => new { x.EventID, x.GallaryID });

            modelBuilder.Entity<EventGallary>()
                .HasOne(x => x.Event)
                .WithMany(m => m.EventGallarys)
                .HasForeignKey(x => x.EventID);

            modelBuilder.Entity<EventGallary>()
                .HasOne(x => x.Gallary)
                .WithMany(e => e.Events)
                .HasForeignKey(x => x.GallaryID);

            modelBuilder.Entity<Event>()
                .HasKey(x => x.ID);

            modelBuilder.Entity<EventAdmin>()
                .HasKey(x => new { x.EventID, x.UserID });

            modelBuilder.Entity<EventAdmin>()
                .HasOne(x => x.Event)
                .WithMany(m => m.EventAdmins)
                .HasForeignKey(x => x.EventID);

            modelBuilder.Entity<EventAdmin>()
                .HasOne(x => x.User)
                .WithMany(e => e.Events)
                .HasForeignKey(x => x.UserID);

            modelBuilder.Entity<EventCategoryType>()
                .HasKey(ct => new { ct.EventTypeId, ct.EventCategoryId });

            modelBuilder.Entity<EventCategoryType>()
                .HasOne(ct => ct.EventType)
                .WithMany(t => t.EventCategories)
                .HasForeignKey(ct => ct.EventTypeId);

            modelBuilder.Entity<EventCategoryType>()
                .HasOne(ct => ct.EventCategory)
                .WithMany(c => c.EventTypes)
                .HasForeignKey(ct => ct.EventCategoryId);

            modelBuilder.Entity<User>()
                .HasOne(x => x.UserProfile)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(x => x.CityMembers)
               .WithOne(x => x.User)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(x => x.ClubMembers)
               .WithOne(x => x.User)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasOne(x => x.UserPlastDegrees)
               .WithOne(x => x.User)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(x => x.UserMembershipDates)
               .WithOne(x => x.User)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(x => x.GoverningBodyAdministrations)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(x => x.RegionAdministrations)
               .WithOne(x => x.User)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(x => x.CityAdministrations)
               .WithOne(x => x.User)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(x => x.ClubAdministrations)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CityDocumentType>()
                .HasMany(x => x.CityDocuments)
                .WithOne(x => x.CityDocumentType)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClubDocumentType>()
                .HasMany(x => x.ClubDocuments)
                .WithOne(x => x.ClubDocumentType)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(x => x.Participants)
               .WithOne(x => x.User)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(x => x.Events)
               .WithOne(x => x.User)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(x => x.ConfirmedUsers)
               .WithOne(x => x.User)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(x => x.Approvers)
               .WithOne(x => x.User)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(x => x.UserDistinctions)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Distinction>()
                .HasMany(x => x.UserDistinctions)
                .WithOne(x => x.Distinction)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AnnualReport>(annualReport =>
            {
                annualReport.HasOne(a => a.Creator)
                    .WithMany(u => u.CreatedAnnualReports)
                    .HasForeignKey(a => a.CreatorId);
                annualReport.HasOne(a => a.NewCityAdmin)
                    .WithMany(u => u.NewCityAdminAnnualReports)
                    .HasForeignKey(a => a.NewCityAdminId);
            });

            modelBuilder.Entity<ClubAnnualReport>(annualReport =>
            {
                annualReport.HasOne(a => a.Club);
            });
        }
        public DbSet<RegionAnnualReport> RegionAnnualReports { get; set; }
        public DbSet<DocumentTemplate> DocumentTemplates { get; set; }
        public DbSet<DecesionTarget> DecesionTargets { get; set; }
        public DbSet<Decesion> Decesions { get; set; }
        public DbSet<MethodicDocument> MethodicDocuments { get; set; }
        public DbSet<AnnualReport> AnnualReports { get; set; }
        public DbSet<ClubAnnualReport> ClubAnnualReports { get; set; }

        public DbSet<MembersStatistic> MembersStatistics { get; set; }

        public DbSet<Organization> Organization { get; set; }
        public DbSet<GoverningBodyAdministration> GoverningBodyAdministrations { get; set; }
        public DbSet<GoverningBodyDocuments> GoverningBodyDocuments { get; set; }
        public DbSet<GoverningBodyDocumentType> GoverningBodyDocumentTypes { get; set; }

        public DbSet<Sector> GoverningBodySectors { get; set; }
        public DbSet<SectorAdministration> GoverningBodySectorAdministrations { get; set; }
        public DbSet<SectorDocuments> GoverningBodySectorDocuments { get; set; }
        public DbSet<SectorDocumentType> GoverningBodySectorDocumentTypes { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<CityAdministration> CityAdministrations { get; set; }
        public DbSet<CityDocuments> CityDocuments { get; set; }
        public DbSet<CityDocumentType> CityDocumentTypes { get; set; }
        public DbSet<CityMembers> CityMembers { get; set; }

        public DbSet<AdminType> AdminTypes { get; set; }

        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubAdministration> ClubAdministrations { get; set; }
        public DbSet<ClubDocuments> ClubDocuments { get; set; }
        public DbSet<ClubDocumentType> ClubDocumentTypes { get; set; }
        public DbSet<ClubMembers> ClubMembers { get; set; }

        public DbSet<Region> Regions { get; set; }
        public DbSet<RegionDocuments> RegionDocs { get; set; }
        public DbSet<RegionAdministration> RegionAdministrations { get; set; }
        public DbSet<RegionFollowers> RegionFollowers { get; set; }
        public DbSet<CityLegalStatus> CityLegalStatuses { get; set; }
        public DbSet<ClubLegalStatus> ClubLegalStatuses { get; set; }
        public DbSet<UserPlastDegree> UserPlastDegrees { get; set; }
        public DbSet<UserMembershipDates> UserMembershipDates { get; set; }
        public DbSet<PlastDegree> PlastDegrees { get; set; }
        public DbSet<EducatorsStaff> KVs { get; set; }
        public DbSet<EducatorsStaffTypes> KVTypes { get; set; }
        public DbSet<Precaution> Precautions { get; set; }
        public DbSet<Distinction> Distinctions { get; set; }
        public DbSet<UserPrecaution> UserPrecautions { get; set; }
        public DbSet<UserDistinction> UserDistinctions { get; set; }
        public DbSet<BlankBiographyDocuments> BlankBiographyDocuments { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<AchievementDocuments> AchievementDocuments { get; set; }
        public DbSet<ExtractFromUPUDocuments> ExtractFromUPUDocuments { get; set; }
        public DbSet<EventSection> EventSection { get; set; }
        public DbSet<ClubReportPlastDegrees> ClubReportPlastDegrees { get; set; }
        public DbSet<ClubReportMember> ClubReportMember { get; set; }
        public DbSet<ClubReportAdmins> ClubReportAdmins { get; set; }
        public DbSet<ClubMemberHistory> ClubMemberHistory { get; set; }
    }
}
