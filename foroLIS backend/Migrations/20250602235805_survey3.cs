using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace foroLIS_backend.Migrations
{
    /// <inheritdoc />
    public partial class survey3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMessages_Surveys_SurveyId",
                table: "CommunityMessages");

            migrationBuilder.DropIndex(
                name: "IX_CommunityMessages_SurveyId",
                table: "CommunityMessages");

            migrationBuilder.DropColumn(
                name: "SurveyId",
                table: "CommunityMessages");

            migrationBuilder.CreateTable(
                name: "CommunitySurveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunitySurveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunitySurveys_CommunityMessages_Id",
                        column: x => x.Id,
                        principalTable: "CommunityMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunitySurveys");

            migrationBuilder.AddColumn<Guid>(
                name: "SurveyId",
                table: "CommunityMessages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMessages_SurveyId",
                table: "CommunityMessages",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMessages_Surveys_SurveyId",
                table: "CommunityMessages",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id");
        }
    }
}
