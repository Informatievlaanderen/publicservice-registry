using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class DeletePublicServiceIdList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicServiceIdGenerator",
                schema: "PublicServiceRegistry");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
