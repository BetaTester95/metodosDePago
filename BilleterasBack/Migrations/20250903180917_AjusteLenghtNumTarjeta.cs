using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BilleterasBack.Migrations
{
    /// <inheritdoc />
    public partial class AjusteLenghtNumTarjeta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumeroTarjeta",
                table: "Tarjeta",
                type: "nvarchar(22)",
                maxLength: 22,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumeroTarjeta",
                table: "Tarjeta",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(22)",
                oldMaxLength: 22);
        }
    }
}
