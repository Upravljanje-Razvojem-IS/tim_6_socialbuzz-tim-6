using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockingService.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    BlockerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlockedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => new { x.BlockerId, x.BlockedId });
                });

            migrationBuilder.InsertData(
                table: "Blocks",
                columns: new[] { "BlockedId", "BlockerId" },
                values: new object[] { new Guid("34a81ef8-2831-4444-8355-859d02ae2290"), new Guid("eb78bf24-a3ab-48bf-a931-7b0c6e680bfd") });

            migrationBuilder.InsertData(
                table: "Blocks",
                columns: new[] { "BlockedId", "BlockerId" },
                values: new object[] { new Guid("eb78bf24-a3ab-48bf-a931-7b0c6e680bfd"), new Guid("34a81ef8-2831-4444-8355-859d02ae2290") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blocks");
        }
    }
}
