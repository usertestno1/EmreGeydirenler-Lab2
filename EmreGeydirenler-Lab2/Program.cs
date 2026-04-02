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

    if (!dbContext.PlanModules.Any())
    {
        dbContext.PlanModules.AddRange(
            new PlanModule { PlanId = 1, ModuleId = 1, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 1, ModuleId = 5, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 2, ModuleId = 1, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 2, ModuleId = 2, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 2, ModuleId = 5, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 3, ModuleId = 1, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 3, ModuleId = 2, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 3, ModuleId = 3, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 3, ModuleId = 5, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 4, ModuleId = 1, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 4, ModuleId = 2, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 4, ModuleId = 3, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 4, ModuleId = 4, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 4, ModuleId = 5, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 5, ModuleId = 1, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 5, ModuleId = 2, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 5, ModuleId = 3, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 5, ModuleId = 4, SubscriptionPlan = null!, Module = null! },
            new PlanModule { PlanId = 5, ModuleId = 5, SubscriptionPlan = null!, Module = null! });
    }

    if (!dbContext.AccountSettings.Any())
    {
        dbContext.AccountSettings.AddRange(
            new AccountSetting { CustomerId = 2001, TwoFactorEnabled = true, ReceiveEmailAlerts = true, Customer = null! },
            new AccountSetting { CustomerId = 2002, TwoFactorEnabled = false, ReceiveEmailAlerts = true, Customer = null! },
            new AccountSetting { CustomerId = 2003, TwoFactorEnabled = true, ReceiveEmailAlerts = false, Customer = null! },
            new AccountSetting { CustomerId = 2004, TwoFactorEnabled = false, ReceiveEmailAlerts = true, Customer = null! },
            new AccountSetting { CustomerId = 2005, TwoFactorEnabled = true, ReceiveEmailAlerts = true, Customer = null! });
    }

    if (!dbContext.Subscriptions.Any())
    {
        dbContext.Subscriptions.AddRange(
            new Subscription { Id = 3001, StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), Status = "Active", CustomerId = 2001, Customer = null!, PlanId = 2, SubscriptionPlan = null! },
            new Subscription { Id = 3002, StartDate = new DateTime(2026, 2, 1), EndDate = new DateTime(2027, 1, 31), Status = "Active", CustomerId = 2002, Customer = null!, PlanId = 3, SubscriptionPlan = null! },
            new Subscription { Id = 3003, StartDate = new DateTime(2025, 10, 1), EndDate = new DateTime(2026, 9, 30), Status = "Active", CustomerId = 2003, Customer = null!, PlanId = 4, SubscriptionPlan = null! },
            new Subscription { Id = 3004, StartDate = new DateTime(2025, 7, 1), EndDate = new DateTime(2026, 6, 30), Status = "Active", CustomerId = 2004, Customer = null!, PlanId = 1, SubscriptionPlan = null! },
            new Subscription { Id = 3005, StartDate = new DateTime(2026, 3, 1), EndDate = new DateTime(2027, 2, 28), Status = "Active", CustomerId = 2005, Customer = null!, PlanId = 5, SubscriptionPlan = null! });
    }

    if (!dbContext.Invoices.Any())
    {
        dbContext.Invoices.AddRange(
            new Invoice { Id = 4001, IssueDate = new DateTime(2026, 3, 1), DueDate = new DateTime(2026, 3, 15), TotalAmount = 79m, IsPaid = true, CustomerId = 2001, Customer = null! },
            new Invoice { Id = 4002, IssueDate = new DateTime(2026, 3, 1), DueDate = new DateTime(2026, 3, 15), TotalAmount = 149m, IsPaid = true, CustomerId = 2002, Customer = null! },
            new Invoice { Id = 4003, IssueDate = new DateTime(2026, 3, 1), DueDate = new DateTime(2026, 3, 15), TotalAmount = 299m, IsPaid = false, CustomerId = 2003, Customer = null! },
            new Invoice { Id = 4004, IssueDate = new DateTime(2026, 3, 1), DueDate = new DateTime(2026, 3, 15), TotalAmount = 29m, IsPaid = true, CustomerId = 2004, Customer = null! },
            new Invoice { Id = 4005, IssueDate = new DateTime(2026, 3, 1), DueDate = new DateTime(2026, 3, 15), TotalAmount = 599m, IsPaid = false, CustomerId = 2005, Customer = null! });
    }

    if (!dbContext.UsageRecords.Any())
    {
        dbContext.UsageRecords.AddRange(
            new UsageRecord { Id = 5001, LogDate = new DateTime(2026, 3, 20), ApiRequestsCount = 3420, StorageUsedMb = 15360m, CustomerId = 2001, Customer = null! },
            new UsageRecord { Id = 5002, LogDate = new DateTime(2026, 3, 20), ApiRequestsCount = 8750, StorageUsedMb = 28160m, CustomerId = 2002, Customer = null! },
            new UsageRecord { Id = 5003, LogDate = new DateTime(2026, 3, 20), ApiRequestsCount = 15120, StorageUsedMb = 65280m, CustomerId = 2003, Customer = null! },
            new UsageRecord { Id = 5004, LogDate = new DateTime(2026, 3, 20), ApiRequestsCount = 950, StorageUsedMb = 3840m, CustomerId = 2004, Customer = null! },
            new UsageRecord { Id = 5005, LogDate = new DateTime(2026, 3, 20), ApiRequestsCount = 23890, StorageUsedMb = 121000m, CustomerId = 2005, Customer = null! });
    }

    if (!dbContext.AuditTrails.Any())
    {
        dbContext.AuditTrails.AddRange(
            new AuditTrail { Id = 6001, ActionDescription = "Created the Growth plan campaign settings.", Timestamp = new DateTime(2026, 3, 10, 9, 15, 0), IPAddress = "10.10.1.11", UserId = 1001, User = null! },
            new AuditTrail { Id = 6002, ActionDescription = "Reviewed invoice disputes for enterprise clients.", Timestamp = new DateTime(2026, 3, 12, 11, 45, 0), IPAddress = "10.10.1.12", UserId = 1002, User = null! },
            new AuditTrail { Id = 6003, ActionDescription = "Updated security rules for admin sign-in.", Timestamp = new DateTime(2026, 3, 14, 14, 5, 0), IPAddress = "10.10.1.13", UserId = 1003, User = null! },
            new AuditTrail { Id = 6004, ActionDescription = "Approved plan pricing adjustment request.", Timestamp = new DateTime(2026, 3, 16, 16, 30, 0), IPAddress = "10.10.1.14", UserId = 1004, User = null! },
            new AuditTrail { Id = 6005, ActionDescription = "Published monthly usage analytics report.", Timestamp = new DateTime(2026, 3, 18, 10, 0, 0), IPAddress = "10.10.1.15", UserId = 1005, User = null! });
    }

    dbContext.SaveChanges();
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
