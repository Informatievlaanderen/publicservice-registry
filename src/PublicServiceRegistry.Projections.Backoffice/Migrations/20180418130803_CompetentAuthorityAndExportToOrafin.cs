using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class CompetentAuthorityAndExportToOrafin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ExportToOrafin",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExportToOrafin",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList");
        }
    }
}
