using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmreGeydirenler_Lab2.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "BaseUsers",
                type: "TEXT",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1001,
                column: "Password",
                value: "Admin@123");

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1002,
                column: "Password",
                value: "Support@123");

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1003,
                column: "Password",
                value: "Security@123");

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1004,
                column: "Password",
                value: "Billing@123");

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 1005,
                column: "Password",
                value: "Product@123");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "BaseUsers",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2001,
                column: "Password",
                value: null);

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2002,
                column: "Password",
                value: null);

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2003,
                column: "Password",
                value: null);

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2004,
                column: "Password",
                value: null);

            migrationBuilder.UpdateData(
                table: "BaseUsers",
                keyColumn: "Id",
                keyValue: 2005,
                column: "Password",
                value: null);
        }
    }
}
