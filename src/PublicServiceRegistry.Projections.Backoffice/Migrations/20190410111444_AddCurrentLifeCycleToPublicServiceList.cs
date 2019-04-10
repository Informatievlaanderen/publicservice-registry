using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class AddCurrentLifeCycleToPublicServiceList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentLifeCycleStageEndsAt",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentLifeCycleStageId",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentLifeCycleStageType",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LifeCycleStagesForPublicServiceList",
                schema: "PublicServiceRegistryBackoffice",
                columns: table => new
                {
                    PublicServiceId = table.Column<string>(nullable: false),
                    LifeCycleStageId = table.Column<int>(nullable: false),
                    LifeCycleStageType = table.Column<string>(nullable: true),
                    From = table.Column<int>(nullable: true),
                    To = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifeCycleStagesForPublicServiceList", x => new { x.PublicServiceId, x.LifeCycleStageId })
                        .Annotation("SqlServer:Clustered", true);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LifeCycleStagesForPublicServiceList",
                schema: "PublicServiceRegistryBackoffice");

            migrationBuilder.DropColumn(
                name: "CurrentLifeCycleStageEndsAt",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList");

            migrationBuilder.DropColumn(
                name: "CurrentLifeCycleStageId",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList");

            migrationBuilder.DropColumn(
                name: "CurrentLifeCycleStageType",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList");
        }
    }
}
