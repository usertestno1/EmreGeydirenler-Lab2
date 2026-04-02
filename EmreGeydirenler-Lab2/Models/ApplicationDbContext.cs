using Microsoft.EntityFrameworkCore;

namespace EmreGeydirenler_Lab2.Models
{
    // Central EF Core context for app entities, relationships, and sample seed data.
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
        public DbSet<Module> Modules => Set<Module>();
        public DbSet<PlanModule> PlanModules => Set<PlanModule>();
        public DbSet<BaseUser> BaseUsers => Set<BaseUser>();
        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<AuditTrail> AuditTrails => Set<AuditTrail>();
        public DbSet<UsageRecord> UsageRecords => Set<UsageRecord>();
        public DbSet<AccountSetting> AccountSettings => Set<AccountSetting>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Store Admin and Customer in one table using a discriminator column.
            modelBuilder.Entity<BaseUser>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Admin>("Admin")
                .HasValue<Customer>("Customer");

            modelBuilder.Entity<PlanModule>()
                .HasKey(pm => new { pm.PlanId, pm.ModuleId });

            modelBuilder.Entity<PlanModule>()
                .HasOne(pm => pm.SubscriptionPlan)
                .WithMany(p => p.PlanModules)
                .HasForeignKey(pm => pm.PlanId);

