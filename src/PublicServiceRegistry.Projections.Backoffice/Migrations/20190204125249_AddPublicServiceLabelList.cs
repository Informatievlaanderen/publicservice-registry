using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    public partial class AddPublicServiceLabelList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PublicServiceLabelList",
                schema: "PublicServiceRegistryBackoffice",
                columns: table => new
                {
                    PublicServiceId = table.Column<string>(nullable: false),
                    LabelType = table.Column<string>(nullable: false),
                    LabelValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicServiceLabelList", x => new { x.PublicServiceId, x.LabelType })
                        .Annotation("SqlServer:Clustered", true);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicServiceLabelList",
                schema: "PublicServiceRegistryBackoffice");
        }
    }
}
