using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmreGeydirenler_Lab2.Migrations
{
    /// <inheritdoc />
    public partial class CompleteRemainingLab3Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountSetting_BaseUsers_CustomerId",
                table: "AccountSetting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountSetting",
                table: "AccountSetting");

            migrationBuilder.RenameTable(
                name: "AccountSetting",
                newName: "AccountSettings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountSettings",
                table: "AccountSettings",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountSettings_BaseUsers_CustomerId",
                table: "AccountSettings",
                column: "CustomerId",
                principalTable: "BaseUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountSettings_BaseUsers_CustomerId",
                table: "AccountSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountSettings",
                table: "AccountSettings");

            migrationBuilder.RenameTable(
                name: "AccountSettings",
                newName: "AccountSetting");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountSetting",
                table: "AccountSetting",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountSetting_BaseUsers_CustomerId",
                table: "AccountSetting",
                column: "CustomerId",
                principalTable: "BaseUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
