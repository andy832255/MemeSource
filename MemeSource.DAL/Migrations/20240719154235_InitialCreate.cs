using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MemeSource.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CATE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CREATED = table.Column<DateTime>(type: "datetime", nullable: true),
                    UPDATED = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CATE_BINDING",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IMAGE_ID = table.Column<long>(type: "bigint", nullable: true),
                    CATE_ID = table.Column<long>(type: "bigint", nullable: true),
                    IS_BOUND = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATE_BINDING", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IMAGE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DATA = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    SIZE = table.Column<int>(type: "int", nullable: true),
                    TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CREATED = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UPDATED = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMAGE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SystemProperty",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SP_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parameter1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parameter2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parameter3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parameter4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemProperty", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TAG",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CREATED = table.Column<DateTime>(type: "datetime", nullable: true),
                    UPDATED = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TAG_BINDING",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IMAGE_ID = table.Column<long>(type: "bigint", nullable: true),
                    TAG_ID = table.Column<long>(type: "bigint", nullable: true),
                    IS_BOUND = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAG_BINDING", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "SystemProperty",
                columns: new[] { "ID", "CreatedDate", "Parameter1", "Parameter2", "Parameter3", "Parameter4", "SP_Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 7, 19, 23, 42, 35, 582, DateTimeKind.Local).AddTicks(2051), "param1_twitter", "param2_twitter", "param3_twitter", "param4_twitter", "Twitter", null },
                    { 2, new DateTime(2024, 7, 19, 23, 42, 35, 582, DateTimeKind.Local).AddTicks(2060), "param1_pixiv", "param2_pixiv", "param3_pixiv", null, "Pixiv", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CATE");

            migrationBuilder.DropTable(
                name: "CATE_BINDING");

            migrationBuilder.DropTable(
                name: "IMAGE");

            migrationBuilder.DropTable(
                name: "SystemProperty");

            migrationBuilder.DropTable(
                name: "TAG");

            migrationBuilder.DropTable(
                name: "TAG_BINDING");
        }
    }
}
