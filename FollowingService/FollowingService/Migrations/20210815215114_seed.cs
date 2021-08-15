using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FollowingService.Migrations
{
    public partial class seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Followings",
                table: "Followings");

            migrationBuilder.RenameTable(
                name: "Followings",
                newName: "Follows");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Follows",
                table: "Follows",
                columns: new[] { "FollowerId", "FollowingId" });

            migrationBuilder.InsertData(
                table: "Follows",
                columns: new[] { "FollowerId", "FollowingId" },
                values: new object[] { new Guid("eb78bf24-a3ab-48bf-a931-7b0c6e680bfd"), new Guid("34a81ef8-2831-4444-8355-859d02ae2290") });

            migrationBuilder.InsertData(
                table: "Follows",
                columns: new[] { "FollowerId", "FollowingId" },
                values: new object[] { new Guid("34a81ef8-2831-4444-8355-859d02ae2290"), new Guid("eb78bf24-a3ab-48bf-a931-7b0c6e680bfd") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Follows",
                table: "Follows");

            migrationBuilder.DeleteData(
                table: "Follows",
                keyColumns: new[] { "FollowerId", "FollowingId" },
                keyValues: new object[] { new Guid("34a81ef8-2831-4444-8355-859d02ae2290"), new Guid("eb78bf24-a3ab-48bf-a931-7b0c6e680bfd") });

            migrationBuilder.DeleteData(
                table: "Follows",
                keyColumns: new[] { "FollowerId", "FollowingId" },
                keyValues: new object[] { new Guid("eb78bf24-a3ab-48bf-a931-7b0c6e680bfd"), new Guid("34a81ef8-2831-4444-8355-859d02ae2290") });

            migrationBuilder.RenameTable(
                name: "Follows",
                newName: "Followings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Followings",
                table: "Followings",
                columns: new[] { "FollowerId", "FollowingId" });
        }
    }
}
