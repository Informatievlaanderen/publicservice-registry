using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class AddLifeCycleProjection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PublicServiceLifeCycleList",
                schema: "PublicServiceRegistryBackoffice",
                columns: table => new
                {
                    PublicServiceId = table.Column<string>(nullable: false),
                    LifeCycleStage = table.Column<string>(nullable: false),
                    From = table.Column<DateTimeOffset>(nullable: true),
                    To = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicServiceLifeCycleList", x => new { x.PublicServiceId, x.LifeCycleStage })
                        .Annotation("SqlServer:Clustered", true);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicServiceLifeCycleList",
                schema: "PublicServiceRegistryBackoffice");
        }
    }
}
