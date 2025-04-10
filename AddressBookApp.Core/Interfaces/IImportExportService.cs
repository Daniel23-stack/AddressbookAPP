using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AddressBookApp.Core.Models;

namespace AddressBookApp.Core.Interfaces
{
    public interface IImportExportService
    {
        Task<byte[]> ExportContactsToCsvAsync(Guid userId);
        Task<byte[]> ExportClientsToCsvAsync(Guid userId);
        Task<(int TotalRecords, int SuccessfulRecords, int FailedRecords)> ImportContactsFromCsvAsync(Stream fileStream, Guid userId);
        Task<IReadOnlyList<DataExport>> GetExportHistoryAsync();
        Task<IReadOnlyList<ImportHistory>> GetImportHistoryAsync();
    }
}
