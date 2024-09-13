using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OracaoApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrayerPrayingFor");

            migrationBuilder.CreateIndex(
                name: "IX_PrayingFors_PrayerId",
                table: "PrayingFors",
                column: "PrayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrayingFors_Prayers_PrayerId",
                table: "PrayingFors",
                column: "PrayerId",
                principalTable: "Prayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrayingFors_Prayers_PrayerId",
                table: "PrayingFors");

            migrationBuilder.DropIndex(
                name: "IX_PrayingFors_PrayerId",
                table: "PrayingFors");

            migrationBuilder.CreateTable(
                name: "PrayerPrayingFor",
                columns: table => new
                {
                    PrayersId = table.Column<int>(type: "integer", nullable: false),
                    PrayingForsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrayerPrayingFor", x => new { x.PrayersId, x.PrayingForsId });
                    table.ForeignKey(
                        name: "FK_PrayerPrayingFor_Prayers_PrayersId",
                        column: x => x.PrayersId,
                        principalTable: "Prayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrayerPrayingFor_PrayingFors_PrayingForsId",
                        column: x => x.PrayingForsId,
                        principalTable: "PrayingFors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrayerPrayingFor_PrayingForsId",
                table: "PrayerPrayingFor",
                column: "PrayingForsId");
        }
    }
}
