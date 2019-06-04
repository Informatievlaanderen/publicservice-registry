using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class AddIpdcCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpdcCode",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpdcCode",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList");
        }
    }
}
