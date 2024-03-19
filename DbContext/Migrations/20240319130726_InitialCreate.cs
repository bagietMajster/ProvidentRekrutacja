using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NBP.Context.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExchangeRateTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    TableType = table.Column<string>(type: "varchar(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRateTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRateValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrencyCode = table.Column<string>(type: "varchar", nullable: false),
                    Mid = table.Column<double>(type: "double", nullable: false),
                    ExchangeRateTableId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRateValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeRateValue_ExchangeRateTable_ExchangeRateTableId",
                        column: x => x.ExchangeRateTableId,
                        principalTable: "ExchangeRateTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRateValue_ExchangeRateTableId",
                table: "ExchangeRateValue",
                column: "ExchangeRateTableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeRateValue");

            migrationBuilder.DropTable(
                name: "ExchangeRateTable");
        }
    }
}
