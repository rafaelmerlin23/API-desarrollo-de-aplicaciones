using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace foroLIS_backend.Migrations
{
    /// <inheritdoc />
    public partial class commentCommunityMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunityMessageComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommunityMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityMessageComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityMessageComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityMessageComments_CommunityMessages_CommunityMessageId",
                        column: x => x.CommunityMessageId,
                        principalTable: "CommunityMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMessageComments_CommunityMessageId",
                table: "CommunityMessageComments",
                column: "CommunityMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMessageComments_UserId",
                table: "CommunityMessageComments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityMessageComments");
        }
    }
}
