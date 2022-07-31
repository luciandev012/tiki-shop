using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tiki_shop.Migrations
{
    public partial class AddFullNameField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "dc16c911-6047-4d6d-a0dd-1676b9668071");

            migrationBuilder.AddColumn<string>(
                name: "Fullname",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Balance", "Commission", "Email", "Fullname", "Password", "PhoneNumber", "RoleId", "Status" },
                values: new object[] { "3c05f411-5e5c-40db-84fb-adc88a6baf3a", "Hà Nội", 0f, 0f, "admin@email.com", "Admin nè", "$2a$10$MEhfF4w8ga3GGcJrMW7iWu6RG0A1kUqC0FM0R9BbJjSd3yKxgBLM2", "0123456789", 1, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3c05f411-5e5c-40db-84fb-adc88a6baf3a");

            migrationBuilder.DropColumn(
                name: "Fullname",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Balance", "Commission", "Email", "Password", "PhoneNumber", "RoleId", "Status" },
                values: new object[] { "dc16c911-6047-4d6d-a0dd-1676b9668071", "Hà Nội", 0f, 0f, "admin@email.com", "$2a$10$MEhfF4w8ga3GGcJrMW7iWu6RG0A1kUqC0FM0R9BbJjSd3yKxgBLM2", "0123456789", 1, true });
        }
    }
}
