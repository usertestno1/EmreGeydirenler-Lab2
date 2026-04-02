using Microsoft.EntityFrameworkCore;

namespace EmreGeydirenler_Lab2.Models
{
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
                    FirstName = "Aylin",
                    LastName = "Demir",
                    Email = "aylin.demir@qutech.com",
                    Password = "Admin@123",
                    PhoneNumber = "+90-212-555-0101",
                    Address = "Maslak Mah. IT Plaza No:12, Istanbul",
                    CreatedAt = new DateTime(2024, 1, 15),
                    AdminRole = "SuperAdmin",
                    Department = "Platform Operations"
                },
                new Admin
                {
                    Id = 1002,
                    FirstName = "Mert",
                    LastName = "Kara",
                    Email = "mert.kara@qutech.com",
                    Password = "Support@123",
                    PhoneNumber = "+90-212-555-0102",
                    Address = "Levent Mah. Teknoloji Cad. No:8, Istanbul",
                    CreatedAt = new DateTime(2024, 2, 10),
                    AdminRole = "SupportAdmin",
                    Department = "Customer Success"
                },
                new Admin
                {
                    Id = 1003,
                    FirstName = "Selin",
                    LastName = "Yilmaz",
                    Email = "selin.yilmaz@qutech.com",
                    Password = "Security@123",
                    PhoneNumber = "+90-312-555-0103",
                    Address = "Cankaya Mah. Is Merkezi No:4, Ankara",
                    CreatedAt = new DateTime(2024, 3, 5),
                    AdminRole = "SecurityAdmin",
                    Department = "Information Security"
                },
                new Admin
                {
                    Id = 1004,
                    FirstName = "Can",
                    LastName = "Aydin",
                    Email = "can.aydin@qutech.com",
                    Password = "Billing@123",
                    PhoneNumber = "+90-232-555-0104",
                    Address = "Alsancak Mah. Inovasyon Sok. No:19, Izmir",
                    CreatedAt = new DateTime(2024, 4, 18),
                    AdminRole = "BillingAdmin",
                    Department = "Finance Operations"
                },
                new Admin
                {
                    Id = 1005,
                    FirstName = "Ece",
                    LastName = "Sahin",
                    Email = "ece.sahin@qutech.com",
                    Password = "Product@123",
                    PhoneNumber = "+90-216-555-0105",
                    Address = "Atasehir Mah. Startup Cad. No:22, Istanbul",
                    CreatedAt = new DateTime(2024, 5, 2),
                    AdminRole = "ProductAdmin",
                    Department = "Product Management"
                }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 2001,
                    FirstName = "Burak",
                    LastName = "Celik",
                    Email = "burak.celik@novafoods.com",
                    PhoneNumber = "+90-212-555-1101",
                    Address = "Beylikduzu OSB 2. Cad. No:45, Istanbul",
                    CreatedAt = new DateTime(2024, 1, 20),
                    CompanyName = "Nova Foods A.S.",
                    TaxNumber = "TR3456789012"
                },
                new Customer
                {
                    Id = 2002,
                    FirstName = "Zeynep",
                    LastName = "Arslan",
                    Email = "zeynep.arslan@metallink.com",
                    PhoneNumber = "+90-224-555-1102",
                    Address = "Nilufer Sanayi Bolgesi No:77, Bursa",
                    CreatedAt = new DateTime(2024, 2, 8),
                    CompanyName = "MetalLink Manufacturing Ltd.",
                    TaxNumber = "TR4567890123"
                },
                new Customer
                {
                    Id = 2003,
                    FirstName = "Kerem",
                    LastName = "Toprak",
                    Email = "kerem.toprak@medisync.io",
                    PhoneNumber = "+90-312-555-1103",
                    Address = "ODTU Teknokent A Blok No:9, Ankara",
                    CreatedAt = new DateTime(2024, 3, 12),
                    CompanyName = "MediSync Health Tech",
                    TaxNumber = "TR5678901234"
                },
                new Customer
                {
                    Id = 2004,
                    FirstName = "Elif",
                    LastName = "Tas",
                    Email = "elif.tas@logiroad.com",
                    PhoneNumber = "+90-232-555-1104",
                    Address = "Kemalpasa Lojistik Merkezi No:31, Izmir",
                    CreatedAt = new DateTime(2024, 4, 6),
                    CompanyName = "LogiRoad Logistics",
                    TaxNumber = "TR6789012345"
                },
                new Customer
                {
                    Id = 2005,
                    FirstName = "Onur",
                    LastName = "Gunes",
                    Email = "onur.gunes@finpeak.co",
                    PhoneNumber = "+90-216-555-1105",
                    Address = "Kadikoy Finans Merkezi No:14, Istanbul",
                    CreatedAt = new DateTime(2024, 5, 25),
                    CompanyName = "FinPeak Advisory",
                    TaxNumber = "TR7890123456"
                }
            );

        }
    }
}
