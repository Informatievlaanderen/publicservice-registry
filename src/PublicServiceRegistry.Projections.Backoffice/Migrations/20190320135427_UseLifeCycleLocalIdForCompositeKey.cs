using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class UseLifeCycleLocalIdForCompositeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PublicServiceLifeCycleList",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                newName: "LocalId");

            migrationBuilder.AlterColumn<string>(
                name: "LifeCycleStage",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublicServiceLifeCycleList",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                columns: new[] { "PublicServiceId", "LocalId" })
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PublicServiceLifeCycleList",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList");

            migrationBuilder.RenameColumn(
                name: "LocalId",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "LifeCycleStage",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublicServiceLifeCycleList",
                schema: "PublicServiceRegistryBackoffice",
                table: "PublicServiceLifeCycleList",
                columns: new[] { "PublicServiceId", "LifeCycleStage" })
                .Annotation("SqlServer:Clustered", true);
        }
    }
}
