using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_job_FN.Migrations
{
    /// <inheritdoc />
    public partial class iku : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apply_Jobs_majors_MajorId",
                table: "apply_Jobs");

            migrationBuilder.DropIndex(
                name: "IX_apply_Jobs_MajorId",
                table: "apply_Jobs");

            migrationBuilder.DropColumn(
                name: "MajorId",
                table: "apply_Jobs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MajorId",
                table: "apply_Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_apply_Jobs_MajorId",
                table: "apply_Jobs",
                column: "MajorId");

            migrationBuilder.AddForeignKey(
                name: "FK_apply_Jobs_majors_MajorId",
                table: "apply_Jobs",
                column: "MajorId",
                principalTable: "majors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
