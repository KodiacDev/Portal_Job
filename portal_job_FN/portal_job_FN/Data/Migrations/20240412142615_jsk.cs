using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portal_job_FN.Migrations
{
    /// <inheritdoc />
    public partial class jsk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_job_location_job_locationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_post_Jobs_images_post_Jobs_post_JobId",
                table: "post_Jobs_images");

            migrationBuilder.DropColumn(
                name: "post_job_id",
                table: "post_Jobs_images");

            migrationBuilder.DropColumn(
                name: "applicationUser_id",
                table: "company_image");

            migrationBuilder.RenameColumn(
                name: "job_locationId",
                table: "AspNetUsers",
                newName: "JobLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_job_locationId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_JobLocationId");

            migrationBuilder.AlterColumn<int>(
                name: "post_JobId",
                table: "post_Jobs_images",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_job_location_JobLocationId",
                table: "AspNetUsers",
                column: "JobLocationId",
                principalTable: "job_location",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_post_Jobs_images_post_Jobs_post_JobId",
                table: "post_Jobs_images",
                column: "post_JobId",
                principalTable: "post_Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_job_location_JobLocationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_post_Jobs_images_post_Jobs_post_JobId",
                table: "post_Jobs_images");

            migrationBuilder.RenameColumn(
                name: "JobLocationId",
                table: "AspNetUsers",
                newName: "job_locationId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_JobLocationId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_job_locationId");

            migrationBuilder.AlterColumn<int>(
                name: "post_JobId",
                table: "post_Jobs_images",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "post_job_id",
                table: "post_Jobs_images",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "applicationUser_id",
                table: "company_image",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_job_location_job_locationId",
                table: "AspNetUsers",
                column: "job_locationId",
                principalTable: "job_location",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_post_Jobs_images_post_Jobs_post_JobId",
                table: "post_Jobs_images",
                column: "post_JobId",
                principalTable: "post_Jobs",
                principalColumn: "Id");
        }
    }
}
