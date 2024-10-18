using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using MusicPortalWebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MusicPortalContext>(options => options.UseSqlServer(connection));

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100000000;
});

builder.Services.AddControllersWithViews();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 100000000;
});

var app = builder.Build();
app.UseSession();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
