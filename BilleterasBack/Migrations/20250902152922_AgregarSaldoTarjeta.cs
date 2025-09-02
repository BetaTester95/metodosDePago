using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BilleterasBack.Migrations
{
    /// <inheritdoc />
    public partial class AgregarSaldoTarjeta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Saldo",
                table: "Tarjeta",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Saldo",
                table: "Tarjeta");
        }
    }
}
