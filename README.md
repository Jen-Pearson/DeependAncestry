# DeependAncestry

## Summary of Tasks completed
1. Task 1a: Create Home page / simple search page
2. Task 2a: Implement partial name with optional gender searching to find matching records from the data store
3. Task 2b: Implement paging on search results
4. Task 3: Implement advanced search - allowing ancestors or descendants search based on exact name matching - up to a maximum of 10 records (including optional gender for filtering results)
 

## Notes
* Appsettings key controls whether small or large sample json file is loaded
* Data is loaded on initial application start - so initial load can be slow whilst loading the data the initial time.  Could create a keep alive service to prevent a real user experiencing a delay
* Basic tests have been added - if it was a real solution then more would be added
* No file validation is occurring due to prototype nature of solution
* Checkboxes could be set to gender enumeration - time constraints led to it being bound to a constructed SelectItemList
* Maximum page size could be made configurable - currently hardcoded to 10 results for advanced search
* Pager could be hidden if only one page of results found
* PagedList component used for paging was problematic so a quick and dirty approach due to time constraints was used for passing back relevant model data
* UI Could be cleaned up to better make use of partials.  Result listing was a start on this effort
* UI could definitely be cleaned up to make it look nicer