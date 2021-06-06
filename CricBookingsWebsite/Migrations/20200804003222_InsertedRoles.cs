using Microsoft.EntityFrameworkCore.Migrations;

namespace CricBookingsWebsite.Migrations
{
    public partial class InsertedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "70e41991-480a-4085-8fae-e3f56e47e9a2", "1aced537-7606-4308-8310-a99505b1474f", "Visitor", "VISITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "af3eb8d5-8342-4b64-9fec-3edc80c3d61f", "567cf922-68e3-4cf5-9bee-7972e37df74e", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "70e41991-480a-4085-8fae-e3f56e47e9a2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af3eb8d5-8342-4b64-9fec-3edc80c3d61f");
        }
    }
}
