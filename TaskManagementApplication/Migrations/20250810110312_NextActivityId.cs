using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementApplication.Migrations
{
    /// <inheritdoc />
    public partial class NextActivityId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NextActivityId",
                table: "OnBoardingTasks",
                type: "nvarchar(300)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextActivityId",
                table: "OnBoardingTasks");
        }
    }
}
