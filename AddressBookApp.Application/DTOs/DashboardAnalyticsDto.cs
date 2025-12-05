using System;
using System.Collections.Generic;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Application.DTOs
{
    // DTOs inherit from Core models - can add validation or API-specific properties
    public class DashboardAnalyticsDto : DashboardAnalytics
    {
        // Inherits all properties from DashboardAnalytics
    }

    public class ContactGrowthChartDto : ContactGrowthChart { }
    public class CompanyDistributionDto : CompanyDistribution { }
    public class ActivityTimelineDto : ActivityTimeline { }
    public class ApiUsageChartDto : ApiUsageChart { }
    public class RecentActivityDto : RecentActivity 
    {
        public new List<ActivityItemDto> Items { get; set; } = new();
    }
    public class ActivityItemDto : ActivityItem { }
}

