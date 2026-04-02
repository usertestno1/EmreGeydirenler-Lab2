using EmreGeydirenler_Lab2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });
builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    var seededAdminPasswords = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["aylin.demir@qutech.com"] = "Admin@123",
        ["mert.kara@qutech.com"] = "Support@123",
        ["selin.yilmaz@qutech.com"] = "Security@123",
        ["can.aydin@qutech.com"] = "Billing@123",
        ["ece.sahin@qutech.com"] = "Product@123"
    };

    var adminsToFix = dbContext.Admins
        .Where(a => string.IsNullOrWhiteSpace(a.Password))
        .ToList();

    foreach (var admin in adminsToFix)
    {
        if (seededAdminPasswords.TryGetValue(admin.Email, out var password))
        {
            admin.Password = password;
        }
    }

    if (adminsToFix.Count > 0)
    {
        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
