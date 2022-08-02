using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tiki_shop.Migrations
{
    public partial class RegenDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "ffc8cc5f-4a9b-447f-a367-04fe39d2d8ec");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Balance", "Commission", "Email", "Fullname", "Password", "PhoneNumber", "RoleId", "Status" },
                values: new object[] { "6d8d22c4-4e74-415b-bde5-c67d5934ae8e", "Hà Nội", 0f, 0f, "admin@email.com", "Admin nè", "$2a$10$MEhfF4w8ga3GGcJrMW7iWu6RG0A1kUqC0FM0R9BbJjSd3yKxgBLM2", "0123456789", 1, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "6d8d22c4-4e74-415b-bde5-c67d5934ae8e");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Balance", "Commission", "Email", "Fullname", "Password", "PhoneNumber", "RoleId", "Status" },
                values: new object[] { "ffc8cc5f-4a9b-447f-a367-04fe39d2d8ec", "Hà Nội", 0f, 0f, "admin@email.com", "Admin nè", "$2a$10$MEhfF4w8ga3GGcJrMW7iWu6RG0A1kUqC0FM0R9BbJjSd3yKxgBLM2", "0123456789", 1, true });
        }
    }
}
