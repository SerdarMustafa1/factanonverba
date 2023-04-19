using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class UserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppJobApplicants_AppJobs_UserId",
                table: "AppJobApplicants");

            migrationBuilder.AddColumn<string>(
                name: "CoverLetter",
                table: "AppJobApplicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CvBlobName",
                table: "AppJobApplicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CvContentType",
                table: "AppJobApplicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CvFileName",
                table: "AppJobApplicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "JobApplicantId",
                table: "AppJobApplicants",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AppJobApplicants_JobApplicantId",
                table: "AppJobApplicants",
                column: "JobApplicantId");

            migrationBuilder.CreateTable(
                name: "AppApplicantScreeningAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobApplicantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScreeningQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Answer = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApplicantScreeningAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppApplicantScreeningAnswers_AppJobApplicants_JobApplicantId",
                        column: x => x.JobApplicantId,
                        principalTable: "AppJobApplicants",
                        principalColumn: "JobApplicantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppApplicantScreeningAnswers_AppScreeningQuestions_ScreeningQuestionId",
                        column: x => x.ScreeningQuestionId,
                        principalTable: "AppScreeningQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CvBlobName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CvFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CvContentType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserProfiles_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppApplicantScreeningAnswers_JobApplicantId",
                table: "AppApplicantScreeningAnswers",
                column: "JobApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_AppApplicantScreeningAnswers_ScreeningQuestionId",
                table: "AppApplicantScreeningAnswers",
                column: "ScreeningQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserProfiles_UserId",
                table: "AppUserProfiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobApplicants_AppJobs_JobId",
                table: "AppJobApplicants",
                column: "JobId",
                principalTable: "AppJobs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppJobApplicants_AppJobs_JobId",
                table: "AppJobApplicants");

            migrationBuilder.DropTable(
                name: "AppApplicantScreeningAnswers");

            migrationBuilder.DropTable(
                name: "AppUserProfiles");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AppJobApplicants_JobApplicantId",
                table: "AppJobApplicants");

            migrationBuilder.DropColumn(
                name: "CoverLetter",
                table: "AppJobApplicants");

            migrationBuilder.DropColumn(
                name: "CvBlobName",
                table: "AppJobApplicants");

            migrationBuilder.DropColumn(
                name: "CvContentType",
                table: "AppJobApplicants");

            migrationBuilder.DropColumn(
                name: "CvFileName",
                table: "AppJobApplicants");

            migrationBuilder.DropColumn(
                name: "JobApplicantId",
                table: "AppJobApplicants");

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobApplicants_AppJobs_UserId",
                table: "AppJobApplicants",
                column: "UserId",
                principalTable: "AppJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
