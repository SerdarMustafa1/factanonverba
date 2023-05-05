using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class JobRefIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "AppJobs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppJobs_Reference",
                table: "AppJobs",
                column: "Reference");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppJobs_Reference",
                table: "AppJobs");

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "AppJobs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
