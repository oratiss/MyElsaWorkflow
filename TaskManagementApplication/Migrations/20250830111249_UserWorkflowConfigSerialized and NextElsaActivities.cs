using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementApplication.Migrations
{
    /// <inheritdoc />
    public partial class UserWorkflowConfigSerializedandNextElsaActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextActivityId",
                table: "Steps",
                newName: "NextElsaActivities");

            migrationBuilder.CreateTable(
                name: "WorkflowEntities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workflows",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElsaWorkflowDefinitionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkflowEntitiesId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workflows_WorkflowEntities_WorkflowEntitiesId",
                        column: x => x.WorkflowEntitiesId,
                        principalTable: "WorkflowEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workflows_WorkflowEntitiesId",
                table: "Workflows",
                column: "WorkflowEntitiesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workflows");

            migrationBuilder.DropTable(
                name: "WorkflowEntities");

            migrationBuilder.RenameColumn(
                name: "NextElsaActivities",
                table: "Steps",
                newName: "NextActivityId");
        }
    }
}
