using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem22.Migrations
{
    public partial class Testi1711 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Users_OwnerId",
                table: "Items");

            migrationBuilder.AlterColumn<long>(
                name: "OwnerId",
                table: "Items",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Users_OwnerId",
                table: "Items",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Users_OwnerId",
                table: "Items");

            migrationBuilder.AlterColumn<long>(
                name: "OwnerId",
                table: "Items",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Users_OwnerId",
                table: "Items",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
