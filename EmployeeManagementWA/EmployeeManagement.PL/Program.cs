using EmployeeManagement.BL.Services.Abstractions;
using EmployeeManagement.BL.Services.Concretes;
using EmployeeManagement.DAL.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(
    opt =>
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString("MacBookMsSql"));
    }
    );

builder.Services.AddScoped<IGenericCRUDService, GenericCRUDService>();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name : "areas",
    pattern : "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();