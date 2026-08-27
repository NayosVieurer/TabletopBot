using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TabletobClubBot.Migrations
{
    /// <inheritdoc />
    public partial class RenameHqConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HQChannel_hQConfigs_HQConfigID",
                table: "HQChannel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_hQConfigs",
                table: "hQConfigs");

            migrationBuilder.RenameTable(
                name: "hQConfigs",
                newName: "hqConfigs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_hqConfigs",
                table: "hqConfigs",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HQChannel_hqConfigs_HQConfigID",
                table: "HQChannel",
                column: "HQConfigID",
                principalTable: "hqConfigs",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HQChannel_hqConfigs_HQConfigID",
                table: "HQChannel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_hqConfigs",
                table: "hqConfigs");

            migrationBuilder.RenameTable(
                name: "hqConfigs",
                newName: "hQConfigs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_hQConfigs",
                table: "hQConfigs",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_HQChannel_hQConfigs_HQConfigID",
                table: "HQChannel",
                column: "HQConfigID",
                principalTable: "hQConfigs",
                principalColumn: "ID");
        }
    }
}
