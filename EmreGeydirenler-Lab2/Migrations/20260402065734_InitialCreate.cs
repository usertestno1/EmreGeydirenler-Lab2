using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmreGeydirenler_Lab2.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserType = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    AdminRole = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Department = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    CompanyName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TaxNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModuleName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlanName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MonthlyPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    MaxUsers = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountSetting",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReceiveEmailAlerts = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSetting", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_AccountSetting_BaseUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActionDescription = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IPAddress = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditTrails_BaseUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IssueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsPaid = table.Column<bool>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_BaseUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsageRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LogDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ApiRequestsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    StorageUsedMb = table.Column<decimal>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsageRecords_BaseUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanModules",
                columns: table => new
                {
                    PlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    ModuleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanModules", x => new { x.PlanId, x.ModuleId });
                    table.ForeignKey(
                        name: "FK_PlanModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanModules_SubscriptionPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_BaseUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_SubscriptionPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BaseUsers",
                columns: new[] { "Id", "Address", "AdminRole", "CreatedAt", "Department", "Email", "FirstName", "LastName", "Password", "PhoneNumber", "UserType" },
                values: new object[,]
                {
                    { 1001, "Maslak Mah. IT Plaza No:12, Istanbul", "SuperAdmin", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Platform Operations", "aylin.demir@qutech.com", "Aylin", "Demir", "Admin@123", "+90-212-555-0101", "Admin" },
                    { 1002, "Levent Mah. Teknoloji Cad. No:8, Istanbul", "SupportAdmin", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer Success", "mert.kara@qutech.com", "Mert", "Kara", "Support@123", "+90-212-555-0102", "Admin" },
                    { 1003, "Cankaya Mah. Is Merkezi No:4, Ankara", "SecurityAdmin", new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Information Security", "selin.yilmaz@qutech.com", "Selin", "Yilmaz", "Security@123", "+90-312-555-0103", "Admin" },
                    { 1004, "Alsancak Mah. Inovasyon Sok. No:19, Izmir", "BillingAdmin", new DateTime(2024, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finance Operations", "can.aydin@qutech.com", "Can", "Aydin", "Billing@123", "+90-232-555-0104", "Admin" },
                    { 1005, "Atasehir Mah. Startup Cad. No:22, Istanbul", "ProductAdmin", new DateTime(2024, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product Management", "ece.sahin@qutech.com", "Ece", "Sahin", "Product@123", "+90-216-555-0105", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "BaseUsers",
                columns: new[] { "Id", "Address", "CompanyName", "CreatedAt", "Email", "FirstName", "LastName", "PhoneNumber", "TaxNumber", "UserType" },
                values: new object[,]
                {
                    { 2001, "Beylikduzu OSB 2. Cad. No:45, Istanbul", "Nova Foods A.S.", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "burak.celik@novafoods.com", "Burak", "Celik", "+90-212-555-1101", "TR3456789012", "Customer" },
                    { 2002, "Nilufer Sanayi Bolgesi No:77, Bursa", "MetalLink Manufacturing Ltd.", new DateTime(2024, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "zeynep.arslan@metallink.com", "Zeynep", "Arslan", "+90-224-555-1102", "TR4567890123", "Customer" },
                    { 2003, "ODTU Teknokent A Blok No:9, Ankara", "MediSync Health Tech", new DateTime(2024, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "kerem.toprak@medisync.io", "Kerem", "Toprak", "+90-312-555-1103", "TR5678901234", "Customer" },
                    { 2004, "Kemalpasa Lojistik Merkezi No:31, Izmir", "LogiRoad Logistics", new DateTime(2024, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "elif.tas@logiroad.com", "Elif", "Tas", "+90-232-555-1104", "TR6789012345", "Customer" },
                    { 2005, "Kadikoy Finans Merkezi No:14, Istanbul", "FinPeak Advisory", new DateTime(2024, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "onur.gunes@finpeak.co", "Onur", "Gunes", "+90-216-555-1105", "TR7890123456", "Customer" }
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "Description", "ModuleName" },
                values: new object[,]
                {
                    { 1, "Automates recurring invoices, payment reminders, and ledger exports.", "Billing Automation" },
                    { 2, "Tracks leads, deal stages, and customer lifecycle activities.", "CRM Pipeline" },
                    { 3, "Designs approval flows and task routing for internal processes.", "Workflow Engine" },
                    { 4, "Provides KPI dashboards, cohort analysis, and custom reporting.", "Analytics Hub" },
                    { 5, "Manages role-based permissions, SSO integration, and audit visibility.", "Access Control" }
                });

            migrationBuilder.InsertData(
                table: "SubscriptionPlans",
                columns: new[] { "Id", "IsActive", "MaxUsers", "MonthlyPrice", "PlanName" },
                values: new object[,]
                {
                    { 1, true, 5, 29m, "Starter" },
                    { 2, true, 20, 79m, "Growth" },
                    { 3, true, 50, 149m, "Professional" },
                    { 4, true, 150, 299m, "Scale" },
                    { 5, true, 500, 599m, "Enterprise" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_UserId",
                table: "AuditTrails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanModules_ModuleId",
                table: "PlanModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_CustomerId",
                table: "Subscriptions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PlanId",
                table: "Subscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UsageRecords_CustomerId",
                table: "UsageRecords",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountSetting");

            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "PlanModules");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "UsageRecords");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "SubscriptionPlans");

            migrationBuilder.DropTable(
                name: "BaseUsers");
        }
    }
}
