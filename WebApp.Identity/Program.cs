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

builder.Services.AddIdentity<MyUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;

    //options.Password.RequireDigit = false;
    //options.Password.RequireNonAlphanumeric = false;
    //options.Password.RequireUppercase = false;
    //options.Password.RequireLowercase = false;
    //options.Password.RequiredLength = 6;

    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
})
    .AddEntityFrameworkStores<MyUserDbContext>()
    .AddDefaultTokenProviders()
    .AddPasswordValidator<DoesNotContainPasswordValidation<MyUser>>();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<MyUser>,
    MyUserClaimsPrincipalFactory>();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromHours(3));

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
