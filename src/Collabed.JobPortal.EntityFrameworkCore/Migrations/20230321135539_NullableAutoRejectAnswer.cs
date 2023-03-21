using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class NullableAutoRejectAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAutoRejectQuestion",
                table: "AppScreeningQuestions");

            migrationBuilder.AddColumn<bool>(
                name: "AutoRejectAnswer",
                table: "AppScreeningQuestions",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoRejectAnswer",
                table: "AppScreeningQuestions");

            migrationBuilder.AddColumn<bool>(
                name: "IsAutoRejectQuestion",
                table: "AppScreeningQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
