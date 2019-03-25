using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class CompetentAuthorityCodeRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResponsibleOrganisationCode",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList",
                newName: "CompetentAuthorityCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompetentAuthorityCode",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceList",
                newName: "ResponsibleOrganisationCode");
        }
    }
}
