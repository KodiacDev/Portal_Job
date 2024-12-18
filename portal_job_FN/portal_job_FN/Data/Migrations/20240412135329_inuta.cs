using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_job_FN.Migrations
{
    /// <inheritdoc />
    public partial class inuta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "applicationUser_id",
                table: "educations");

            migrationBuilder.DropColumn(
                name: "major_id",
                table: "educations");

            migrationBuilder.DropColumn(
                name: "university_id",
                table: "educations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "applicationUser_id",
                table: "educations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "major_id",
                table: "educations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "university_id",
                table: "educations",
                type: "int",
                nullable: true);
        }
    }
}
