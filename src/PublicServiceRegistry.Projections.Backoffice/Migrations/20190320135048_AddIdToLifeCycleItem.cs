using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class AddIdToLifeCycleItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList");
        }
    }
}
