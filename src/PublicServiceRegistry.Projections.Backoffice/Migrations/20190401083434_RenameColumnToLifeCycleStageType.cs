using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class RenameColumnToLifeCycleStageType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LifeCycleStage",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                newName: "LifeCycleStageType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LifeCycleStageType",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                newName: "LifeCycleStage");
        }
    }
}
