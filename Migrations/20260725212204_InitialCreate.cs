using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TabletobClubBot.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hQConfigs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hQConfigs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HQChannel",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ChannelType = table.Column<int>(type: "integer", nullable: false),
                    BotOnly = table.Column<bool>(type: "boolean", nullable: false),
                    HQConfigID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HQChannel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HQChannel_hQConfigs_HQConfigID",
                        column: x => x.HQConfigID,
                        principalTable: "hQConfigs",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HQChannel_HQConfigID",
                table: "HQChannel",
                column: "HQConfigID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HQChannel");

            migrationBuilder.DropTable(
                name: "hQConfigs");
        }
    }
}
