using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BilleterasBack.Migrations
{
    /// <inheritdoc />
    public partial class CambiarDniAint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Dni",
                table: "Usuario",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Dni",
                table: "Usuario",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
