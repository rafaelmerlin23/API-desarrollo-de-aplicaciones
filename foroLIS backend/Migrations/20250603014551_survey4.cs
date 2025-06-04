using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace foroLIS_backend.Migrations
{
    /// <inheritdoc />
    public partial class survey4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunityFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityFields_CommunitySurveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "CommunitySurveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityFields_SurveyId",
                table: "CommunityFields",
                column: "SurveyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityFields");
        }
    }
}
