using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class AddRemovedToPublicServiceList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Removed",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_PublicServiceList_Removed",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList",
                column: "Removed");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PublicServiceList_Removed",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList");

            migrationBuilder.DropColumn(
                name: "Removed",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList");
        }
    }
}
