using System.Threading.Tasks;
using EPlast.DataAccess.Repositories.Contracts;
using EPlast.DataAccess.Repositories.Interfaces.Blank;
using EPlast.DataAccess.Repositories.Interfaces.Club;
using EPlast.DataAccess.Repositories.Interfaces.Events;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody.Announcement;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody.Sector;
using EPlast.DataAccess.Repositories.Interfaces.Region;
using EPlast.DataAccess.Repositories.Interfaces.User;

namespace EPlast.DataAccess.Repositories
{
    public interface IRepositoryWrapper
    {
        IAchievementDocumentsRepository AchievementDocumentsRepository { get; }
        IAdminTypeRepository AdminType { get; }
        IAnnualReportsRepository AnnualReports { get; }
        IApproverRepository Approver { get; }
        IBlankBiographyDocumentsRepository BiographyDocumentsRepository { get; }
        ICityRepository City { get; }
        ICityAdministrationRepository CityAdministration { get; }
        ICityDocumentsRepository CityDocuments { get; }
        ICityDocumentTypeRepository CityDocumentType { get; }
        ICityLegalStatusesRepository CityLegalStatuses { get; }
        ICityMembersRepository CityMembers { get; }
        IClubRepository Club { get; }
        IClubAdministrationRepository ClubAdministration { get; }
        IClubAnnualReportsRepository ClubAnnualReports { get; }
        IClubReportMemberRepository ClubReportMember { get; }
        IClubReportCitiesRepository ClubReportCities { get; }
        IClubReportPlastDegreesRepository ClubReportPlastDegrees { get; }
        IClubDocumentsRepository ClubDocuments { get; }
        IClubDocumentTypeRepository ClubDocumentType { get; }
        IClubLegalStatusesRepository ClubLegalStatuses { get; }
        IClubMembersRepository ClubMembers { get; }
        IClubMemberHistoryRepository ClubMemberHistory { get; }
        IClubReportAdminsRepository ClubReportAdmins { get; }
        IConfirmedUserRepository ConfirmedUser { get; }
        IDecesionRepository Decesion { get; }
        IDecesionTargetRepository DecesionTarget { get; }
        IDegreeRepository Degree { get; }
        IDistinctionRepository Distinction { get; }
        IDocumentTemplateRepository DocumentTemplate { get; }
        IGoverningBodyAnnouncementRepository GoverningBodyAnnouncement { get; }
        IGoverningBodyAnnouncementImageRepository GoverningBodyAnnouncementImage { get; }
        IEducationRepository Education { get; }
        IEventRepository Event { get; }
        IEventAdminRepository EventAdmin { get; }
        IEventAdministrationRepository EventAdministration { get; }
        IEventAdministrationTypeRepository EventAdministrationType { get; }
        IEventCategoryRepository EventCategory { get; }
        IEventCategoryTypeRepository EventCategoryType { get; }
        IEventGallaryRepository EventGallary { get; }
        IEventStatusRepository EventStatus { get; }
        IEventTypeRepository EventType { get; }
        IEventSectionRepository EventSection { get; }
        IExtractFromUpuDocumentsRepository ExtractFromUPUDocumentsRepository { get; }
        IGallaryRepository Gallary { get; }
        IGenderRepository Gender { get; }
        string GetCitiesUrl { get; }
        string GetUserPageUrl { get; }
        IEducatorsStaffRepository KVs { get; }
        IEducatorsStaffTypesRepository KVTypes { get; }
        IMembersStatisticsRepository MembersStatistics { get; }
        IMethodicDocumentRepository MethodicDocument { get; }
        INationalityRepository Nationality { get; }
        INotificationTypeRepository NotificationTypes { get; }
        IOrganizationRepository GoverningBody { get; }
        IGoverningBodyAdministrationRepository GoverningBodyAdministration { get; }
        IGoverningBodyDocumentsRepository GoverningBodyDocuments { get; }
        IGoverningBodyDocumentTypeRepository GoverningBodyDocumentType { get; }
        ISectorRepository GoverningBodySector { get; }
        ISectorAdministrationRepository GoverningBodySectorAdministration { get; }
        ISectorDocumentsRepository GoverningBodySectorDocuments { get; }
        ISectorDocumentTypeRepository GoverningBodySectorDocumentType { get; }
        ISubsectionRepository Subsection { get; }
        ISubsectionPicturesRepository SubsectionPictures { get; }
        IParticipantRepository Participant { get; }
        IParticipantStatusRepository ParticipantStatus { get; }
        IPicturesRepository Pictures { get;  }
        IPlastDegreeRepository PlastDegrees { get; }
        IPrecautionRepository Precaution { get; }
        IRegionRepository Region { get; }
        IRegionAdministrationRepository RegionAdministration { get; }
        IRegionFollowersRepository RegionFollowers { get; }
        IRegionAnnualReportsRepository RegionAnnualReports { get; }
        IRegionDocumentRepository RegionDocument { get; }
        IReligionRepository Religion { get; }
        IUpuDegreeRepository UpuDegree { get; }
        IUserRepository User { get; }
        IUserDistinctionRepository UserDistinction { get; }
        IUserMembershipDatesRepository UserMembershipDates { get; }
        IUserNotificationRepository UserNotifications { get; }
        IUserPlastDegreeRepository UserPlastDegree { get; }
        IUserPrecautionRepository UserPrecaution { get; }
        IUserProfileRepository UserProfile { get; }
        IWorkRepository Work { get; }
        ISectionRepository AboutBaseSection { get; }
        ISubsectionRepository AboutBaseSubsection { get; }
        ITermsRepository TermsOfUse { get; }
        IUserRenewalRepository UserRenewal { get; }
        ICourseRepository Course { get; }

        
        void Save();

        Task SaveAsync();
    }
}
