using EPlast.BLL.DTO.Statistics;
using EPlast.BLL.Services.Statistics.StatisticsItems.Interfaces;
using EPlast.BLL.Services.Statistics.StatisticsItems.MajorStatisticsItems;
using EPlast.BLL.Services.Statistics.StatisticsItems.MinorStatisticsItems;
using System.Collections.Generic;

namespace EPlast.BLL.Settings
{
    public class StatisticsServiceSettings
    {
        public Dictionary<StatisticsItemIndicator, IStatisticsItem> GetMinorItems()
        {
            return new Dictionary<StatisticsItemIndicator, IStatisticsItem>
            {
                { StatisticsItemIndicator.NumberOfPtashata, new PtashataStatisticsItem() },
                { StatisticsItemIndicator.NumberOfNovatstva, new NovatstvaStatisticsItem() },
                { StatisticsItemIndicator.NumberOfUnatstvaMembers, new UnatstvaMembersStatisticsItem() },
                { StatisticsItemIndicator.NumberOfUnatstvaNoname, new UnatstvaNonameStatisticsItem() },
                { StatisticsItemIndicator.NumberOfUnatstvaProspectors, new UnatstvaProspectorsStatisticsItem() },
                { StatisticsItemIndicator.NumberOfUnatstvaSkobVirlyts, new UnatstvaSkobVirlytsStatisticsItem() },
                { StatisticsItemIndicator.NumberOfUnatstvaSupporters, new UnatstvaSupportersStatisticsItem() },
                { StatisticsItemIndicator.NumberOfSeniorPlastynMembers, new SeniorPlastynMembersStatisticsItem() },
                { StatisticsItemIndicator.NumberOfSeniorPlastynSupporters, new SeniorPlastynSupportersStatisticsItem() },
                { StatisticsItemIndicator.NumberOfSeigneurMembers, new SeigneurMembersStatisticsItem() },
                { StatisticsItemIndicator.NumberOfSeigneurSupporters, new SeigneurSupportersStatisticsItem() }
            };
        }

        public Dictionary<StatisticsItemIndicator, MajorStatisticsItem> GetMajorItems()
        {
            return new Dictionary<StatisticsItemIndicator, MajorStatisticsItem>
            {
                { StatisticsItemIndicator.NumberOfUnatstva, new UnatstvaStatisticsItem() },
                { StatisticsItemIndicator.NumberOfSenior, new SeniorStatisticsItem() },
                { StatisticsItemIndicator.NumberOfSeigneur, new SeigneurStatisticsItem() }
            };
        }
    }
}