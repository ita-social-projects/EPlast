﻿using EPlast.DataAccess.Repositories.Contracts;
using System.Threading.Tasks;
using EPlast.DataAccess.Repositories.Interfaces.Events;
using EPlast.DataAccess.Repositories.Interfaces.Region;
using EPlast.DataAccess.Repositories.Interfaces.Blank;
using EPlast.DataAccess.Repositories.Interfaces.Club;

namespace EPlast.DataAccess.Repositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IUserProfileRepository UserProfile { get; }
        INationalityRepository Nationality { get; }
        IOrganizationRepository Organization { get; }
        IDecesionTargetRepository DecesionTarget { get; }
        IDocumentTemplateRepository DocumentTemplate { get; }
        IDecesionRepository Decesion { get; }
        IMethodicDocumentRepository MethodicDocument { get; }
        IEventRepository Event { get; }
        IGallaryRepository Gallary { get; }
        IParticipantStatusRepository ParticipantStatus { get; }
        IParticipantRepository Participant { get; }
        IEventCategoryRepository EventCategory { get; }
        IEventGallaryRepository EventGallary { get; }
        IEventAdminRepository EventAdmin { get; }
        IEventTypeRepository EventType { get; }
        IEventStatusRepository EventStatus { get; }
        IEventAdministrationTypeRepository EventAdministrationType { get; }
        IEducationRepository Education { get; }
        IDegreeRepository Degree { get; }
        IReligionRepository Religion { get; }
        IGenderRepository Gender { get; }
        IUpuDegreeRepository UpuDegree { get; }
        IWorkRepository Work { get; }
        IConfirmedUserRepository ConfirmedUser { get; }
        IApproverRepository Approver { get; }

        ICityAdministrationRepository CityAdministration { get; }
        ICityDocumentsRepository CityDocuments { get; }
        IRegionDocumentRepository RegionDocument { get; }
        ICityDocumentTypeRepository CityDocumentType { get; }
        ICityMembersRepository CityMembers { get; }
        ICityRepository City { get; }

        IAdminTypeRepository AdminType { get; }

        IClubAdministrationRepository ClubAdministration { get; }
        IClubDocumentsRepository ClubDocuments { get; }
        IClubDocumentTypeRepository ClubDocumentType { get; }
        IClubMembersRepository ClubMembers { get; }
        IClubRepository Club { get; }

        IRegionRepository Region { get; }
        IRegionAdministrationRepository RegionAdministration { get; }
        IAnnualReportsRepository AnnualReports { get; }
        IClubAnnualReportsRepository ClubAnnualReports { get; }
        IRegionAnnualReportsRepository RegionAnnualReports { get; }

        IMembersStatisticsRepository MembersStatistics { get; }

        ICityLegalStatusesRepository CityLegalStatuses { get; }

        INotificationTypeRepository NotificationTypes { get; }
        IUserNotificationRepository UserNotifications { get; }
        IClubLegalStatusesRepository ClubLegalStatuses { get; }

        IUserPlastDegreesRepository UserPlastDegrees { get; }
        IUserMembershipDatesRepository UserMembershipDates { get; }
        IEventAdministrationRepository EventAdministration { get; }
        IEventCategoryTypeRepository EventCategoryType { get; }
        IDistinctionRepository Distinction { get; }
        IPrecautionRepository Precaution { get; }
        IUserDistinctionRepository UserDistinction { get; }
        IUserPrecautionRepository UserPrecaution { get; }
        IBlankBiographyDocumentsRepository  BiographyDocumentsRepository { get; }
        IAchievementDocumentsRepository AchievementDocumentsRepository { get; }
        IExtractFromUPUDocumentsRepository ExtractFromUPUDocumentsRepository { get; }
        IPlastDegreeRepository PlastDegrees { get; }
        IEducatorsStaffRepository KVs { get; }
        IEducatorsStaffTypesRepository KVTypes { get; }
        void Save();
        Task SaveAsync();
    }
}