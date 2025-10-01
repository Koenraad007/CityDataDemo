using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AP.CityDataDemo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "City");

            migrationBuilder.EnsureSchema(
                name: "Country");

            migrationBuilder.CreateTable(
                name: "tblCountries",
                schema: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCountries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblCities",
                schema: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Population = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblCities_tblCountries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "Country",
                        principalTable: "tblCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Country",
                table: "tblCountries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Belgium" },
                    { 2, "UK" },
                    { 3, "France" },
                    { 4, "Netherlands" },
                    { 5, "Germany" }
                });

            migrationBuilder.InsertData(
                schema: "City",
                table: "tblCities",
                columns: new[] { "Id", "CountryId", "Name", "Population" },
                values: new object[,]
                {
                    { 1, 1, "Brussels", 1860000 },
                    { 2, 2, "London", 8900000 },
                    { 3, 3, "Paris", 2140000 },
                    { 4, 4, "Amsterdam", 872000 },
                    { 5, 5, "Berlin", 3769000 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblCities_CountryId",
                schema: "City",
                table: "tblCities",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblCities",
                schema: "City");

            migrationBuilder.DropTable(
                name: "tblCountries",
                schema: "Country");
        }
    }
}
