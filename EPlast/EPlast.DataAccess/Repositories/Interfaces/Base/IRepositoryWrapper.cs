using EPlast.DataAccess.Repositories.Contracts;
using EPlast.DataAccess.Repositories.Interfaces.Blank;
using EPlast.DataAccess.Repositories.Interfaces.Club;
using EPlast.DataAccess.Repositories.Interfaces.Events;
using EPlast.DataAccess.Repositories.Interfaces.Region;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody;

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
        IClubDocumentsRepository ClubDocuments { get; }
        IClubDocumentTypeRepository ClubDocumentType { get; }
        IClubLegalStatusesRepository ClubLegalStatuses { get; }
        IClubMembersRepository ClubMembers { get; }
        IConfirmedUserRepository ConfirmedUser { get; }
        IDecesionRepository Decesion { get; }
        IDecesionTargetRepository DecesionTarget { get; }
        IDegreeRepository Degree { get; }
        IDistinctionRepository Distinction { get; }
        IDocumentTemplateRepository DocumentTemplate { get; }
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
        IExtractFromUPUDocumentsRepository ExtractFromUPUDocumentsRepository { get; }
        IGallaryRepository Gallary { get; }
        IGenderRepository Gender { get; }
        string GetCitiesUrl { get; }
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
        IParticipantRepository Participant { get; }
        IParticipantStatusRepository ParticipantStatus { get; }
        IPlastDegreeRepository PlastDegrees { get; }
        IPrecautionRepository Precaution { get; }
        IRegionRepository Region { get; }
        IRegionAdministrationRepository RegionAdministration { get; }
        IRegionAnnualReportsRepository RegionAnnualReports { get; }
        IRegionDocumentRepository RegionDocument { get; }
        IReligionRepository Religion { get; }
        IUpuDegreeRepository UpuDegree { get; }
        IUserRepository User { get; }
        IUserDistinctionRepository UserDistinction { get; }
        IUserMembershipDatesRepository UserMembershipDates { get; }
        IUserNotificationRepository UserNotifications { get; }
        IUserPlastDegreesRepository UserPlastDegrees { get; }
        IUserPrecautionRepository UserPrecaution { get; }
        IUserProfileRepository UserProfile { get; }
        IWorkRepository Work { get; }
        ISectionRepository AboutBaseSection { get; }
        ISubsectionRepository AboutBaseSubsection { get; }

        void Save();

        Task SaveAsync();
    }
}
