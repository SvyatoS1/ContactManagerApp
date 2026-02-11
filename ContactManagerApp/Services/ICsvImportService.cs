using ContactManagerApp.Models;

namespace ContactManagerApp.Services
{
    public interface ICsvImportService
    {
        public IEnumerable<Contact> ReadContactsFromCsvFile(IFormFile file);
        public bool IsValidCsvFile(IFormFile file); 
    }
}
