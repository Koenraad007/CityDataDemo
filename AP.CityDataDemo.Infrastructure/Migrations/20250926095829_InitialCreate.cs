using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AP.CityDataDemo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false)
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
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
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
                values: new object[] { 1, "UK" });

            migrationBuilder.InsertData(
                schema: "City",
                table: "tblCities",
                columns: new[] { "Id", "CountryId", "Name", "Population" },
                values: new object[] { 1, 1, "London", 9800000 });

            migrationBuilder.CreateIndex(
                name: "IX_tblCities_CountryId",
                schema: "City",
                table: "tblCities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_tblCities_Id",
                schema: "City",
                table: "tblCities",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblCountries_Id",
                schema: "Country",
                table: "tblCountries",
                column: "Id",
                unique: true);
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
