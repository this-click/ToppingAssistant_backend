using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ReplacePriorityColumnWithWeekly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Toppers");

            migrationBuilder.AddColumn<bool>(
                name: "ThisWeek",
                table: "Toppers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThisWeek",
                table: "Toppers");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Toppers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
