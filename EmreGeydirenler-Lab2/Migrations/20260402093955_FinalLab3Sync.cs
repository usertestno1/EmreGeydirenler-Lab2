using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmreGeydirenler_Lab2.Migrations
{
    /// <inheritdoc />
    public partial class FinalLab3Sync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Customer_Password",
                table: "BaseUsers",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.InsertData(
                table: "AccountSettings",
                columns: new[] { "CustomerId", "ReceiveEmailAlerts", "TwoFactorEnabled" },
                values: new object[,]
                {
                    { 2001, true, true },
                    { 2002, true, false },
                    { 2003, false, true },
                    { 2004, true, false },
                    { 2005, true, true }
                });

            migrationBuilder.InsertData(
                table: "AuditTrails",
                columns: new[] { "Id", "ActionDescription", "IPAddress", "Timestamp", "UserId" },
                values: new object[,]
                {
                    { 6001, "Created the Growth plan campaign settings.", "10.10.1.11", new DateTime(2026, 3, 10, 9, 15, 0, 0, DateTimeKind.Unspecified), 1001 },
                    { 6002, "Reviewed invoice disputes for enterprise clients.", "10.10.1.12", new DateTime(2026, 3, 12, 11, 45, 0, 0, DateTimeKind.Unspecified), 1002 },
                    { 6003, "Updated security rules for admin sign-in.", "10.10.1.13", new DateTime(2026, 3, 14, 14, 5, 0, 0, DateTimeKind.Unspecified), 1003 },
                    { 6004, "Approved plan pricing adjustment request.", "10.10.1.14", new DateTime(2026, 3, 16, 16, 30, 0, 0, DateTimeKind.Unspecified), 1004 },
                    { 6005, "Published monthly usage analytics report.", "10.10.1.15", new DateTime(2026, 3, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), 1005 }
                });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1001,
                columns: new[] { "Address", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { "Level 12, 200 George Street, Sydney NSW 2000", "olivia.bennett@qutech.com", "Olivia", "Bennett", "+61 2 9001 1001" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1002,
                columns: new[] { "Address", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { "85 Collins Street, Melbourne VIC 3000", "liam.carter@qutech.com", "Liam", "Carter", "+61 3 9001 1002" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1003,
                columns: new[] { "Address", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { "40 Adelaide Street, Brisbane QLD 4000", "charlotte.nguyen@qutech.com", "Charlotte", "Nguyen", "+61 7 9001 1003" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1004,
                columns: new[] { "Address", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { "27 St Georges Terrace, Perth WA 6000", "noah.reed@qutech.com", "Noah", "Reed", "+61 8 9001 1004" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1005,
                columns: new[] { "Address", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { "15 Barangaroo Avenue, Sydney NSW 2000", "amelia.foster@qutech.com", "Amelia", "Foster", "+61 2 9001 1005" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2001,
                columns: new[] { "Address", "CompanyName", "Email", "FirstName", "LastName", "Customer_Password", "PhoneNumber", "TaxNumber" },
                values: new object[] { "48 Darling Drive, Pyrmont NSW 2009", "Harbor Foods Pty Ltd", "ethan.walker@harborfoods.com.au", "Ethan", "Walker", "Customer@123", "+61 2 8012 1101", "ABN 34 567 890 123" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2002,
                columns: new[] { "Address", "CompanyName", "Email", "FirstName", "LastName", "Customer_Password", "PhoneNumber", "TaxNumber" },
                values: new object[] { "190 Flinders Street, Melbourne VIC 3000", "Southern Steel Manufacturing Pty Ltd", "sophie.mitchell@southernsteel.com.au", "Sophie", "Mitchell", "Customer@234", "+61 3 8012 1102", "ABN 45 678 901 234" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2003,
                columns: new[] { "Address", "CompanyName", "Email", "FirstName", "LastName", "Customer_Password", "PhoneNumber", "TaxNumber" },
                values: new object[] { "22 Eagle Street, Brisbane QLD 4000", "MediTrack Health Systems Pty Ltd", "jack.thompson@meditrackhealth.com.au", "Jack", "Thompson", "Customer@345", "+61 7 8012 1103", "ABN 56 789 012 345" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2004,
                columns: new[] { "Address", "CompanyName", "Email", "FirstName", "LastName", "Customer_Password", "PhoneNumber", "TaxNumber" },
                values: new object[] { "90 Terrace Road, East Perth WA 6004", "Pacific Freight Logistics Pty Ltd", "grace.evans@pacificfreight.com.au", "Grace", "Evans", "Customer@456", "+61 8 8012 1104", "ABN 67 890 123 456" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2005,
                columns: new[] { "Address", "CompanyName", "Email", "FirstName", "LastName", "Customer_Password", "PhoneNumber", "TaxNumber" },
                values: new object[] { "101 Pitt Street, Sydney NSW 2000", "Austral Advisory Group Pty Ltd", "mason.parker@australadvisory.com.au", "Mason", "Parker", "Customer@567", "+61 2 8012 1105", "ABN 78 901 234 567" });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "Id", "CustomerId", "DueDate", "IsPaid", "IssueDate", "TotalAmount" },
                values: new object[,]
                {
                    { 4001, 2001, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 79m },
                    { 4002, 2002, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 149m },
                    { 4003, 2003, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 299m },
                    { 4004, 2004, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 29m },
                    { 4005, 2005, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 599m }
                });

            migrationBuilder.InsertData(
                table: "PlanModules",
                columns: new[] { "ModuleId", "PlanId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 5, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 5, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 3, 3 },
                    { 5, 3 },
                    { 1, 4 },
                    { 2, 4 },
                    { 3, 4 },
                    { 4, 4 },
                    { 5, 4 },
                    { 1, 5 },
                    { 2, 5 },
                    { 3, 5 },
                    { 4, 5 },
                    { 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "Id", "CustomerId", "EndDate", "PlanId", "StartDate", "Status" },
                values: new object[,]
                {
                    { 3001, 2001, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { 3002, 2002, new DateTime(2027, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { 3003, 2003, new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { 3004, 2004, new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { 3005, 2005, new DateTime(2027, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" }
                });

            migrationBuilder.InsertData(
                table: "UsageRecords",
                columns: new[] { "Id", "ApiRequestsCount", "CustomerId", "LogDate", "StorageUsedMb" },
                values: new object[,]
                {
                    { 5001, 3420, 2001, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 15360m },
                    { 5002, 8750, 2002, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 28160m },
                    { 5003, 15120, 2003, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 65280m },
                    { 5004, 950, 2004, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3840m },
                    { 5005, 23890, 2005, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 121000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountSettings",
                keyColumn: "CustomerId",
                keyValue: 2001);

            migrationBuilder.DeleteData(
                table: "AccountSettings",
                keyColumn: "CustomerId",
                keyValue: 2002);

            migrationBuilder.DeleteData(
                table: "AccountSettings",
                keyColumn: "CustomerId",
                keyValue: 2003);

            migrationBuilder.DeleteData(
                table: "AccountSettings",
                keyColumn: "CustomerId",
                keyValue: 2004);

            migrationBuilder.DeleteData(
                table: "AccountSettings",
                keyColumn: "CustomerId",
                keyValue: 2005);

            migrationBuilder.DeleteData(
                table: "AuditTrails",
                keyColumn: "Id",
                keyValue: 6001);

            migrationBuilder.DeleteData(
                table: "AuditTrails",
                keyColumn: "Id",
                keyValue: 6002);

            migrationBuilder.DeleteData(
                table: "AuditTrails",
                keyColumn: "Id",
                keyValue: 6003);

            migrationBuilder.DeleteData(
                table: "AuditTrails",
                keyColumn: "Id",
                keyValue: 6004);

            migrationBuilder.DeleteData(
                table: "AuditTrails",
                keyColumn: "Id",
                keyValue: 6005);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 4001);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 4002);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 4003);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 4004);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 4005);

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 4, 5 });

            migrationBuilder.DeleteData(
                table: "PlanModules",
                keyColumns: new[] { "ModuleId", "PlanId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "Id",
                keyValue: 3001);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "Id",
                keyValue: 3002);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "Id",
                keyValue: 3003);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "Id",
                keyValue: 3004);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "Id",
                keyValue: 3005);

            migrationBuilder.DeleteData(
                table: "UsageRecords",
                keyColumn: "Id",
                keyValue: 5001);

            migrationBuilder.DeleteData(
                table: "UsageRecords",
                keyColumn: "Id",
                keyValue: 5002);

            migrationBuilder.DeleteData(
                table: "UsageRecords",
                keyColumn: "Id",
                keyValue: 5003);

            migrationBuilder.DeleteData(
                table: "UsageRecords",
                keyColumn: "Id",
                keyValue: 5004);

            migrationBuilder.DeleteData(
                table: "UsageRecords",
                keyColumn: "Id",
                keyValue: 5005);

            migrationBuilder.DropColumn(
                name: "Customer_Password",
                table: "BaseUsers");

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1001,
                columns: new[] { "Address", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { "Maslak Mah. IT Plaza No:12, Istanbul", "aylin.demir@qutech.com", "Aylin", "Demir", "+90-212-555-0101" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1002,
                columns: new[] { "Address", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { "Levent Mah. Teknoloji Cad. No:8, Istanbul", "mert.kara@qutech.com", "Mert", "Kara", "+90-212-555-0102" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1003,
                columns: new[] { "Address", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { "Cankaya Mah. Is Merkezi No:4, Ankara", "selin.yilmaz@qutech.com", "Selin", "Yilmaz", "+90-312-555-0103" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1004,
                columns: new[] { "Address", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { "Alsancak Mah. Inovasyon Sok. No:19, Izmir", "can.aydin@qutech.com", "Can", "Aydin", "+90-232-555-0104" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1005,
                columns: new[] { "Address", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { "Atasehir Mah. Startup Cad. No:22, Istanbul", "ece.sahin@qutech.com", "Ece", "Sahin", "+90-216-555-0105" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2001,
                columns: new[] { "Address", "CompanyName", "Email", "FirstName", "LastName", "PhoneNumber", "TaxNumber" },
                values: new object[] { "Beylikduzu OSB 2. Cad. No:45, Istanbul", "Nova Foods A.S.", "burak.celik@novafoods.com", "Burak", "Celik", "+90-212-555-1101", "TR3456789012" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2002,
                columns: new[] { "Address", "CompanyName", "Email", "FirstName", "LastName", "PhoneNumber", "TaxNumber" },
                values: new object[] { "Nilufer Sanayi Bolgesi No:77, Bursa", "MetalLink Manufacturing Ltd.", "zeynep.arslan@metallink.com", "Zeynep", "Arslan", "+90-224-555-1102", "TR4567890123" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2003,
                columns: new[] { "Address", "CompanyName", "Email", "FirstName", "LastName", "PhoneNumber", "TaxNumber" },
                values: new object[] { "ODTU Teknokent A Blok No:9, Ankara", "MediSync Health Tech", "kerem.toprak@medisync.io", "Kerem", "Toprak", "+90-312-555-1103", "TR5678901234" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2004,
                columns: new[] { "Address", "CompanyName", "Email", "FirstName", "LastName", "PhoneNumber", "TaxNumber" },
                values: new object[] { "Kemalpasa Lojistik Merkezi No:31, Izmir", "LogiRoad Logistics", "elif.tas@logiroad.com", "Elif", "Tas", "+90-232-555-1104", "TR6789012345" });

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2005,
                columns: new[] { "Address", "CompanyName", "Email", "FirstName", "LastName", "PhoneNumber", "TaxNumber" },
                values: new object[] { "Kadikoy Finans Merkezi No:14, Istanbul", "FinPeak Advisory", "onur.gunes@finpeak.co", "Onur", "Gunes", "+90-216-555-1105", "TR7890123456" });
        }
    }
}
