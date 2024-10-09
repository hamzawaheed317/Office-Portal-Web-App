using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficePortal.Migrations
{
    /// <inheritdoc />
    public partial class initial_setup2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "MissionandTrainingForm",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LineManager",
                table: "MissionandTrainingForm",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "MissionandTrainingForm");

            migrationBuilder.DropColumn(
                name: "LineManager",
                table: "MissionandTrainingForm");
        }
    }
}
