using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class AddLegislativeDocumentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LegislativeDocumentId",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegislativeDocumentId",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList");
        }
    }
}
