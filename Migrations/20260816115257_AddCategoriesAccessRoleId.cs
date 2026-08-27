using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TabletobClubBot.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriesAccessRoleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AccessRoleId",
                table: "hqConfigs",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsCommon",
                table: "hqConfigs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessRoleId",
                table: "hqConfigs");

            migrationBuilder.DropColumn(
                name: "IsCommon",
                table: "hqConfigs");
        }
    }
}
