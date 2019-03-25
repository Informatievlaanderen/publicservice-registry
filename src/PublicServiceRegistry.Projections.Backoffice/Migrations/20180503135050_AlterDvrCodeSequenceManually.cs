using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    using Infrastructure;

    public partial class AlterDvrCodeSequenceManually : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
    ALTER SEQUENCE {Schema.Backoffice}.{BackofficeContext.DvrCodeSequenceName}
    RESTART WITH 1
    NO CYCLE
    NO CACHE
;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
    ALTER SEQUENCE {Schema.Backoffice}.{BackofficeContext.DvrCodeSequenceName}
    CYCLE
    CACHE 3
;");
        }
    }
}
