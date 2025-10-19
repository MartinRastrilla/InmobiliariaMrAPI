using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InmobiliariaMrAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixPropietarioIdOnUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Propietarios_PropietarioId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PropietarioId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PropietarioId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Propietarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Propietarios_UserId",
                table: "Propietarios",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Propietarios_Users_UserId",
                table: "Propietarios",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Propietarios_Users_UserId",
                table: "Propietarios");

            migrationBuilder.DropIndex(
                name: "IX_Propietarios_UserId",
                table: "Propietarios");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Propietarios");

            migrationBuilder.AddColumn<int>(
                name: "PropietarioId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PropietarioId",
                table: "Users",
                column: "PropietarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Propietarios_PropietarioId",
                table: "Users",
                column: "PropietarioId",
                principalTable: "Propietarios",
                principalColumn: "Id");
        }
    }
}
