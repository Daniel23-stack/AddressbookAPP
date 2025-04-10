using System.Threading.Tasks;
using AddressBookApp.Core.Interfaces;

namespace AddressBookApp.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IDataExportRepository _dataExportRepository;
        private readonly IApiUsageRepository _apiUsageRepository;
        private readonly IImportHistoryRepository _importHistoryRepository;
        
        public DashboardService(
            IContactRepository contactRepository,
            IDataExportRepository dataExportRepository,
            IApiUsageRepository apiUsageRepository,
            IImportHistoryRepository importHistoryRepository)
        {
            _contactRepository = contactRepository;
            _dataExportRepository = dataExportRepository;
            _apiUsageRepository = apiUsageRepository;
            _importHistoryRepository = importHistoryRepository;
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
    }
}
