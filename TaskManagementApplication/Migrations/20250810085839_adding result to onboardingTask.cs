using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementApplication.Migrations
{
    /// <inheritdoc />
    public partial class addingresulttoonboardingTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "OnBoardingTasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "OnBoardingTasks");
        }
    }
}
