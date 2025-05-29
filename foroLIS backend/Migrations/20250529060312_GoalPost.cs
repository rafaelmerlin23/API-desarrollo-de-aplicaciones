using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace foroLIS_backend.Migrations
{
    /// <inheritdoc />
    public partial class GoalPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeAccountId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<decimal>(
                name: "Goal",
                table: "Posts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Goal",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "StripeAccountId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
