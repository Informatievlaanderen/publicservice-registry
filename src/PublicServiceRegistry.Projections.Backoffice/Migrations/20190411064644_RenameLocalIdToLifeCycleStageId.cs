using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class RenameLocalIdToLifeCycleStageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocalId",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                newName: "LifeCycleStageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LifeCycleStageId",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                newName: "LocalId");
        }
    }
}
