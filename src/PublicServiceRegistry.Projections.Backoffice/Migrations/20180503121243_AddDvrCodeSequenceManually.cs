using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    using Infrastructure;

    public partial class AddDvrCodeSequenceManually : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
  CREATE SEQUENCE {Schema.Backoffice}.{BackofficeContext.DvrCodeSequenceName}
    AS int
    START WITH 1
    INCREMENT BY 1
	MINVALUE 1
    MAXVALUE 999999999
    CYCLE
    CACHE 3
;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
  DROP SEQUENCE {Schema.Backoffice}.{BackofficeContext.DvrCodeSequenceName}
;");
        }
    }
}
