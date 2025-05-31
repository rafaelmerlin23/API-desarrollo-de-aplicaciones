using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace foroLIS_backend.Migrations
{
    /// <inheritdoc />
    public partial class mercadoPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MercadoPagoAccessToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MercadoPagoUserId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MercadoPagoAccessToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MercadoPagoUserId",
                table: "AspNetUsers");
        }
    }
}
