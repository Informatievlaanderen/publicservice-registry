using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class AddCompetentAuthorityName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompetentAuthorityName",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompetentAuthorityName",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList");
        }
    }
}
