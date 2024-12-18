using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_job_FN.Migrations
{
    /// <inheritdoc />
    public partial class inut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "experience_id",
                table: "post_Jobs");

            migrationBuilder.DropColumn(
                name: "location_id",
                table: "post_Jobs");

            migrationBuilder.RenameColumn(
                name: "major_id",
                table: "post_Jobs",
                newName: "locationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "locationId",
                table: "post_Jobs",
                newName: "major_id");

            migrationBuilder.AddColumn<int>(
                name: "experience_id",
                table: "post_Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                table: "post_Jobs",
                type: "int",
                nullable: true);
        }
    }
}
