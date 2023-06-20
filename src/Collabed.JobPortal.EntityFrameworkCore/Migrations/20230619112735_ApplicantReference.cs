using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class ApplicantReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "AppJobApplicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE AppJobApplicants SET Reference = CONVERT(varchar(255), NEWID()) WHERE Reference is null");

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "AppJobApplicants",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reference",
                table: "AppJobApplicants");
        }
    }
}
