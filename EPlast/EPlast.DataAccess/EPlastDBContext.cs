using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.AboutBase;
using EPlast.DataAccess.Entities.Blank;
using EPlast.DataAccess.Entities.Decision;
using EPlast.DataAccess.Entities.EducatorsStaff;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Entities.GoverningBody;
using EPlast.DataAccess.Entities.GoverningBody.Announcement;
using EPlast.DataAccess.Entities.GoverningBody.Sector;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.ExtensionMethods;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess
{
    public class EPlastDBContext : IdentityDbContext<User, IdentityRole, string>
    {
        public EPlastDBContext(DbContextOptions<EPlastDBContext> options) : base(options) { }

        public DbSet<AchievementDocuments> AchievementDocuments { get; set; }
        public DbSet<AdminType> AdminTypes { get; set; }
        public DbSet<AnnualReport> AnnualReports { get; set; }
        public DbSet<AnnualReportTableObject> AnnualReportTableObjects { get; set; }
        public DbSet<Approver> Approvers { get; set; }
        public DbSet<BlankBiographyDocuments> BlankBiographyDocuments { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CityAdministration> CityAdministrations { get; set; }
        public DbSet<CityDocuments> CityDocuments { get; set; }
        public DbSet<CityDocumentType> CityDocumentTypes { get; set; }
        public DbSet<CityLegalStatus> CityLegalStatuses { get; set; }
        public DbSet<CityMembers> CityMembers { get; set; }
        public DbSet<CityObject> CityObjects { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubAdministration> ClubAdministrations { get; set; }
        public DbSet<ClubAnnualReport> ClubAnnualReports { get; set; }
        public DbSet<ClubAnnualReportTableObject> ClubAnnualReportTableObjects { get; set; }
        public DbSet<ClubDocuments> ClubDocuments { get; set; }
        public DbSet<ClubDocumentType> ClubDocumentTypes { get; set; }
        public DbSet<ClubLegalStatus> ClubLegalStatuses { get; set; }
        public DbSet<ClubMemberHistory> ClubMemberHistory { get; set; }
        public DbSet<ClubMembers> ClubMembers { get; set; }
        public DbSet<ClubReportAdmins> ClubReportAdmins { get; set; }
        public DbSet<ClubReportCities> ClubReportCities { get; set; }
        public DbSet<ClubReportMember> ClubReportMember { get; set; }
        public DbSet<ClubReportPlastDegrees> ClubReportPlastDegrees { get; set; }
        public DbSet<ConfirmedUser> ConfirmedUsers { get; set; }
        public DbSet<Decesion> Decesions { get; set; }
        public DbSet<DecesionTarget> DecesionTargets { get; set; }
        public DbSet<DecisionTableObject> DecisionTableObject { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Distinction> Distinctions { get; set; }
        public DbSet<DocumentTemplate> DocumentTemplates { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<EducatorsStaff> KVs { get; set; }
        public DbSet<EducatorsStaffTableObject> EducatorsStaffTableObjects { get; set; }
        public DbSet<EducatorsStaffTypes> KVTypes { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventAdministration> EventAdministration { get; set; }
        public DbSet<EventAdministrationType> EventAdministrationType { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventCategoryType> EventCategoryTypes { get; set; }
        public DbSet<EventGallary> EventGallarys { get; set; }
        public DbSet<EventSection> EventSection { get; set; }
        public DbSet<EventStatus> EventStatuses { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<ExtractFromUpuDocuments> ExtractFromUPUDocuments { get; set; }
        public DbSet<Gallary> Gallarys { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<GoverningBodyAdministration> GoverningBodyAdministrations { get; set; }
        public DbSet<GoverningBodyAnnouncement> GoverningBodyAnnouncement { get; set; }
        public DbSet<GoverningBodyAnnouncementImage> GoverningBodyAnnouncementImages { get; set; }
        public DbSet<GoverningBodyDocuments> GoverningBodyDocuments { get; set; }
        public DbSet<GoverningBodyDocumentType> GoverningBodyDocumentTypes { get; set; }
        public DbSet<MembersStatistic> MembersStatistics { get; set; }
        public DbSet<MethodicDocument> MethodicDocuments { get; set; }
        public DbSet<MethodicDocumentTableObject> MethodicDocumentTableObjects { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantStatus> ParticipantStatuses { get; set; }
        public DbSet<Pictures> Pictures { get; set; }
        public DbSet<PlastDegree> PlastDegrees { get; set; }
        public DbSet<Precaution> Precautions { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<RegionAdministration> RegionAdministrations { get; set; }
        public DbSet<RegionAnnualReport> RegionAnnualReports { get; set; }
        public DbSet<RegionAnnualReportTableObject> RegionAnnualReportTableObjects { get; set; }
        public DbSet<RegionDocuments> RegionDocs { get; set; }
        public DbSet<RegionFollowers> RegionFollowers { get; set; }
        public DbSet<RegionMembersInfoTableObject> RegionMembersInfoTableObjects { get; set; }
        public DbSet<RegionNamesObject> RegionNamesObjects { get; set; }
        public DbSet<RegionObject> RegionObjects { get; set; }
        public DbSet<Religion> Religions { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Sector> GoverningBodySectors { get; set; }
        public DbSet<SectorAdministration> GoverningBodySectorAdministrations { get; set; }
        public DbSet<SectorDocuments> GoverningBodySectorDocuments { get; set; }
        public DbSet<SectorDocumentType> GoverningBodySectorDocumentTypes { get; set; }
        public DbSet<Subsection> Subsections { get; set; }
        public DbSet<SubsectionPictures> SubsectionsPictures { get; set; }
        public DbSet<Terms> Terms { get; set; }
        public DbSet<UpuDegree> UpuDegrees { get; set; }
        public DbSet<UserDistinction> UserDistinctions { get; set; }
        public DbSet<UserDistinctionsTableObject> UserDistinctionsTableObject { get; set; }
        public DbSet<UserMembershipDates> UserMembershipDates { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<UserPlastDegree> UserPlastDegrees { get; set; }
        public DbSet<UserPrecaution> UserPrecautions { get; set; }
        public DbSet<UserPrecautionsTableObject> UserPrecautionsTableObject { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserRenewal> UserRenewals { get; set; }
        public DbSet<UserRenewalsTableObject> UserRenewalsTableObjects { get; set; }
      
        public DbSet<Course> Courses { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<UserTableObject> UserTableObjects { get; set; }
        public DbSet<Work> Works { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTableObject>().HasNoKey();
            modelBuilder.Entity<AnnualReportTableObject>().HasNoKey();
            modelBuilder.Entity<ClubAnnualReportTableObject>().HasNoKey();
            modelBuilder.Entity<RegionAnnualReportTableObject>().HasNoKey();
            modelBuilder.Entity<UserDistinctionsTableObject>().HasNoKey();
            modelBuilder.Entity<DecisionTableObject>().HasNoKey();
            modelBuilder.Entity<RegionMembersInfoTableObject>().HasNoKey();
            modelBuilder.Entity<MethodicDocumentTableObject>().HasNoKey();
            modelBuilder.Entity<UserRenewalsTableObject>().HasNoKey();
            modelBuilder.Entity<EducatorsStaffTableObject>().HasNoKey();
            modelBuilder.Entity<UserCourse>()
                .HasKey(bc => bc.ID);
            modelBuilder.Entity<UserCourse>()
                .HasOne(bc => bc.Сourse)
                .WithMany(b => b.UserCourses)
                .HasForeignKey(bc => bc.CourseId);
            modelBuilder.Entity<UserCourse>()
                .HasOne(bc => bc.User)
                .WithMany(c => c.UserCourses)
                .HasForeignKey(bc => bc.UserId);
            modelBuilder.Entity<UserRenewal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRenewals)
                    .HasForeignKey(d => d.UserId)
                    .IsRequired();

                entity.HasOne(d => d.City)
                    .WithMany(p => p.UserRenewals)
                    .HasForeignKey(d => d.CityId)
                    .IsRequired();

                entity.Property(e => e.RequestDate)
                    .IsRequired();

                entity.Property(e => e.Approved)
                    .HasDefaultValue(false);
            });

            builder.Entity<Pictures>()
                .HasKey(x => x.ID);

            builder.Entity<SubsectionPictures>(subsectionPictures =>
            {
                subsectionPictures
                    .HasKey(x => new { x.SubsectionID, x.PictureID });

                subsectionPictures
                    .HasOne(x => x.Subsection)
                    .WithMany(m => m.SubsectionsPictures)
                    .HasForeignKey(x => x.SubsectionID);

                subsectionPictures
                    .HasOne(x => x.Pictures)
                    .WithMany(e => e.Subsections)
                    .HasForeignKey(x => x.PictureID);
            });

            builder.Entity<Subsection>()
                .HasKey(x => x.Id);

            builder.Entity<Event>()
                .HasKey(x => x.ID);

            builder.Entity<Gallary>()
                .HasKey(x => x.ID);

            builder.Entity<EventGallary>(eventGallary =>
            {
                eventGallary
                    .HasKey(x => new { x.EventID, x.GallaryID });

                eventGallary
                    .HasOne(x => x.Event)
                    .WithMany(m => m.EventGallarys)
                    .HasForeignKey(x => x.EventID);

                eventGallary
                    .HasOne(x => x.Gallary)
                    .WithMany(e => e.Events)
                    .HasForeignKey(x => x.GallaryID);
            });

            builder.Entity<Event>()
                .HasKey(x => x.ID);

            builder.Entity<EventAdmin>(eventAdmin =>
            {
                eventAdmin
                    .HasKey(x => new { x.EventID, x.UserID });

                eventAdmin
                    .HasOne(x => x.Event)
                    .WithMany(m => m.EventAdmins)
                    .HasForeignKey(x => x.EventID);

                eventAdmin
                    .HasOne(x => x.User)
                    .WithMany(e => e.Events)
                    .HasForeignKey(x => x.UserID);
            });

            builder.Entity<EventCategoryType>(eventCategoryType =>
            {
                eventCategoryType
                    .HasKey(ct => new { ct.EventTypeId, ct.EventCategoryId });

                eventCategoryType
                    .HasOne(ct => ct.EventType)
                    .WithMany(t => t.EventCategories)
                    .HasForeignKey(ct => ct.EventTypeId);

                eventCategoryType
                    .HasOne(ct => ct.EventCategory)
                    .WithMany(c => c.EventTypes)
                    .HasForeignKey(ct => ct.EventCategoryId);
            });

            builder.Entity<CityDocumentType>()
                .HasMany(x => x.CityDocuments)
                .WithOne(x => x.CityDocumentType)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ClubDocumentType>()
                .HasMany(x => x.ClubDocuments)
                .WithOne(x => x.ClubDocumentType)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>(user =>
            {
                user
                    .HasMany(x => x.Participants)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);

                user
                    .HasMany(x => x.Events)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);

                user
                    .HasMany(x => x.ConfirmedUsers)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);

                user
                    .HasMany(x => x.Approvers)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);

                user
                    .HasMany(x => x.UserDistinctions)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);

                user
                    .HasOne(x => x.UserProfile)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);

                user
                    .HasMany(x => x.CityMembers)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);

                user
                    .HasMany(x => x.ClubMembers)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);

                user
                    .HasOne(x => x.UserPlastDegrees)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);

                user
                    .HasMany(x => x.UserMembershipDates)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);

                user
                    .HasMany(x => x.GoverningBodyAdministrations)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);

                user
                    .HasMany(x => x.RegionAdministrations)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);

                user
                    .HasMany(x => x.CityAdministrations)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);

                user
                    .HasMany(x => x.ClubAdministrations)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<Distinction>()
                .HasMany(x => x.UserDistinctions)
                .WithOne(x => x.Distinction)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AnnualReport>(annualReport =>
            {
                annualReport.HasOne(a => a.Creator)
                    .WithMany(u => u.CreatedAnnualReports)
                    .HasForeignKey(a => a.CreatorId);
                annualReport.HasOne(a => a.NewCityAdmin)
                    .WithMany(u => u.NewCityAdminAnnualReports)
                    .HasForeignKey(a => a.NewCityAdminId);
            });

            builder.Entity<ClubAnnualReport>(annualReport =>
            {
                annualReport.HasOne(a => a.Club);
            });

            builder.Entity<ClubReportAdmins>(reportAdmin =>
            {
                reportAdmin.HasOne(a => a.ClubAnnualReport)
                    .WithMany(r => r.ClubReportAdmins)
                    .HasForeignKey(a => a.ClubAnnualReportId)
                    .OnDelete(DeleteBehavior.Cascade);
                reportAdmin.HasOne(a => a.ClubAdministration)
                    .WithMany(c => c.ClubReportAdmins)
                    .HasForeignKey(a => a.ClubAdministrationId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<ClubReportMember>(reportMember =>
            {
                reportMember.HasOne(m => m.ClubAnnualReport)
                    .WithMany(r => r.ClubReportMembers)
                    .HasForeignKey(m => m.ClubAnnualReportId)
                    .OnDelete(DeleteBehavior.Cascade);
                reportMember.HasOne(a => a.ClubMemberHistory)
                    .WithMany(c => c.ClubReportMembers)
                    .HasForeignKey(a => a.ClubMemberHistoryId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.SeedData();
        }
    }
}
