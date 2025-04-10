using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBookApp.Core.Interfaces;
using AddressBookApp.Core.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace AddressBookApp.Application.Services
{
    public class ImportExportService : IImportExportService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IDataExportRepository _dataExportRepository;
        private readonly IImportHistoryRepository _importHistoryRepository;
        
        public ImportExportService(
            IContactRepository contactRepository,
            IDataExportRepository dataExportRepository,
            IImportHistoryRepository importHistoryRepository)
        {
            _contactRepository = contactRepository;
            _dataExportRepository = dataExportRepository;
            _importHistoryRepository = importHistoryRepository;
        }
        
        public async Task<byte[]> ExportContactsToCsvAsync(Guid userId)
        {
            var contacts = await _contactRepository.ListAllAsync();
            
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(contacts.Select(c => new
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Company = c.Company,
                    Email = c.EmailAddresses.FirstOrDefault()?.Email ?? "",
                    Phone = c.PhoneNumbers.FirstOrDefault()?.Number ?? "",
                    IsClient = c.IsClient,
                    Notes = c.Notes
                }));
                
                writer.Flush();
                
                // Record the export
                await _dataExportRepository.AddAsync(new DataExport
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    FileName = $"contacts_export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv",
                    RecordCount = contacts.Count,
                    ExportedAt = DateTime.UtcNow,
                    Type = ExportType.Csv
                });
                
                return memoryStream.ToArray();
            }
        }
        
        public async Task<byte[]> ExportClientsToCsvAsync(Guid userId)
        {
            var clients = await _contactRepository.GetClientContactsAsync();
            
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(clients.Select(c => new
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Company = c.Company,
                    Email = c.EmailAddresses.FirstOrDefault()?.Email ?? "",
                    Phone = c.PhoneNumbers.FirstOrDefault()?.Number ?? "",
                    Notes = c.Notes
                }));
                
                writer.Flush();
                
                // Record the export
                await _dataExportRepository.AddAsync(new DataExport
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    FileName = $"clients_export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv",
                    RecordCount = clients.Count,
                    ExportedAt = DateTime.UtcNow,
                    Type = ExportType.Csv
                });
                
                return memoryStream.ToArray();
            }
        }
        
        public async Task<(int TotalRecords, int SuccessfulRecords, int FailedRecords)> ImportContactsFromCsvAsync(Stream fileStream, Guid userId)
        {
            int totalRecords = 0;
            int successfulRecords = 0;
            int failedRecords = 0;
            
            try
            {
                using (var reader = new StreamReader(fileStream))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null
                }))
                {
                    var records = csv.GetRecords<dynamic>().ToList();
                    totalRecords = records.Count;
                    
                    foreach (var record in records)
                    {
                        try
                        {
                            var contact = new Contact
                            {
                                Id = Guid.NewGuid(),
                                FirstName = record.FirstName,
                                LastName = record.LastName,
                                Company = record.Company,
                                Notes = record.Notes,
                                IsClient = record.IsClient == "True" || record.IsClient == "true" || record.IsClient == "1",
                                IsImported = true,
                                ImportedById = userId,
                                CreatedAt = DateTime.UtcNow
                            };
                            
                            // Add email if present
                            if (!string.IsNullOrEmpty(record.Email))
                            {
                                contact.EmailAddresses.Add(new EmailAddress
                                {
                                    Id = Guid.NewGuid(),
                                    Email = record.Email,
                                    Type = EmailType.Work
                                });
                            }
                            
                            // Add phone if present
                            if (!string.IsNullOrEmpty(record.Phone))
                            {
                                contact.PhoneNumbers.Add(new PhoneNumber
                                {
                                    Id = Guid.NewGuid(),
                                    Number = record.Phone,
                                    Type = PhoneType.Work
                                });
                            }
                            
                            await _contactRepository.AddAsync(contact);
                            successfulRecords++;
                        }
                        catch
                        {
                            failedRecords++;
                        }
                    }
                }
                
                // Record the import
                await _importHistoryRepository.AddAsync(new ImportHistory
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    FileName = $"import_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv",
                    TotalRecords = totalRecords,
                    SuccessfulRecords = successfulRecords,
                    FailedRecords = failedRecords,
                    ImportedAt = DateTime.UtcNow
                });
                
                return (totalRecords, successfulRecords, failedRecords);
            }
            catch (Exception)
            {
                // Record the failed import
                await _importHistoryRepository.AddAsync(new ImportHistory
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    FileName = $"import_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv",
                    TotalRecords = totalRecords,
                    SuccessfulRecords = successfulRecords,
                    FailedRecords = failedRecords,
                    ImportedAt = DateTime.UtcNow
                });
                
                throw;
            }
        }
        
        public async Task<IReadOnlyList<DataExport>> GetExportHistoryAsync()
        {
            return await _dataExportRepository.ListAllAsync();
        }
        
        public async Task<IReadOnlyList<ImportHistory>> GetImportHistoryAsync()
        {
            return await _importHistoryRepository.ListAllAsync();
        }
    }
}
