using System;
using System.Threading.Tasks;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Core.Interfaces
{
    public interface IDashboardService
    {
        Task<int> GetTotalContactsCountAsync();
        Task<int> GetClientContactsCountAsync();
        Task<int> GetImportedContactsCountAsync();
        Task<int> GetDataExportCountAsync();
        Task<int> GetApiClientUsageCountLastDaysAsync(int days);
        Task<DashboardAnalytics> GetAnalyticsAsync();
        Task<ContactGrowthChart> GetContactGrowthAsync(int days = 30);
        Task<CompanyDistribution> GetCompanyDistributionAsync(int topCount = 10);
        Task<ActivityTimeline> GetActivityTimelineAsync(int days = 30);
        Task<ApiUsageChart> GetApiUsageChartAsync(int days = 7);
        Task<RecentActivity> GetRecentActivityAsync(int count = 10);
    }
}
