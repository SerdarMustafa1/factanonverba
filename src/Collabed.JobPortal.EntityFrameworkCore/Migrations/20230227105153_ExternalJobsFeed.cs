using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class ExternalJobsFeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExternal",
                table: "AppJobs");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "AppJobs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Industry",
                table: "AppJobs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationEmail",
                table: "AppJobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUrl",
                table: "AppJobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "AppJobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DaysToAdvertise",
                table: "AppJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JobOrigin",
                table: "AppJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationEmail",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "ApplicationUrl",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "DaysToAdvertise",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "JobOrigin",
                table: "AppJobs");

            migrationBuilder.AlterColumn<int>(
                name: "Location",
                table: "AppJobs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Industry",
                table: "AppJobs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExternal",
                table: "AppJobs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
