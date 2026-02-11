using CsvHelper.Configuration.Attributes;

namespace ContactManagerApp.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public bool Married { get; set; }
        public string? Phone { get; set; }
        public decimal Salary { get; set; }
    }
}
