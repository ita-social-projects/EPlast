using System;
using EPlast.DataAccess.Entities;
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
        public DbSet<EventStatus> EventStatuses { get; set; }
        public DbSet<EventAdministration> EventAdministration { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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
               .HasMany(x => x.UserPlastDegrees)
               .WithOne(x => x.User)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(x => x.RegionAdministrations)
               .WithOne(x => x.User)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(x => x.UnconfirmedCityMembers)
               .WithOne(x => x.User)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(x => x.CityAdministrations)
               .WithOne(x => x.User)
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
        }

        public DbSet<DocumentTemplate> DocumentTemplates { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<DecesionTarget> DecesionTargets { get; set; }
        public DbSet<Decesion> Decesions { get; set; }
        public DbSet<AnnualReport> AnnualReports { get; set; }
        public DbSet<MembersStatistic> MembersStatistics { get; set; }
        public DbSet<CityManagement> CityManagements { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CityAdministration> CityAdministrations { get; set; }
        public DbSet<CityDocuments> CityDocuments { get; set; }
        public DbSet<CityDocumentType> CityDocumentTypes { get; set; }
        public DbSet<CityMembers> CityMembers { get; set; }
        public DbSet<UnconfirmedCityMember> UnconfirmedCityMember { get; set; }
        public DbSet<AdminType> AdminTypes { get; set; }
        public DbSet<ClubMembers> ClubMembers { get; set; }
        public DbSet<ClubAdministration> ClubAdministrations { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<RegionAdministration> RegionAdministrations { get; set; }
        public DbSet<CityLegalStatus> CityLegalStatuses { get; set; }
        public DbSet<UserPlastDegree> UserPlastDegrees { get; set; }
    }
}