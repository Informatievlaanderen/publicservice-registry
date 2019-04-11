using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class UseIntsForLocalDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentLifeCycleStageEndsAt",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList");

            migrationBuilder.AddColumn<int>(
                name: "CurrentLifeCycleStageEndsAt",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "To",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList");

            migrationBuilder.AddColumn<int>(
                name: "To",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "From",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList");

            migrationBuilder.AddColumn<int>(
                name: "From",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentLifeCycleStageEndsAt",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList");

            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentLifeCycleStageEndsAt",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "From",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList");

            migrationBuilder.AddColumn<DateTime>(
                name: "To",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "From",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList");

            migrationBuilder.AddColumn<DateTime>(
                name: "From",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                nullable: true);
        }
    }
}
