using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace foroLIS_backend.Migrations
{
    /// <inheritdoc />
    public partial class survey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
