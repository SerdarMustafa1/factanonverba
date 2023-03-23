using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class SalaryConversionFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxSalaryConverted",
                table: "AppJobs",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinSalaryConverted",
                table: "AppJobs",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxSalaryConverted",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "MinSalaryConverted",
                table: "AppJobs");
        }
    }
}
