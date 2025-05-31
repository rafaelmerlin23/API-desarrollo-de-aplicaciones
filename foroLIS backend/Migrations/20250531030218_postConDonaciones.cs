using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace foroLIS_backend.Migrations
{
    /// <inheritdoc />
    public partial class postConDonaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "Donations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Donations_PostId",
                table: "Donations",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Posts_PostId",
                table: "Donations",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Posts_PostId",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_PostId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Donations");
        }
    }
}
