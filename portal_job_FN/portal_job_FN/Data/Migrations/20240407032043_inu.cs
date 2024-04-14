using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_job_FN.Migrations
{
    /// <inheritdoc />
    public partial class inu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "application_user",
                table: "post_Jobs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "application_user",
                table: "post_Jobs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
