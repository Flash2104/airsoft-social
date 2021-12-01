using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirSoft.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Users",
                columns: new[] { "Id", "AddedDate", "CreatedBy", "Email", "ModifiedBy", "ModifiedDate", "PasswordHash", "Phone" },
                values: new object[] { new Guid("fadde9ec-7dc4-4033-b1e6-2f83a08c70f3"), new DateTime(2021, 12, 2, 1, 50, 0, 0, DateTimeKind.Unspecified), new Guid("fadde9ec-7dc4-4033-b1e6-2f83a08c70f3"), "khoruzhenko.work@gmail.com", new Guid("fadde9ec-7dc4-4033-b1e6-2f83a08c70f3"), new DateTime(2021, 12, 2, 1, 50, 0, 0, DateTimeKind.Unspecified), "AQAAAAEAACcQAAAAEMQnvSxDqgyc+KNNzIFjcuST/qZGfHVSLT9P+Z3revJP2Q9Tctz8PIeDxj2k7aJkLg==", "89266762453" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");
        }
    }
}
