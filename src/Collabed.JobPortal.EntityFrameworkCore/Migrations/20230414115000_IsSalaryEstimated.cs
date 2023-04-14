using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class IsSalaryEstimated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSalaryEstimated",
                table: "AppJobs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSalaryEstimated",
                table: "AppJobs");
        }
    }
}
