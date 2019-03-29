using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class UseDateTimeForFromAndTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "To",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "From",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "To",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "From",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
