using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_job_FN.Migrations
{
    /// <inheritdoc />
    public partial class ibu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apply_Jobs_majors_MajorId",
                table: "apply_Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_apply_Jobs_post_Jobs_post_JobId",
                table: "apply_Jobs");

            migrationBuilder.DropColumn(
                name: "major_id",
                table: "apply_Jobs");

            migrationBuilder.DropColumn(
                name: "post_job_id",
                table: "apply_Jobs");

            migrationBuilder.RenameColumn(
                name: "application_user_id",
                table: "apply_Jobs",
                newName: "application_userId");

            migrationBuilder.RenameColumn(
                name: "application_company_id",
                table: "apply_Jobs",
                newName: "FullName");

            migrationBuilder.AlterColumn<int>(
                name: "post_JobId",
                table: "apply_Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MajorId",
                table: "apply_Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "apply_Jobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_apply_Jobs_majors_MajorId",
                table: "apply_Jobs",
                column: "MajorId",
                principalTable: "majors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_apply_Jobs_post_Jobs_post_JobId",
                table: "apply_Jobs",
                column: "post_JobId",
                principalTable: "post_Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apply_Jobs_majors_MajorId",
                table: "apply_Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_apply_Jobs_post_Jobs_post_JobId",
                table: "apply_Jobs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "apply_Jobs");

            migrationBuilder.RenameColumn(
                name: "application_userId",
                table: "apply_Jobs",
                newName: "application_user_id");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "apply_Jobs",
                newName: "application_company_id");

            migrationBuilder.AlterColumn<int>(
                name: "post_JobId",
                table: "apply_Jobs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MajorId",
                table: "apply_Jobs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "major_id",
                table: "apply_Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "post_job_id",
                table: "apply_Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_apply_Jobs_majors_MajorId",
                table: "apply_Jobs",
                column: "MajorId",
                principalTable: "majors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_apply_Jobs_post_Jobs_post_JobId",
                table: "apply_Jobs",
                column: "post_JobId",
                principalTable: "post_Jobs",
                principalColumn: "Id");
        }
    }
}
