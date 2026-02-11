using ContactManagerApp.Exceptions;
using ContactManagerApp.Mapping;
using ContactManagerApp.Models;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace ContactManagerApp.Services
{
    public class CsvImportService : ICsvImportService
    {
        public IEnumerable<Contact> ReadContactsFromCsvFile(IFormFile file)
        {
            try
            {
                using (var streamReader = new StreamReader(file.OpenReadStream()))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true,
                        MissingFieldFound = null,
                        BadDataFound = null,
                    };

                    using (var csv = new CsvReader(streamReader, config))
                    {
                        csv.Context.RegisterClassMap<ContactMap>();
                        return csv.GetRecords<Contact>().ToList();
                    }
                }
            }
            catch (HeaderValidationException ex)
            {
                throw new CsvValidationException("Csv header is invalid or missing required columns");
            }
            catch (TypeConverterException ex) 
            {
                throw new CsvValidationException($"Invalid data format in CSV: {ex.Message}");
            }
            catch(Exception ex)
            {
                throw new CsvValidationException($"Error reading CSV file: {ex.Message}");
            }
            
        }
        public bool IsValidCsvFile(IFormFile file)
        {
            if(file == null || file.Length == 0) return false;

            return Path.GetExtension(file.FileName)
                .Equals(".csv", StringComparison.OrdinalIgnoreCase);
        }
    }
}
