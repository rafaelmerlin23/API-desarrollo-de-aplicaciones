using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace foroLIS_backend.Migrations
{
    /// <inheritdoc />
    public partial class survey5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunityUserFields",
                columns: table => new
                {
                    CommunityFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommunityFieldSurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityUserFields", x => new { x.CommunityFieldId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CommunityUserFields_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommunityUserFields_CommunityFields_CommunityFieldId",
                        column: x => x.CommunityFieldId,
                        principalTable: "CommunityFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommunityUserFields_CommunityFields_CommunityFieldSurveyId",
                        column: x => x.CommunityFieldSurveyId,
                        principalTable: "CommunityFields",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityUserFields_CommunityFieldSurveyId",
                table: "CommunityUserFields",
                column: "CommunityFieldSurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityUserFields_UserId",
                table: "CommunityUserFields",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityUserFields");
        }
    }
}
