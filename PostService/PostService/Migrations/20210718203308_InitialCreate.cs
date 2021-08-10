using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PostService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostHistories",
                columns: table => new
                {
                    PostHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<double>(type: "float", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostHistories", x => x.PostHistoryId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    PostImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.PostId);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    PostImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.PostId);
                });

            migrationBuilder.InsertData(
                table: "PostHistories",
                columns: new[] { "PostHistoryId", "DateFrom", "DateTo", "PostId", "Price" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 5, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 10, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b43b4683-7d9d-413f-a6e1-f821a80c8ce4"), 2499.0 },
                    { 2, new DateTime(2020, 10, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("b43b4683-7d9d-413f-a6e1-f821a80c8ce4"), 2590.0 },
                    { 3, new DateTime(2021, 6, 11, 11, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("8ccb1467-9f38-4164-88da-15882fe82e58"), 560.0 },
                    { 4, new DateTime(2021, 4, 17, 8, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("23d2cce9-86d7-4bff-887e-f7712b16766d"), 14690.0 },
                    { 5, new DateTime(2021, 6, 12, 12, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("5284a73f-1f9e-4799-a793-5a4fe4a1df56"), 2500.0 },
                    { 6, new DateTime(2021, 7, 7, 12, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("54f9baf6-271e-40cb-8d80-a27980fc8b63"), 450.0 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "PostId", "AccountId", "Category", "Currency", "Description", "PostImage", "PostName", "Price", "PublicationDate", "Weight" },
                values: new object[,]
                {
                    { new Guid("b43b4683-7d9d-413f-a6e1-f821a80c8ce4"), new Guid("59ed7d80-39c9-42b8-a822-70ddd295914a"), "Sportska odeća", "Rsd", "Adidas Essentials T-Shirt je muška majica koja je pogodna za mnoge sportske i zabavne aktivnosti. Napravljena je od prijatne mešavine prirodnih i veštačkih materijala.", "https://www.intersport.rs/media/catalog/product/cache/382907d7f48ae2519bf16cd5f39b77f9/g/k/gk9635_app_photo_front-center_white.jpg", "Adidas sportska majica", 2590.0, new DateTime(2020, 5, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "130g" },
                    { new Guid("8ccb1467-9f38-4164-88da-15882fe82e58"), new Guid("42b70088-9dbd-4b19-8fc7-16414e94a8a6"), "Mobilni telefoni", "Euro", "Nov, neotpakovan Iphone 11 64GB. Garancija godinu dana.", "https://www.tehnomedia.rs/image/71354.jpg?tip=huge&tip_slike=0", "Iphone 11", 560.0, new DateTime(2021, 6, 11, 11, 0, 0, 0, DateTimeKind.Unspecified), "194g" },
                    { new Guid("23d2cce9-86d7-4bff-887e-f7712b16766d"), new Guid("42b70088-9dbd-4b19-8fc7-16414e94a8a6"), "Patike za trčanje", "Rsd", "Adidas adizero Boston 9 M su muške patike za trčanje koje su namenjene za trčanje po urbanim površinama. Stvorene su za vrhunske trkače. Đon je napravljen od Boost pene, a Continental guma pruža odlično prijanjanje.", "https://www.sportvision.rs/files/thumbs/files/images/slike_proizvoda/media/FY0/FY0343/images/thumbs_600/FY0343_600_600px.jpg", "Adidas Adizero Boston 9", 14690.0, new DateTime(2021, 4, 17, 8, 0, 0, 0, DateTimeKind.Unspecified), "238g" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "PostId", "AccountId", "Category", "Currency", "Description", "PostImage", "PostName", "Price", "PublicationDate" },
                values: new object[,]
                {
                    { new Guid("5284a73f-1f9e-4799-a793-5a4fe4a1df56"), new Guid("f2f88bcd-d0a2-4fe7-a23f-df97a59731cd"), "Lepota i zdravlje", "Rsd", "Zablistajte uz najnoviju šminku vrhunskog kvaliteta.", "", "Šminkanje", 2500.0, new DateTime(2021, 6, 12, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("54f9baf6-271e-40cb-8d80-a27980fc8b63"), new Guid("59ed7d80-39c9-42b8-a822-70ddd295914a"), "Muški frizer", "Rsd", "Zakaži svoj termin i dobij frizuru baš po tvojoj želji", "", "Muško šišanje", 450.0, new DateTime(2021, 7, 7, 12, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostHistories");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
