using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressBookApp.Application.DTOs;
using AddressBookApp.Core.Interfaces;
using AddressBookApp.Core.Models;
using AddressBookApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AddressBookApp.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IDataExportRepository _dataExportRepository;
        private readonly IApiUsageRepository _apiUsageRepository;
        private readonly IImportHistoryRepository _importHistoryRepository;
        private readonly AddressBookContext _context;
        
        public DashboardService(
            IContactRepository contactRepository,
            IDataExportRepository dataExportRepository,
            IApiUsageRepository apiUsageRepository,
            IImportHistoryRepository importHistoryRepository,
            AddressBookContext context)
        {
            _contactRepository = contactRepository;
            _dataExportRepository = dataExportRepository;
            _apiUsageRepository = apiUsageRepository;
            _importHistoryRepository = importHistoryRepository;
            _context = context;
        }
        
        public async Task<int> GetTotalContactsCountAsync()
        {
            var contacts = await _contactRepository.ListAllAsync();
            return contacts.Count;
        }
        
        public async Task<int> GetClientContactsCountAsync()
        {
            return await _contactRepository.GetClientCountAsync();
        }
        
        public async Task<int> GetImportedContactsCountAsync()
        {
            return await _contactRepository.GetImportedContactsCountAsync();
        }
        
        public async Task<int> GetDataExportCountAsync()
        {
            return await _dataExportRepository.GetExportCountAsync();
        }
        
        public async Task<int> GetApiClientUsageCountLastDaysAsync(int days)
        {
            return await _apiUsageRepository.GetUniqueClientCountInLastDaysAsync(days);
        }

        public async Task<DashboardAnalytics> GetAnalyticsAsync()
        {
            return new DashboardAnalytics
            {
                TotalContacts = await GetTotalContactsCountAsync(),
                ClientContacts = await GetClientContactsCountAsync(),
                ImportedContacts = await GetImportedContactsCountAsync(),
                DataExportCount = await GetDataExportCountAsync(),
                ApiClientUsageCount = await GetApiClientUsageCountLastDaysAsync(7),
                ContactGrowth = await GetContactGrowthAsync(30),
                CompanyDistribution = await GetCompanyDistributionAsync(10),
                ActivityTimeline = await GetActivityTimelineAsync(30),
                ApiUsage = await GetApiUsageChartAsync(7),
                RecentActivity = await GetRecentActivityAsync(10)
            };
        }

        public async Task<ContactGrowthChart> GetContactGrowthAsync(int days = 30)
        {
            var endDate = DateTime.UtcNow.Date;
            var startDate = endDate.AddDays(-days);
            
            var contacts = await _contactRepository.ListAllAsync();
            var result = new ContactGrowthChart();
            
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var count = contacts.Count(c => c.CreatedAt.Date <= date);
                result.Labels.Add(date.ToString("MMM dd"));
                result.Values.Add(count);
            }
            
            return result;
        }

        public async Task<CompanyDistribution> GetCompanyDistributionAsync(int topCount = 10)
        {
            var contacts = await _contactRepository.ListAllAsync();
            var companyGroups = contacts
                .Where(c => !string.IsNullOrWhiteSpace(c.Company))
                .GroupBy(c => c.Company)
                .OrderByDescending(g => g.Count())
                .Take(topCount)
                .ToList();
            
            return new CompanyDistribution
            {
                Companies = companyGroups.Select(g => g.Key!).ToList(),
                Counts = companyGroups.Select(g => g.Count()).ToList()
            };
        }

        public async Task<ActivityTimeline> GetActivityTimelineAsync(int days = 30)
        {
            // This will work once ContactActivity is implemented
            // For now, return empty data
            var endDate = DateTime.UtcNow.Date;
            var startDate = endDate.AddDays(-days);
            
            var result = new ActivityTimeline();
            var contacts = await _contactRepository.ListAllAsync();
            
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                result.Labels.Add(date.ToString("MMM dd"));
                result.Created.Add(contacts.Count(c => c.CreatedAt.Date == date));
                result.Updated.Add(contacts.Count(c => c.UpdatedAt.HasValue && c.UpdatedAt.Value.Date == date));
                result.Viewed.Add(0); // Will be populated when activity tracking is implemented
            }
            
            return result;
        }

        public async Task<ApiUsageChart> GetApiUsageChartAsync(int days = 7)
        {
            var endDate = DateTime.UtcNow.Date;
            var startDate = endDate.AddDays(-days);
            
            var result = new ApiUsageChart();
            
            if (_context.ApiUsages != null)
            {
                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    var nextDate = date.AddDays(1);
                    var count = await _context.ApiUsages
                        .CountAsync(u => u.Timestamp >= date && u.Timestamp < nextDate);
                    result.Labels.Add(date.ToString("MMM dd"));
                    result.RequestCounts.Add(count);
                }

                // Top endpoints
                var topEndpoints = await _context.ApiUsages
                    .Where(u => u.Timestamp >= startDate)
                    .GroupBy(u => u.Endpoint)
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .Select(g => new { Endpoint = g.Key, Count = g.Count() })
                    .ToListAsync();

                result.Endpoints = topEndpoints.Select(e => e.Endpoint ?? "Unknown").ToList();
                result.EndpointCounts = topEndpoints.Select(e => e.Count).ToList();
            }
            else
            {
                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    result.Labels.Add(date.ToString("MMM dd"));
                    result.RequestCounts.Add(0);
                }
            }
            
            return result;
        }

        public async Task<RecentActivity> GetRecentActivityAsync(int count = 10)
        {
            var result = new RecentActivity();
            
            if (_context.ContactActivities != null)
            {
                var activities = await _context.ContactActivities
                    .Include(a => a.Contact)
                    .Include(a => a.User)
                    .OrderByDescending(a => a.Timestamp)
                    .Take(count)
                    .ToListAsync();

                result.Items = activities.Select(a => new ActivityItem
                {
                    Type = a.Type.ToString(),
                    Description = a.Description,
                    ContactName = $"{a.Contact?.FirstName} {a.Contact?.LastName}".Trim(),
                    Timestamp = a.Timestamp,
                    UserName = $"{a.User?.FirstName} {a.User?.LastName}".Trim() ?? "System"
                }).ToList();
            }
            
            return result;
        }
    }
}
