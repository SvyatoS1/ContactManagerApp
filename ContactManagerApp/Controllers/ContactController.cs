using ContactManagerApp.Data;
using ContactManagerApp.DTOs;
using ContactManagerApp.Exceptions;
using ContactManagerApp.Models;
using ContactManagerApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly ICsvImportService _csvImportService;
        private readonly ContactDbContext _context;
        public ContactController(ICsvImportService csvImportService, ContactDbContext context)
        {
            _csvImportService = csvImportService;
            _context = context;
        }
        public async Task<IActionResult> Index() 
        {
            var contacts = await _context.Contacts.ToListAsync();
            return View(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (!_csvImportService.IsValidCsvFile(file))
            {
                return BadRequest("Invalid file");
            }
            try
            {
                var contactsList = _csvImportService.ReadContactsFromCsvFile(file);
                _context.AddRange(contactsList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (CsvValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateContact([FromBody] UpdateContactDto updatedContact)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contact = await _context.Contacts.FindAsync(updatedContact.Id);

            if(contact == null) return NotFound();

            UpdateContactFields(contact, updatedContact);

            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private static void UpdateContactFields(Contact contact, UpdateContactDto updatedContact)
        {
            contact.Name = updatedContact.Name;
            contact.DateOfBirth = updatedContact.DateOfBirth;
            contact.Married = updatedContact.Married;
            contact.Phone = updatedContact.Phone;
            contact.Salary = updatedContact.Salary;
        }
    }
}
