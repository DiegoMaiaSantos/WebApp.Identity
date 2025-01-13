using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApp.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = "Data Source=localhost\\sqlexpress;" +
                       "Initial Catalog=WebApp_Identity;" +
                       "Integrated Security=SSPI;" +
                       "TrustServerCertificate=True;";

var migrationAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name; 

builder.Services.AddDbContext<MyUserDbContext>(
    options => options.UseSqlServer(connectionString, 
    sql => sql.MigrationsAssembly(migrationAssembly))
);

builder.Services.AddIdentity<MyUser, IdentityRole>(options => { })
    .AddEntityFrameworkStores<MyUserDbContext>();

builder.Services.ConfigureApplicationCookie(options => 
    options.LoginPath = "/Home/Login");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseAuthentication();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
