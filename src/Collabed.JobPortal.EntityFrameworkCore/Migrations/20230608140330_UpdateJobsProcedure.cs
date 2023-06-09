using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class UpdateJobsProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[UpdateJobStatus]
                AS
                BEGIN
                  Update [AppJobs]
                  set [Status] = 1
                  where DATEDIFF (DAY, [ApplicationDeadline], GETDATE())  > 0
                END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP PROCEDURE [dbo].[UpdateJobStatus]";

            migrationBuilder.Sql(sp);
        }
    }
}
