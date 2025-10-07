using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BilleterasBack.Migrations
{
    /// <inheritdoc />
    public partial class MovimientosModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposMovimiento",
                columns: table => new
                {
                    IdTipoMovimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposMovimiento", x => x.IdTipoMovimiento);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosBilletera",
                columns: table => new
                {
                    IdMovimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaMovimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdBilletera = table.Column<int>(type: "int", nullable: false),
                    IdTipoMovimiento = table.Column<int>(type: "int", nullable: false),
                    IdTarjeta = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosBilletera", x => x.IdMovimiento);
                    table.ForeignKey(
                        name: "FK_MovimientosBilletera_Billetera_IdBilletera",
                        column: x => x.IdBilletera,
                        principalTable: "Billetera",
                        principalColumn: "IdBilletera",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovimientosBilletera_Tarjeta_IdTarjeta",
                        column: x => x.IdTarjeta,
                        principalTable: "Tarjeta",
                        principalColumn: "IdTarjeta");
                    table.ForeignKey(
                        name: "FK_MovimientosBilletera_TiposMovimiento_IdTipoMovimiento",
                        column: x => x.IdTipoMovimiento,
                        principalTable: "TiposMovimiento",
                        principalColumn: "IdTipoMovimiento",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosBilletera_IdBilletera",
                table: "MovimientosBilletera",
                column: "IdBilletera");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosBilletera_IdTarjeta",
                table: "MovimientosBilletera",
                column: "IdTarjeta");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosBilletera_IdTipoMovimiento",
                table: "MovimientosBilletera",
                column: "IdTipoMovimiento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimientosBilletera");

            migrationBuilder.DropTable(
                name: "TiposMovimiento");
        }
    }
}
