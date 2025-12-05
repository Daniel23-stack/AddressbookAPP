using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AddressBookApp.Application.DTOs;
using AddressBookApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookApp.API.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        
        public async Task<IActionResult> Index()
        {
            var analytics = await _dashboardService.GetAnalyticsAsync();
            ViewBag.UserName = User.FindFirst(ClaimTypes.GivenName)?.Value;
            return View(analytics);
        }

        [HttpGet("api/dashboard/analytics")]
        [ProducesResponseType(typeof(DashboardAnalyticsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<DashboardAnalyticsDto>> GetAnalytics()
        {
            var analytics = await _dashboardService.GetAnalyticsAsync();
            return Ok(new DashboardAnalyticsDto
            {
                TotalContacts = analytics.TotalContacts,
                ClientContacts = analytics.ClientContacts,
                ImportedContacts = analytics.ImportedContacts,
                DataExportCount = analytics.DataExportCount,
                ApiClientUsageCount = analytics.ApiClientUsageCount,
                ContactGrowth = new ContactGrowthChartDto
                {
                    Labels = analytics.ContactGrowth.Labels,
                    Values = analytics.ContactGrowth.Values
                },
                CompanyDistribution = new CompanyDistributionDto
                {
                    Companies = analytics.CompanyDistribution.Companies,
                    Counts = analytics.CompanyDistribution.Counts
                },
                ActivityTimeline = new ActivityTimelineDto
                {
                    Labels = analytics.ActivityTimeline.Labels,
                    Created = analytics.ActivityTimeline.Created,
                    Updated = analytics.ActivityTimeline.Updated,
                    Viewed = analytics.ActivityTimeline.Viewed
                },
                ApiUsage = new ApiUsageChartDto
                {
                    Labels = analytics.ApiUsage.Labels,
                    RequestCounts = analytics.ApiUsage.RequestCounts,
                    Endpoints = analytics.ApiUsage.Endpoints,
                    EndpointCounts = analytics.ApiUsage.EndpointCounts
                },
                RecentActivity = new RecentActivityDto
                {
                    Items = analytics.RecentActivity.Items.Select(i => new ActivityItemDto
                    {
                        Type = i.Type,
                        Description = i.Description,
                        ContactName = i.ContactName,
                        Timestamp = i.Timestamp,
                        UserName = i.UserName
                    }).ToList()
                }
            });
        }

        [HttpGet("api/dashboard/contact-growth")]
        [ProducesResponseType(typeof(ContactGrowthChartDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ContactGrowthChartDto>> GetContactGrowth([FromQuery] int days = 30)
        {
            var data = await _dashboardService.GetContactGrowthAsync(days);
            return Ok(new ContactGrowthChartDto { Labels = data.Labels, Values = data.Values });
        }

        [HttpGet("api/dashboard/company-distribution")]
        [ProducesResponseType(typeof(CompanyDistributionDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CompanyDistributionDto>> GetCompanyDistribution([FromQuery] int topCount = 10)
        {
            var data = await _dashboardService.GetCompanyDistributionAsync(topCount);
            return Ok(new CompanyDistributionDto { Companies = data.Companies, Counts = data.Counts });
        }

        [HttpGet("api/dashboard/activity-timeline")]
        [ProducesResponseType(typeof(ActivityTimelineDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ActivityTimelineDto>> GetActivityTimeline([FromQuery] int days = 30)
        {
            var data = await _dashboardService.GetActivityTimelineAsync(days);
            return Ok(new ActivityTimelineDto
            {
                Labels = data.Labels,
                Created = data.Created,
                Updated = data.Updated,
                Viewed = data.Viewed
            });
        }

        [HttpGet("api/dashboard/api-usage")]
        [ProducesResponseType(typeof(ApiUsageChartDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiUsageChartDto>> GetApiUsage([FromQuery] int days = 7)
        {
            var data = await _dashboardService.GetApiUsageChartAsync(days);
            return Ok(new ApiUsageChartDto
            {
                Labels = data.Labels,
                RequestCounts = data.RequestCounts,
                Endpoints = data.Endpoints,
                EndpointCounts = data.EndpointCounts
            });
        }

        [HttpGet("api/dashboard/recent-activity")]
        [ProducesResponseType(typeof(RecentActivityDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<RecentActivityDto>> GetRecentActivity([FromQuery] int count = 10)
        {
            var data = await _dashboardService.GetRecentActivityAsync(count);
            return Ok(new RecentActivityDto
            {
                Items = data.Items.Select(i => new ActivityItemDto
                {
                    Type = i.Type,
                    Description = i.Description,
                    ContactName = i.ContactName,
                    Timestamp = i.Timestamp,
                    UserName = i.UserName
                }).ToList()
            });
        }
    }
}
