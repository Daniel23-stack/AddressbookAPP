# Advanced Search & Filtering Implementation

## ✅ What Was Implemented

### 1. **SearchFilterDto** - Advanced Filtering Model
A comprehensive filter DTO that supports:
- **Search Term**: Search in name, company, notes, email, phone
- **Date Range**: Created/Updated date filters
- **Company**: Filter by company name
- **Has Email/Phone/Address**: Boolean filters
- **IsClient/IsImported**: Status filters
- **Groups/Tags**: Filter by contact groups or tags
- **Email Domain**: Filter by email domain (e.g., "@gmail.com")
- **Phone Number**: Search by phone number
- **Search in Notes**: Option to include notes in search

### 2. **SavedSearch Model** - Smart Lists
- Store search filters as reusable "Smart Lists"
- Track usage count and last used date
- User-specific saved searches
- JSON storage of filter criteria

### 3. **Repository Layer**
- **ContactRepository.AdvancedSearchAsync()**: Complex query builder with all filter options
- **ContactRepository.GetAdvancedSearchCountAsync()**: Count results without loading data
- **SavedSearchRepository**: Full CRUD for saved searches

### 4. **Service Layer**
- **ContactService.AdvancedSearchAsync()**: Business logic for advanced search
- **SavedSearchService**: Complete service for managing saved searches
  - Create, Update, Delete saved searches
  - Get user's saved searches
  - Use saved search (increments counter)

### 5. **API Endpoints**

#### Contacts Controller
- `POST /api/contacts/search/advanced` - Advanced search
- `POST /api/contacts/search/advanced/count` - Get count of search results

#### Saved Searches Controller
- `GET /api/savedsearches` - Get all user's saved searches
- `GET /api/savedsearches/{id}` - Get specific saved search
- `POST /api/savedsearches` - Create new saved search
- `PUT /api/savedsearches/{id}` - Update saved search
- `DELETE /api/savedsearches/{id}` - Delete saved search
- `POST /api/savedsearches/{id}/use` - Use saved search (increments counter)

### 6. **Database Migrations**
- `AddContactGroupsAndTags` - Adds Groups/Tags tables
- `AddSavedSearch` - Adds SavedSearches table

---

## 📋 API Usage Examples

### Advanced Search

**POST** `/api/contacts/search/advanced`
```json
{
  "searchTerm": "john",
  "createdAfter": "2024-01-01T00:00:00Z",
  "createdBefore": "2024-12-31T23:59:59Z",
  "company": "Acme",
  "hasEmail": true,
  "hasPhone": true,
  "isClient": true,
  "groupIds": ["guid-1", "guid-2"],
  "tagIds": ["guid-3"],
  "emailDomain": "gmail.com",
  "searchInNotes": true
}
```

**Response:**
```json
[
  {
    "id": "guid",
    "firstName": "John",
    "lastName": "Doe",
    "company": "Acme Corp",
    ...
  }
]
```

### Create Saved Search (Smart List)

**POST** `/api/savedsearches`
```json
{
  "name": "My Clients",
  "description": "All client contacts",
  "filterCriteria": {
    "isClient": true,
    "hasEmail": true
  }
}
```

**Response:**
```json
{
  "id": "guid",
  "name": "My Clients",
  "description": "All client contacts",
  "userId": "user-guid",
  "createdAt": "2024-12-05T09:14:16Z",
  "lastUsedAt": null,
  "useCount": 0,
  "filterCriteria": {
    "isClient": true,
    "hasEmail": true
  }
}
```

### Use Saved Search

**POST** `/api/savedsearches/{id}/use`

This endpoint:
1. Returns the saved search with its filter criteria
2. Increments the use count
3. Updates the last used date

---

## 🎯 Features Implemented

✅ **Filter by date range** (Created/Updated)  
✅ **Filter by company**  
✅ **Filter by has email/phone/address**  
✅ **Filter by IsClient/IsImported status**  
✅ **Filter by groups/tags** (when Groups/Tags feature is implemented)  
✅ **Search within notes**  
✅ **Search by phone number**  
✅ **Search by email domain**  
✅ **Save search filters as "Smart Lists"**  
✅ **Track saved search usage**  

---

## 🚀 Next Steps

### To Apply Migrations:
```powershell
dotnet ef database update --project AddressBookApp.Infrastructure --startup-project AddressBookApp.API
```

### To Use in Frontend:

1. **Create Advanced Search UI**
   - Build filter form with all options
   - Submit to `/api/contacts/search/advanced`
   - Display results

2. **Saved Searches UI**
   - List user's saved searches
   - Create new saved search from current filters
   - Click to apply saved search
   - Edit/Delete saved searches

3. **Integration with Groups/Tags**
   - Once Groups/Tags are implemented, the filter will automatically work
   - Users can filter by selected groups/tags

---

## 📝 Notes

- All search operations are **case-insensitive**
- Search term searches across: FirstName, LastName, Company, Notes (optional), Email, Phone
- Saved searches are **user-specific** (filtered by UserId)
- Filter criteria is stored as **JSON** for flexibility
- Advanced search includes related entities (Addresses, PhoneNumbers, EmailAddresses, Groups, Tags)

---

## 🔧 Technical Details

### Performance Considerations
- Uses Entity Framework LINQ queries
- Includes necessary related entities
- Count endpoint optimized (no data loading)
- Indexes on UserId and Name+UserId for SavedSearches

### Security
- All endpoints require authentication (`[Authorize]`)
- Saved searches are user-scoped (can only access own searches)
- User ID extracted from JWT claims

---

*Implementation completed! Ready to use.* 🎉