            modelBuilder.Entity<PlanModule>()
                .HasOne(pm => pm.Module)
                .WithMany(m => m.PlanModules)
                .HasForeignKey(pm => pm.ModuleId);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.AccountSetting)
                .WithOne(a => a.Customer)
                .HasForeignKey<AccountSetting>(a => a.CustomerId);

            modelBuilder.Entity<SubscriptionPlan>().HasData(
                new SubscriptionPlan { Id = 1, PlanName = "Starter", MonthlyPrice = 29m, MaxUsers = 5, IsActive = true },
                new SubscriptionPlan { Id = 2, PlanName = "Growth", MonthlyPrice = 79m, MaxUsers = 20, IsActive = true },
                new SubscriptionPlan { Id = 3, PlanName = "Professional", MonthlyPrice = 149m, MaxUsers = 50, IsActive = true },
                new SubscriptionPlan { Id = 4, PlanName = "Scale", MonthlyPrice = 299m, MaxUsers = 150, IsActive = true },
                new SubscriptionPlan { Id = 5, PlanName = "Enterprise", MonthlyPrice = 599m, MaxUsers = 500, IsActive = true }
            );

            modelBuilder.Entity<Module>().HasData(
                new Module { Id = 1, ModuleName = "Billing Automation", Description = "Automates recurring invoices, payment reminders, and ledger exports." },
                new Module { Id = 2, ModuleName = "CRM Pipeline", Description = "Tracks leads, deal stages, and customer lifecycle activities." },
                new Module { Id = 3, ModuleName = "Workflow Engine", Description = "Designs approval flows and task routing for internal processes." },
                new Module { Id = 4, ModuleName = "Analytics Hub", Description = "Provides KPI dashboards, cohort analysis, and custom reporting." },
                new Module { Id = 5, ModuleName = "Access Control", Description = "Manages role-based permissions, SSO integration, and audit visibility." }
            );

            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1001,
                    FirstName = "Olivia",
                    LastName = "Bennett",
                    Email = "olivia.bennett@qutech.com",
                    Password = "Admin@123",
                    PhoneNumber = "+61 2 9001 1001",
                    Address = "Level 12, 200 George Street, Sydney NSW 2000",
                    CreatedAt = new DateTime(2024, 1, 15),
                    AdminRole = "SuperAdmin",
                    Department = "Platform Operations"
                },
                new Admin
                {
                    Id = 1002,
                    FirstName = "Liam",
                    LastName = "Carter",
                    Email = "liam.carter@qutech.com",
                    Password = "Support@123",
                    PhoneNumber = "+61 3 9001 1002",
                    Address = "85 Collins Street, Melbourne VIC 3000",
                    CreatedAt = new DateTime(2024, 2, 10),
                    AdminRole = "SupportAdmin",
                    Department = "Customer Success"
                },
                new Admin
                {
                    Id = 1003,
                    FirstName = "Charlotte",
                    LastName = "Nguyen",
                    Email = "charlotte.nguyen@qutech.com",
                    Password = "Security@123",
                    PhoneNumber = "+61 7 9001 1003",
                    Address = "40 Adelaide Street, Brisbane QLD 4000",
                    CreatedAt = new DateTime(2024, 3, 5),
                    AdminRole = "SecurityAdmin",
                    Department = "Information Security"
                },
                new Admin
                {
                    Id = 1004,
                    FirstName = "Noah",
                    LastName = "Reed",
                    Email = "noah.reed@qutech.com",
                    Password = "Billing@123",
                    PhoneNumber = "+61 8 9001 1004",
                    Address = "27 St Georges Terrace, Perth WA 6000",
                    CreatedAt = new DateTime(2024, 4, 18),
                    AdminRole = "BillingAdmin",
                    Department = "Finance Operations"
                },
                new Admin
                {
                    Id = 1005,
                    FirstName = "Amelia",
                    LastName = "Foster",
                    Email = "amelia.foster@qutech.com",
                    Password = "Product@123",
                    PhoneNumber = "+61 2 9001 1005",
                    Address = "15 Barangaroo Avenue, Sydney NSW 2000",
                    CreatedAt = new DateTime(2024, 5, 2),
                    AdminRole = "ProductAdmin",
                    Department = "Product Management"
                }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 2001,
                    FirstName = "Ethan",
                    LastName = "Walker",
                    Email = "ethan.walker@harborfoods.com.au",
                    Password = "Customer@123",
                    PhoneNumber = "+61 2 8012 1101",
                    Address = "48 Darling Drive, Pyrmont NSW 2009",
                    CreatedAt = new DateTime(2024, 1, 20),
                    CompanyName = "Harbor Foods Pty Ltd",
                    TaxNumber = "ABN 34 567 890 123"
                },
                new Customer
                {
                    Id = 2002,
                    FirstName = "Sophie",
                    LastName = "Mitchell",
                    Email = "sophie.mitchell@southernsteel.com.au",
                    Password = "Customer@234",
                    PhoneNumber = "+61 3 8012 1102",
                    Address = "190 Flinders Street, Melbourne VIC 3000",
                    CreatedAt = new DateTime(2024, 2, 8),
                    CompanyName = "Southern Steel Manufacturing Pty Ltd",
                    TaxNumber = "ABN 45 678 901 234"
                },
                new Customer
                {
                    Id = 2003,
                    FirstName = "Jack",
                    LastName = "Thompson",
                    Email = "jack.thompson@meditrackhealth.com.au",
                    Password = "Customer@345",
                    PhoneNumber = "+61 7 8012 1103",
                    Address = "22 Eagle Street, Brisbane QLD 4000",
                    CreatedAt = new DateTime(2024, 3, 12),
                    CompanyName = "MediTrack Health Systems Pty Ltd",
                    TaxNumber = "ABN 56 789 012 345"
                },
                new Customer
                {
                    Id = 2004,
                    FirstName = "Grace",
                    LastName = "Evans",
                    Email = "grace.evans@pacificfreight.com.au",
                    Password = "Customer@456",
                    PhoneNumber = "+61 8 8012 1104",
                    Address = "90 Terrace Road, East Perth WA 6004",
                    CreatedAt = new DateTime(2024, 4, 6),
                    CompanyName = "Pacific Freight Logistics Pty Ltd",
                    TaxNumber = "ABN 67 890 123 456"
                },
                new Customer
                {
                    Id = 2005,
                    FirstName = "Mason",
                    LastName = "Parker",
                    Email = "mason.parker@australadvisory.com.au",
                    Password = "Customer@567",
                    PhoneNumber = "+61 2 8012 1105",
                    Address = "101 Pitt Street, Sydney NSW 2000",
                    CreatedAt = new DateTime(2024, 5, 25),
                    CompanyName = "Austral Advisory Group Pty Ltd",
                    TaxNumber = "ABN 78 901 234 567"
                }
            );

            modelBuilder.Entity<PlanModule>().HasData(
                new { PlanId = 1, ModuleId = 1 },
                new { PlanId = 1, ModuleId = 5 },
                new { PlanId = 2, ModuleId = 1 },
                new { PlanId = 2, ModuleId = 2 },
                new { PlanId = 2, ModuleId = 5 },
                new { PlanId = 3, ModuleId = 1 },
                new { PlanId = 3, ModuleId = 2 },
                new { PlanId = 3, ModuleId = 3 },
                new { PlanId = 3, ModuleId = 5 },
                new { PlanId = 4, ModuleId = 1 },
                new { PlanId = 4, ModuleId = 2 },
                new { PlanId = 4, ModuleId = 3 },
                new { PlanId = 4, ModuleId = 4 },
                new { PlanId = 4, ModuleId = 5 },
                new { PlanId = 5, ModuleId = 1 },
                new { PlanId = 5, ModuleId = 2 },
                new { PlanId = 5, ModuleId = 3 },
                new { PlanId = 5, ModuleId = 4 },
                new { PlanId = 5, ModuleId = 5 }
            );

            modelBuilder.Entity<AccountSetting>().HasData(
                new { CustomerId = 2001, TwoFactorEnabled = true, ReceiveEmailAlerts = true },
                new { CustomerId = 2002, TwoFactorEnabled = false, ReceiveEmailAlerts = true },
                new { CustomerId = 2003, TwoFactorEnabled = true, ReceiveEmailAlerts = false },
                new { CustomerId = 2004, TwoFactorEnabled = false, ReceiveEmailAlerts = true },
                new { CustomerId = 2005, TwoFactorEnabled = true, ReceiveEmailAlerts = true }
            );

            modelBuilder.Entity<Subscription>().HasData(
                new { Id = 3001, StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), Status = "Active", CustomerId = 2001, PlanId = 2 },
                new { Id = 3002, StartDate = new DateTime(2026, 2, 1), EndDate = new DateTime(2027, 1, 31), Status = "Active", CustomerId = 2002, PlanId = 3 },
                new { Id = 3003, StartDate = new DateTime(2025, 10, 1), EndDate = new DateTime(2026, 9, 30), Status = "Active", CustomerId = 2003, PlanId = 4 },
                new { Id = 3004, StartDate = new DateTime(2025, 7, 1), EndDate = new DateTime(2026, 6, 30), Status = "Active", CustomerId = 2004, PlanId = 1 },
                new { Id = 3005, StartDate = new DateTime(2026, 3, 1), EndDate = new DateTime(2027, 2, 28), Status = "Active", CustomerId = 2005, PlanId = 5 }
            );

            modelBuilder.Entity<Invoice>().HasData(
                new { Id = 4001, IssueDate = new DateTime(2026, 3, 1), DueDate = new DateTime(2026, 3, 15), TotalAmount = 79m, IsPaid = true, CustomerId = 2001 },
                new { Id = 4002, IssueDate = new DateTime(2026, 3, 1), DueDate = new DateTime(2026, 3, 15), TotalAmount = 149m, IsPaid = true, CustomerId = 2002 },
                new { Id = 4003, IssueDate = new DateTime(2026, 3, 1), DueDate = new DateTime(2026, 3, 15), TotalAmount = 299m, IsPaid = false, CustomerId = 2003 },
                new { Id = 4004, IssueDate = new DateTime(2026, 3, 1), DueDate = new DateTime(2026, 3, 15), TotalAmount = 29m, IsPaid = true, CustomerId = 2004 },
                new { Id = 4005, IssueDate = new DateTime(2026, 3, 1), DueDate = new DateTime(2026, 3, 15), TotalAmount = 599m, IsPaid = false, CustomerId = 2005 }
            );

            modelBuilder.Entity<UsageRecord>().HasData(
                new { Id = 5001, LogDate = new DateTime(2026, 3, 20), ApiRequestsCount = 3420, StorageUsedMb = 15360m, CustomerId = 2001 },
                new { Id = 5002, LogDate = new DateTime(2026, 3, 20), ApiRequestsCount = 8750, StorageUsedMb = 28160m, CustomerId = 2002 },
                new { Id = 5003, LogDate = new DateTime(2026, 3, 20), ApiRequestsCount = 15120, StorageUsedMb = 65280m, CustomerId = 2003 },
                new { Id = 5004, LogDate = new DateTime(2026, 3, 20), ApiRequestsCount = 950, StorageUsedMb = 3840m, CustomerId = 2004 },
                new { Id = 5005, LogDate = new DateTime(2026, 3, 20), ApiRequestsCount = 23890, StorageUsedMb = 121000m, CustomerId = 2005 }
            );

            modelBuilder.Entity<AuditTrail>().HasData(
                new { Id = 6001, ActionDescription = "Created the Growth plan campaign settings.", Timestamp = new DateTime(2026, 3, 10, 9, 15, 0), IPAddress = "10.10.1.11", UserId = 1001 },
                new { Id = 6002, ActionDescription = "Reviewed invoice disputes for enterprise clients.", Timestamp = new DateTime(2026, 3, 12, 11, 45, 0), IPAddress = "10.10.1.12", UserId = 1002 },
                new { Id = 6003, ActionDescription = "Updated security rules for admin sign-in.", Timestamp = new DateTime(2026, 3, 14, 14, 5, 0), IPAddress = "10.10.1.13", UserId = 1003 },
                new { Id = 6004, ActionDescription = "Approved plan pricing adjustment request.", Timestamp = new DateTime(2026, 3, 16, 16, 30, 0), IPAddress = "10.10.1.14", UserId = 1004 },
                new { Id = 6005, ActionDescription = "Published monthly usage analytics report.", Timestamp = new DateTime(2026, 3, 18, 10, 0, 0), IPAddress = "10.10.1.15", UserId = 1005 }
            );

        }
    }
}
