using System;
using System.Collections.Generic;

namespace AddressBookApp.Core.Models
{
    public class DashboardAnalytics
    {
        public int TotalContacts { get; set; }
        public int ClientContacts { get; set; }
        public int ImportedContacts { get; set; }
        public int DataExportCount { get; set; }
        public int ApiClientUsageCount { get; set; }
        
        // Chart data
        public ContactGrowthChart ContactGrowth { get; set; } = new();
        public CompanyDistribution CompanyDistribution { get; set; } = new();
        public ActivityTimeline ActivityTimeline { get; set; } = new();
        public ApiUsageChart ApiUsage { get; set; } = new();
        public RecentActivity RecentActivity { get; set; } = new();
    }

    public class ContactGrowthChart
    {
        public List<string> Labels { get; set; } = new(); // Dates
        public List<int> Values { get; set; } = new(); // Contact counts
    }

    public class CompanyDistribution
    {
        public List<string> Companies { get; set; } = new();
        public List<int> Counts { get; set; } = new();
    }

    public class ActivityTimeline
    {
        public List<string> Labels { get; set; } = new(); // Dates
        public List<int> Created { get; set; } = new();
        public List<int> Updated { get; set; } = new();
        public List<int> Viewed { get; set; } = new();
    }

    public class ApiUsageChart
    {
        public List<string> Labels { get; set; } = new(); // Dates
        public List<int> RequestCounts { get; set; } = new();
        public List<string> Endpoints { get; set; } = new();
        public List<int> EndpointCounts { get; set; } = new();
    }

    public class RecentActivity
    {
        public List<ActivityItem> Items { get; set; } = new();
    }

    public class ActivityItem
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}

