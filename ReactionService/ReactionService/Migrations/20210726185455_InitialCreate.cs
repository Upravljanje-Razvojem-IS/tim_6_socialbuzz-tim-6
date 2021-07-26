using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReactionService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    ReactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReactionTypeId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.ReactionId);
                });

            migrationBuilder.CreateTable(
                name: "ReactionTypes",
                columns: table => new
                {
                    ReactionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionTypes", x => x.ReactionTypeId);
                });

            migrationBuilder.InsertData(
                table: "ReactionTypes",
                columns: new[] { "ReactionTypeId", "TypeName" },
                values: new object[,]
                {
                    { 1, "Like" },
                    { 2, "Heart" },
                    { 3, "Smiley" }
                });

            migrationBuilder.InsertData(
                table: "Reactions",
                columns: new[] { "ReactionId", "AccountId", "PostId", "ReactionTypeId" },
                values: new object[,]
                {
                    { new Guid("3b6d0a06-e64b-4f42-8689-fc10e8e6edf7"), new Guid("42b70088-9dbd-4b19-8fc7-16414e94a8a6"), new Guid("15908a81-dcae-43e7-fecb-08d94eb2a3fe"), 1 },
                    { new Guid("d8fe100b-8d5f-4027-961a-fa75bf8a3b94"), new Guid("59ed7d80-39c9-42b8-a822-70ddd295914a"), new Guid("8ccb1467-9f38-4164-88da-15882fe82e58"), 2 },
                    { new Guid("19e0acbf-5707-49ee-8cb6-134c00b7c10b"), new Guid("f2f88bcd-d0a2-4fe7-a23f-df97a59731cd"), new Guid("23d2cce9-86d7-4bff-887e-f7712b16766d"), 3 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "ReactionTypes");
        }
    }
}
