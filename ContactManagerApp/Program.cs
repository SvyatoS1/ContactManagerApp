using ContactManagerApp.Data;
using ContactManagerApp.Models;
using ContactManagerApp.Services;
using ContactManagerApp.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ContactValidator>();

builder.Services.AddTransient<ICsvImportService, CsvImportService>();

builder.Services.AddDbContext<ContactDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ContactsDbConnectionString")));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contact}/{action=Index}")
    .WithStaticAssets();

app.UseStatusCodePages();


app.Run();
