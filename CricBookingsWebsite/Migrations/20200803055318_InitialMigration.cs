using Microsoft.EntityFrameworkCore.Migrations;

namespace CricBookingsWebsite.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestUsers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TestUsers",
                columns: new[] { "Id", "EmailAddress", "FirstName", "LastName", "MiddleName", "Password", "PhoneNumber" },
                values: new object[] { 40, "bobby@bob.com", "Bobby", "Bob", null, "abcd", "1234" });

            migrationBuilder.InsertData(
                table: "TestUsers",
                columns: new[] { "Id", "EmailAddress", "FirstName", "LastName", "MiddleName", "Password", "PhoneNumber" },
                values: new object[] { 45, "babu@khan.com", "Babu", "Khan", null, "efgh", "5678" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestUsers");
        }
    }
}
