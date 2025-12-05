using System;
using System.Collections.Generic;

namespace AddressBookApp.Core.Models
{
    public class SearchFilter
    {
        public string? SearchTerm { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public DateTime? UpdatedAfter { get; set; }
        public DateTime? UpdatedBefore { get; set; }
        public string? Company { get; set; }
        public bool? HasEmail { get; set; }
        public bool? HasPhone { get; set; }
        public bool? HasAddress { get; set; }
        public bool? IsClient { get; set; }
        public bool? IsImported { get; set; }
        public List<Guid>? GroupIds { get; set; }
        public List<Guid>? TagIds { get; set; }
        public bool? SearchInNotes { get; set; }
        public string? EmailDomain { get; set; }
        public string? PhoneNumber { get; set; }
    }
}

