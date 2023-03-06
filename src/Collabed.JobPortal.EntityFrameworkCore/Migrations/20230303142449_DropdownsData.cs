using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class DropdownsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "AppJobs");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "AppJobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AppJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppJobCategories",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppJobCategories", x => new { x.JobId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_AppJobCategories_AppJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "AppJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(8,6)", precision: 8, scale: 6, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSupplementalPays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSupplementalPays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSupportingDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSupportingDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppJobLanguages",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppJobLanguages", x => new { x.JobId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_AppJobLanguages_AppJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "AppJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppJobLanguages_AppLanguages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "AppLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppJobSchedules",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppJobSchedules", x => new { x.JobId, x.ScheduleId });
                    table.ForeignKey(
                        name: "FK_AppJobSchedules_AppJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "AppJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppJobSchedules_AppSchedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "AppSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppJobSupplementalPays",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplementalPayId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppJobSupplementalPays", x => new { x.JobId, x.SupplementalPayId });
                    table.ForeignKey(
                        name: "FK_AppJobSupplementalPays_AppJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "AppJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppJobSupplementalPays_AppSupplementalPays_SupplementalPayId",
                        column: x => x.SupplementalPayId,
                        principalTable: "AppSupplementalPays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppJobSupportingDocuments",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupportingDocumentId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppJobSupportingDocuments", x => new { x.JobId, x.SupportingDocumentId });
                    table.ForeignKey(
                        name: "FK_AppJobSupportingDocuments_AppJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "AppJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppJobSupportingDocuments_AppSupportingDocuments_SupportingDocumentId",
                        column: x => x.SupportingDocumentId,
                        principalTable: "AppSupportingDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobs_LocationId",
                table: "AppJobs",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobCategories_JobId_CategoryId",
                table: "AppJobCategories",
                columns: new[] { "JobId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobLanguages_JobId_LanguageId",
                table: "AppJobLanguages",
                columns: new[] { "JobId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobLanguages_LanguageId",
                table: "AppJobLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobSchedules_JobId_ScheduleId",
                table: "AppJobSchedules",
                columns: new[] { "JobId", "ScheduleId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobSchedules_ScheduleId",
                table: "AppJobSchedules",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobSupplementalPays_JobId_SupplementalPayId",
                table: "AppJobSupplementalPays",
                columns: new[] { "JobId", "SupplementalPayId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobSupplementalPays_SupplementalPayId",
                table: "AppJobSupplementalPays",
                column: "SupplementalPayId");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobSupportingDocuments_JobId_SupportingDocumentId",
                table: "AppJobSupportingDocuments",
                columns: new[] { "JobId", "SupportingDocumentId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobSupportingDocuments_SupportingDocumentId",
                table: "AppJobSupportingDocuments",
                column: "SupportingDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobs_AppLocations_LocationId",
                table: "AppJobs",
                column: "LocationId",
                principalTable: "AppLocations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppJobs_AppLocations_LocationId",
                table: "AppJobs");

            migrationBuilder.DropTable(
                name: "AppCategories");

            migrationBuilder.DropTable(
                name: "AppJobCategories");

            migrationBuilder.DropTable(
                name: "AppJobLanguages");

            migrationBuilder.DropTable(
                name: "AppJobSchedules");

            migrationBuilder.DropTable(
                name: "AppJobSupplementalPays");

            migrationBuilder.DropTable(
                name: "AppJobSupportingDocuments");

            migrationBuilder.DropTable(
                name: "AppLocations");

            migrationBuilder.DropTable(
                name: "AppLanguages");

            migrationBuilder.DropTable(
                name: "AppSchedules");

            migrationBuilder.DropTable(
                name: "AppSupplementalPays");

            migrationBuilder.DropTable(
                name: "AppSupportingDocuments");

            migrationBuilder.DropIndex(
                name: "IX_AppJobs_LocationId",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppJobs");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "AppJobs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
