using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_job_FN.Migrations
{
    /// <inheritdoc />
    public partial class initDBB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_post_Jobs_Experience_experienceId",
                table: "post_Jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Experience",
                table: "Experience");

            migrationBuilder.RenameTable(
                name: "Experience",
                newName: "experience");

            migrationBuilder.AddPrimaryKey(
                name: "PK_experience",
                table: "experience",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_post_Jobs_experience_experienceId",
                table: "post_Jobs",
                column: "experienceId",
                principalTable: "experience",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_post_Jobs_experience_experienceId",
                table: "post_Jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_experience",
                table: "experience");

            migrationBuilder.RenameTable(
                name: "experience",
                newName: "Experience");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Experience",
                table: "Experience",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_post_Jobs_Experience_experienceId",
                table: "post_Jobs",
                column: "experienceId",
                principalTable: "Experience",
                principalColumn: "Id");
        }
    }
}
