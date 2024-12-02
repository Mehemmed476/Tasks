using MediPlus.BL.Services.Abstractions;
using MediPlus.BL.Services.Concretes;
using MediPlusApp.DAL.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MediPlusDbContext>(
    opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("MacBookMsSql"))
    );

builder.Services.AddScoped<IGenericCRUDService, GenericCRUDService>();


var app = builder.Build();

app.UseStaticFiles();
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