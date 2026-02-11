using ContactManagerApp.Models;
using CsvHelper.Configuration;

namespace ContactManagerApp.Mapping
{
    public class ContactMap : ClassMap<Contact>
    {
        public ContactMap()
        {
            Map(m => m.Id).Ignore();
            Map(m => m.Name).Name("Name");
            Map(m => m.DateOfBirth).Name("Date of Birth");
            Map(m => m.Married).Name("Married");
            Map(m => m.Phone).Name("Phone");
            Map(m => m.Salary).Name("Salary");
        }
    }
}
