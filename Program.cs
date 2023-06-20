using MySqlConnector;
using Microsoft.EntityFrameworkCore;
using MvcMovie.DAL;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetConnectionString("Default")!;

builder.Services.AddTransient<MySqlConnection>(_ =>
    new MySqlConnection(connectionString));

builder.Services.AddDbContext<ProductContext>(options => options.UseMySQL(connectionString));
builder.Services.AddDbContext<SchoolContext>(options => options.UseMySQL(connectionString));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute("student_route", "Student", "Student/{controller}/{action}/{id?}");


app.Run();
