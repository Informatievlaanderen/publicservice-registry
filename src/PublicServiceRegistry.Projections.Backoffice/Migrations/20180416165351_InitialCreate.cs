using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "PublicServiceRegistryBackoffice");

            migrationBuilder.EnsureSchema(
                name: "PublicServiceRegistry");

            migrationBuilder.CreateTable(
                name: "PublicServiceIdGenerator",
                schema: "PublicServiceRegistry",
                columns: table => new
                {
                    PublicServiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicServiceIdGenerator", x => x.PublicServiceId)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "ProjectionStates",
                schema: "PublicServiceRegistryBackoffice",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Position = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectionStates", x => x.Name)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "PublicServiceList",
                schema: "PublicServiceRegistryBackoffice",
                columns: table => new
                {
                    PublicServiceId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ResponsibleOrganisationCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicServiceList", x => x.PublicServiceId)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicServiceList_Name",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicServiceIdGenerator",
                schema: "PublicServiceRegistry");

            migrationBuilder.DropTable(
                name: "ProjectionStates",
                schema: "PublicServiceRegistryBackoffice");

            migrationBuilder.DropTable(
                name: "PublicServiceList",
                schema: "PublicServiceRegistryBackoffice");
        }
    }
}
