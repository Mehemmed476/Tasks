using EmployeeManagement.BL.Services.Abstractions;
using EmployeeManagement.BL.Services.Concretes;
using EmployeeManagement.CORE.Models;
using EmployeeManagement.DAL.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(
    opt =>
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString("MacBookMsSql"));
    }
    );

builder.Services.AddIdentity<AppUser, IdentityRole>(
    opt =>
    {
        opt.Password.RequiredLength = 8;
        opt.Password.RequireUppercase = true;
        opt.Password.RequireNonAlphanumeric = false;
        opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        opt.SignIn.RequireConfirmedEmail = false;
        opt.User.RequireUniqueEmail = true;
    }
).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<IGenericCRUDService, GenericCRUDService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name : "areas",
    pattern : "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();