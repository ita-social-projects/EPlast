using EPlast.DataAccess.Repositories.Contracts;
using EPlast.DataAccess.Repositories.Interfaces.Blank;
using EPlast.DataAccess.Repositories.Interfaces.Club;
using EPlast.DataAccess.Repositories.Interfaces.Events;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody;
using EPlast.DataAccess.Repositories.Interfaces.Region;
using EPlast.DataAccess.Repositories.Interfaces;
using EPlast.DataAccess.Repositories.Realizations.Blank;
using EPlast.DataAccess.Repositories.Realizations.Club;
using EPlast.DataAccess.Repositories.Realizations.EducatorsStaff;
using EPlast.DataAccess.Repositories.Realizations.Events;
using EPlast.DataAccess.Repositories.Realizations.Region;
using EPlast.DataAccess.Repositories.Realizations;
using NLog.Extensions.Logging;
using System.Threading.Tasks;
using EPlast.DataAccess.Repositories.Interfaces.GoverningBody.Sector;
using EPlast.DataAccess.Repositories.Realizations.GoverningBody;
using EPlast.DataAccess.Repositories.Realizations.GoverningBody.Sector;

namespace EPlast.DataAccess.Repositories.Realizations.Base
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private EPlastDBContext _dbContext;
        private IUserRepository _user;
        private IUserProfileRepository _userprofile;
        private INationalityRepository _nationality;
        private IDecesionTargetRepository _decesionTarget;
        private IDistinctionRepository _distinction;
        private IPrecautionRepository _precaution;
        private IUserDistinctionRepository _userDistinction;
        private IUserPrecautionRepository _userPrecaution;
        private IDocumentTemplateRepository _documentTemplate;
        private IDecesionRepository _decesion;
        private IMethodicDocumentRepository _methodicDocument;
        private IEventRepository _event;
        private IParticipantStatusRepository _participantStatuses;
        private IGallaryRepository _gallary;
        private IEventGallaryRepository _eventGallary;
        private IParticipantRepository _participant;
        private IEventCategoryRepository _eventCategory;
        private IEventAdminRepository _eventAdmin;
        private IEventTypeRepository _eventType;
        private IEventStatusRepository _eventStatus;
        private IReligionRepository _religion;
        private IGenderRepository _gender;
        private IUpuDegreeRepository _upuDegree;
        private IWorkRepository _work;
        private IEducationRepository _education;
        private IDegreeRepository _degree;
        private IConfirmedUserRepository _confirmedUser;
        private IApproverRepository _approver;

        private IOrganizationRepository _governingBody;
        private IGoverningBodyAdministrationRepository _governingBodyAdministration;
        private IGoverningBodyDocumentsRepository _governingBodyDocuments;
        private IGoverningBodyDocumentTypeRepository _governingBodyDocumentType;

        private ISectorRepository _governingBodySector;
        private ISectorAdministrationRepository _governingBodySectorAdministration;
        private ISectorDocumentsRepository _governingBodySectorDocuments;
        private ISectorDocumentTypeRepository _governingBodySectorDocumentType;

        private ICityAdministrationRepository _cityAdministration;
        private ICityDocumentsRepository _cityDocuments;
        private ICityDocumentTypeRepository _cityDocumentType;
        private ICityMembersRepository _cityMembers;
        private ICityRepository _city;

        private IClubAdministrationRepository _clubAdministration;
        private IAdminTypeRepository _admintype;
        private IClubDocumentsRepository _clubDocuments;
        private IClubDocumentTypeRepository _clubDocumentType;
        private IClubMembersRepository _clubMembers;
        private IClubRepository _club;
        private IClubMemberHistoryRepository _clubMemberHistory;
        private IClubReportMemberRepository _clubReportMember;
        private IClubAnnualReportsRepository _clubAnnualReports;
        private IClubReportAdminsRepository _clubReportAdmins;
        private IClubReportCitiesRepository _clubReportCities;
        private IClubReportPlastDegreesRepository _clubReportPlastDegrees;

        private IRegionRepository _region;
        private IRegionAdministrationRepository _regionAdministration;
        private IRegionFollowersRepository _regionFollowers;
        private IAnnualReportsRepository _annualReports;
        private IMembersStatisticsRepository _membersStatistics;
        private ICityLegalStatusesRepository _cityLegalStatuses;
        private IClubLegalStatusesRepository _clubLegalStatuses;
        private IUserPlastDegreesRepository _userPlastDegrees;
        private IUserNotificationRepository _userNotifications;
        private INotificationTypeRepository _notificationTypes;
        private IUserMembershipDatesRepository _userMembershipDates;
        private IEventAdministrationRepository _eventAdministration;
        private IEventAdministrationTypeRepository _eventAdministrationType;
        private IEventCategoryTypeRepository _eventCategoryTypeRepository;
        private IPlastDegreeRepository _plastDegree;
        private IEducatorsStaffRepository _KVs;
        private IEducatorsStaffTypesRepository _KVtypes;
        private IRegionDocumentRepository _regionDocs;
        private IBlankBiographyDocumentsRepository _biographyDocumentsRepository;
        private IAchievementDocumentsRepository _achievementDocumentsRepository;
        private IExtractFromUPUDocumentsRepository _extractFromUPUDocumentsRepository;
        private IRegionAnnualReportsRepository _regionAnnualReports;

        private SectionRepository _sectionRepository;
        private SubsectionRepository _subsectionRepository;

        public IEducatorsStaffTypesRepository KVTypes
        {
            get
            {
                if (_KVtypes == null)
                {
                    _KVtypes = new EducatorsSatffTypesRepository(_dbContext);
                }
                return _KVtypes;
            }
        }

        public IEducatorsStaffRepository KVs
        {
            get
            {
                if (_KVs == null)
                {
                    _KVs = new EducatorsStaffRepository(_dbContext);
                }
                return _KVs;
            }
        }

        public IDecesionRepository Decesion
        {
            get
            {
                if (_decesion == null)
                {
                    _decesion = new DecesionRepository(_dbContext);
                }
                return _decesion;
            }
        }

        public IMethodicDocumentRepository MethodicDocument
        {
            get
            {
                if (_methodicDocument == null)
                {
                    _methodicDocument = new MethodicDocumentRepository(_dbContext);
                }
                return _methodicDocument;
            }
        }

        public IDocumentTemplateRepository DocumentTemplate
        {
            get
            {
                if (_documentTemplate == null)
                {
                    _documentTemplate = new DocumentTemplateRepository(_dbContext);
                }
                return _documentTemplate;
            }
        }

        public IDecesionTargetRepository DecesionTarget
        {
            get
            {
                if (_decesionTarget == null)
                {
                    _decesionTarget = new DecesionTargetRepository(_dbContext);
                }
                return _decesionTarget;
            }
        }

        public IOrganizationRepository GoverningBody
        {
            get
            {
                if (_governingBody == null)
                {
                    _governingBody = new OrganizationRepository(_dbContext);
                }

                return _governingBody;
            }
        }

        public IGoverningBodyAdministrationRepository GoverningBodyAdministration
        {
            get
            {
                if (_governingBodyAdministration == null)
                {
                    _governingBodyAdministration = new GoverningBodyAdministrationRepository(_dbContext);
                }

                return _governingBodyAdministration;
            }
        }

        public IGoverningBodyDocumentTypeRepository GoverningBodyDocumentType
        {
            get
            {
                if (_governingBodyDocumentType == null)
                {
                    _governingBodyDocumentType = new GoverningBodyDocumentTypeRepository(_dbContext);
                }

                return _governingBodyDocumentType;
            }
        }

        public IGoverningBodyDocumentsRepository GoverningBodyDocuments
        {
            get
            {
                if (_governingBodyDocuments == null)
                {
                    _governingBodyDocuments = new GoverningBodyDocumentsRepository(_dbContext);
                }

                return _governingBodyDocuments;
            }
        }

        public ISectorRepository GoverningBodySector
        {
            get
            {
                if (_governingBodySector == null)
                {
                    _governingBodySector = new SectorRepository(_dbContext);
                }

                return _governingBodySector;
            }
        }

        public ISectorAdministrationRepository GoverningBodySectorAdministration
        {
            get
            {
                if (_governingBodySectorAdministration == null)
                {
                    _governingBodySectorAdministration = new SectorAdministrationRepository(_dbContext);
                }

                return _governingBodySectorAdministration;
            }
        }

        public ISectorDocumentTypeRepository GoverningBodySectorDocumentType
        {
            get
            {
                if (_governingBodySectorDocumentType == null)
                {
                    _governingBodySectorDocumentType = new SectorDocumentTypeRepository(_dbContext);
                }

                return _governingBodySectorDocumentType;
            }
        }

        public ISectorDocumentsRepository GoverningBodySectorDocuments
        {
            get
            {
                if (_governingBodySectorDocuments == null)
                {
                    _governingBodySectorDocuments = new SectorDocumentsRepository(_dbContext);
                }

                return _governingBodySectorDocuments;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_dbContext);
                }

                return _user;
            }
        }

        public IUserProfileRepository UserProfile
        {
            get
            {
                if (_userprofile == null)
                {
                    _userprofile = new UserProfileRepository(_dbContext);
                }
                return _userprofile;
            }
        }

        public INationalityRepository Nationality
        {
            get
            {
                if (_nationality == null)
                {
                    _nationality = new NationalityRepository(_dbContext);
                }

                return _nationality;
            }
        }

        public IEventRepository Event
        {
            get
            {
                if (_event == null)
                {
                    _event = new EventRepository(_dbContext);
                }

                return _event;
            }
        }

        public IEventAdministrationRepository EventAdministration
        {
            get
            {
                if (_eventAdministration == null)
                {
                    _eventAdministration = new EventAdministrationRepository(_dbContext);
                }
                return _eventAdministration;
            }
        }

        public IEventAdministrationTypeRepository EventAdministrationType
        {
            get
            {
                if (_eventAdministrationType == null)
                {
                    _eventAdministrationType = new EventAdministrationTypeRepository(_dbContext);
                }
                return _eventAdministrationType;
            }
        }

        public IGallaryRepository Gallary
        {
            get
            {
                if (_gallary == null)
                {
                    _gallary = new GallaryRepository(_dbContext);
                }
                return _gallary;
            }
        }

        public IEventGallaryRepository EventGallary
        {
            get
            {
                if (_eventGallary == null)
                {
                    _eventGallary = new EventGallaryRepository(_dbContext);
                }
                return _eventGallary;
            }
        }

        public IParticipantStatusRepository ParticipantStatus
        {
            get
            {
                if (_participantStatuses == null)
                {
                    _participantStatuses = new ParticipantStatusRepository(_dbContext);
                }

                return _participantStatuses;
            }
        }

        public IParticipantRepository Participant
        {
            get
            {
                if (_participant == null)
                {
                    _participant = new ParticipantRepository(_dbContext);
                }

                return _participant;
            }
        }

        public IEventCategoryRepository EventCategory
        {
            get
            {
                if (_eventCategory == null)
                {
                    _eventCategory = new EventCategoryRepository(_dbContext);
                }

                return _eventCategory;
            }
        }

        public IEventTypeRepository EventType
        {
            get
            {
                if (_eventType == null)
                {
                    _eventType = new EventTypeRepository(_dbContext);
                }

                return _eventType;
            }
        }

        public IEventCategoryTypeRepository EventCategoryType
        {
            get
            {
                if (_eventCategoryTypeRepository == null)
                {
                    _eventCategoryTypeRepository = new EventCategoryTypeRepository(_dbContext);
                }

                return _eventCategoryTypeRepository;
            }
        }

        public IEventStatusRepository EventStatus
        {
            get
            {
                if (_eventStatus == null)
                {
                    _eventStatus = new EventStatusRepository(_dbContext);
                }

                return _eventStatus;
            }
        }

        public IEventAdminRepository EventAdmin
        {
            get
            {
                if (_eventAdmin == null)
                {
                    _eventAdmin = new EventAdminRepository(_dbContext);
                }

                return _eventAdmin;
            }
        }

        public IReligionRepository Religion
        {
            get
            {
                if (_religion == null)
                {
                    _religion = new ReligionRepository(_dbContext);
                }

                return _religion;
            }
        }

        public IGenderRepository Gender
        {
            get
            {
                if (_gender == null)
                {
                    _gender = new GenderRepository(_dbContext);
                }

                return _gender;
            }
        }

        public IUpuDegreeRepository UpuDegree
        {
            get
            {
                if (_upuDegree == null)
                {
                    _upuDegree = new UpuDegreeRepository(_dbContext);
                }

                return _upuDegree;
            }
        }

        public IWorkRepository Work
        {
            get
            {
                if (_work == null)
                {
                    _work = new WorkRepository(_dbContext);
                }

                return _work;
            }
        }

        public IEducationRepository Education
        {
            get
            {
                if (_education == null)
                {
                    _education = new EducationRepository(_dbContext);
                }

                return _education;
            }
        }

        public IDegreeRepository Degree
        {
            get
            {
                if (_degree == null)
                {
                    _degree = new DegreeRepository(_dbContext);
                }

                return _degree;
            }
        }

        public IConfirmedUserRepository ConfirmedUser
        {
            get
            {
                if (_confirmedUser == null)
                {
                    _confirmedUser = new ConfirmedUserRepository(_dbContext);
                }

                return _confirmedUser;
            }
        }

        public IApproverRepository Approver
        {
            get
            {
                if (_approver == null)
                {
                    _approver = new ApproverRepository(_dbContext);
                }

                return _approver;
            }
        }

        public ICityAdministrationRepository CityAdministration
        {
            get
            {
                if (_cityAdministration == null)
                {
                    _cityAdministration = new CityAdministrationRepository(_dbContext);
                }

                return _cityAdministration;
            }
        }

        public ICityDocumentsRepository CityDocuments
        {
            get
            {
                if (_cityDocuments == null)
                {
                    _cityDocuments = new CityDocumentsRepository(_dbContext);
                }

                return _cityDocuments;
            }
        }

        public IRegionDocumentRepository RegionDocument
        {
            get
            {
                if (_regionDocs == null)
                {
                    _regionDocs = new RegionDocumentRepository(_dbContext);
                }
                return _regionDocs;
            }
        }

        public ICityDocumentTypeRepository CityDocumentType
        {
            get
            {
                if (_cityDocumentType == null)
                {
                    _cityDocumentType = new CityDocumentTypeRepository(_dbContext);
                }

                return _cityDocumentType;
            }
        }

        public ICityMembersRepository CityMembers
        {
            get
            {
                if (_cityMembers == null)
                {
                    _cityMembers = new CityMembersRepository(_dbContext);
                }

                return _cityMembers;
            }
        }

        public ICityRepository City
        {
            get
            {
                if (_city == null)
                {
                    _city = new CityRepository(_dbContext);
                }

                return _city;
            }
        }

        public IAdminTypeRepository AdminType
        {
            get
            {
                if (_admintype == null)
                {
                    _admintype = new AdminTypeRepository(_dbContext);
                }

                return _admintype;
            }
        }

        public IClubRepository Club
        {
            get
            {
                if (_club == null)
                {
                    _club = new ClubRepository(_dbContext);
                }

                return _club;
            }
        }

        public IClubMembersRepository ClubMembers
        {
            get
            {
                if (_clubMembers == null)
                {
                    _clubMembers = new ClubMembersRepository(_dbContext);
                }

                return _clubMembers;
            }
        }

        public IClubDocumentsRepository ClubDocuments
        {
            get
            {
                if (_clubDocuments == null)
                {
                    _clubDocuments = new ClubDocumentsRepository(_dbContext);
                }

                return _clubDocuments;
            }
        }

        public IClubDocumentTypeRepository ClubDocumentType
        {
            get
            {
                if (_clubDocumentType == null)
                {
                    _clubDocumentType = new ClubDocumentTypeRepository(_dbContext);
                }

                return _clubDocumentType;
            }
        }

        public IClubAdministrationRepository ClubAdministration
        {
            get
            {
                if (_clubAdministration == null)
                {
                    _clubAdministration = new ClubAdministrationRepository(_dbContext);
                }

                return _clubAdministration;
            }
        }

        public IClubMemberHistoryRepository ClubMemberHistory
        {
            get
            {
                if (_clubMemberHistory == null)
                {
                    _clubMemberHistory = new ClubMemberHistoryReposetory(_dbContext);
                }

                return _clubMemberHistory;
            }
        }

        public IClubReportAdminsRepository ClubReportAdmins
        {
            get
            {
                if (_clubReportAdmins == null)
                {
                    _clubReportAdmins = new ClubReportAdminsRepository(_dbContext);
                }

                return _clubReportAdmins;
            }
        }

        public IClubReportMemberRepository ClubReportMember
        {
            get
            {
                if (_clubReportMember == null)
                {
                    _clubReportMember = new ClubReportMemberRepository(_dbContext);
                }

                return _clubReportMember;
            }
        }


        public IClubReportCitiesRepository ClubReportCities
        {
            get
            {
                if (_clubReportCities == null)
                {
                    _clubReportCities = new ClubReportCitiesRepository(_dbContext);
                }

                return _clubReportCities;
            }
         
        }
        public IClubReportPlastDegreesRepository ClubReportPlastDegrees
        {
            get
            {
                if (_clubReportPlastDegrees == null)
                {
                    _clubReportPlastDegrees = new ClubReportPlastDegreesRepository(_dbContext);
                }
                return _clubReportPlastDegrees;
            }

        }

        public IRegionRepository Region
        {
            get
            {
                if (_region == null)
                {
                    _region = new RegionRepository(_dbContext);
                }

                return _region;
            }
        }

        public IRegionAdministrationRepository RegionAdministration
        {
            get
            {
                if (_regionAdministration == null)
                {
                    _regionAdministration = new RegionAdministrationRepository(_dbContext);
                }

                return _regionAdministration;
            }
        }

        public IRegionFollowersRepository RegionFollowers
        {
            get
            {
                if (_regionFollowers == null)
                {
                    _regionFollowers = new RegionFollowerRepository(_dbContext);
                }

                return _regionFollowers;
            }
        }

        public IAnnualReportsRepository AnnualReports
        {
            get
            {
                if (_annualReports == null)
                {
                    _annualReports = new AnnualReportsRepository(_dbContext);
                }
                return _annualReports;
            }
        }

        public IClubAnnualReportsRepository ClubAnnualReports
        {
            get
            {
                if (_clubAnnualReports == null)
                {
                    _clubAnnualReports = new ClubAnnualReportRepository(_dbContext);
                }
                return _clubAnnualReports;
            }
        }

        public IRegionAnnualReportsRepository RegionAnnualReports
        {
            get
            {
                if (_regionAnnualReports == null)
                {
                    _regionAnnualReports = new RegionAnnualReportRepository(_dbContext);
                }
                return _regionAnnualReports;
            }
        }


        public ISectionRepository AboutBaseSection
        {
            get
            {
                if(_sectionRepository == null)
                {
                    _sectionRepository = new SectionRepository(_dbContext);
                }
                return _sectionRepository;
            }
        }

        public ISubsectionRepository AboutBaseSubsection
        {
            get
            {
                if(_subsectionRepository == null)
                {
                    _subsectionRepository = new SubsectionRepository(_dbContext);
                }
                return _subsectionRepository;
            }
        }



        public IMembersStatisticsRepository MembersStatistics
        {
            get
            {
                if (_membersStatistics == null)
                {
                    _membersStatistics = new MembersStatisticsRepository(_dbContext);
                }
                return _membersStatistics;
            }
        }

        public ICityLegalStatusesRepository CityLegalStatuses
        {
            get
            {
                if (_cityLegalStatuses == null)
                {
                    _cityLegalStatuses = new CityLegalStatusesRepository(_dbContext);
                }
                return _cityLegalStatuses;
            }
        }

        public IUserMembershipDatesRepository UserMembershipDates
        {
            get
            {
                if (_userMembershipDates == null)
                {
                    _userMembershipDates = new UserMembershipDatesRepository(_dbContext);
                }

                return _userMembershipDates;
            }
        }

        public IClubLegalStatusesRepository ClubLegalStatuses

        {
            get
            {
                if (_clubLegalStatuses == null)
                {
                    _clubLegalStatuses = new ClubLegalStatusesRepository(_dbContext);
                }

                return _clubLegalStatuses;
            }
        }

        public IUserPlastDegreesRepository UserPlastDegrees
        {
            get
            {
                if (_userPlastDegrees == null)
                {
                    _userPlastDegrees = new UserPlastDegreesRepository(_dbContext);
                }
                return _userPlastDegrees;
            }
        }

        public IDistinctionRepository Distinction
        {
            get
            {
                if (_distinction == null)
                {
                    _distinction = new DistinctionRepository(_dbContext);
                }
                return _distinction;
            }
        }

        public IPrecautionRepository Precaution
        {
            get
            {
                if (_precaution == null)
                {
                    _precaution = new PrecautionRepository(_dbContext);
                }
                return _precaution;
            }
        }

        public IUserDistinctionRepository UserDistinction
        {
            get
            {
                if (_userDistinction == null)
                {
                    _userDistinction = new UserDistinctionRepository(_dbContext);
                }
                return _userDistinction;
            }
        }

        public IUserPrecautionRepository UserPrecaution
        {
            get
            {
                if (_userPrecaution == null)
                {
                    _userPrecaution = new UserPrecautionRepository(_dbContext);
                }
                return _userPrecaution;
            }
        }

        public IPlastDegreeRepository PlastDegrees
        {
            get
            {
                if (_plastDegree == null)
                {
                    _plastDegree = new PlastDegreeRepository(_dbContext);
                }
                return _plastDegree;
            }
        }

        public IBlankBiographyDocumentsRepository BiographyDocumentsRepository
        {
            get
            {
                if (_biographyDocumentsRepository == null)
                {
                    _biographyDocumentsRepository = new BlankBiographyDocumentsRepository(_dbContext);
                }
                return _biographyDocumentsRepository;
            }
        }

        public IUserNotificationRepository UserNotifications
        {
            get
            {
                if (_userNotifications == null)
                {
                    _userNotifications = new UserNotificationRepository(_dbContext);
                }
                return _userNotifications;
            }
        }

        public INotificationTypeRepository NotificationTypes
        {
            get
            {
                if (_notificationTypes == null)
                {
                    _notificationTypes = new NotificationTypeRepository(_dbContext);
                }
                return _notificationTypes;
            }
        }

        public IAchievementDocumentsRepository AchievementDocumentsRepository
        {
            get
            {
                if (_achievementDocumentsRepository == null)
                {
                    _achievementDocumentsRepository = new AchievementDocumentsRepository(_dbContext);
                }
                return _achievementDocumentsRepository;
            }
        }

        public IExtractFromUPUDocumentsRepository ExtractFromUPUDocumentsRepository
        {
            get
            {
                if (_extractFromUPUDocumentsRepository == null)
                {
                    _extractFromUPUDocumentsRepository = new ExtractFromUPUDocumentsRepository(_dbContext);
                }
                return _extractFromUPUDocumentsRepository;
            }
        }

        public RepositoryWrapper(EPlastDBContext ePlastDBContext)
        {
            _dbContext = ePlastDBContext;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public string GetCitiesUrl
        {
            get
            {
                return ConfigSettingLayoutRenderer.DefaultConfiguration.GetSection("URLs")["Cities"];
            }
        }

        
    }
}