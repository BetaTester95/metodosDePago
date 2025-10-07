using Azure.Core;
using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using BilleterasBack.Wallets.Validaciones;

namespace BilleterasBack.Wallets.Shared.Strategies.Mp
{
    public class MpAgregarTarjeta : IAgregarCard
    {
        private readonly AppDbContext _context;
        private readonly Validador? _validaciones;
        public string Message { get; private set; } = string.Empty;

        public MpAgregarTarjeta(AppDbContext context, Validador validador)
        {
           _context = context;
           _validaciones = validador;
        }

        public bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
          
            if (_validaciones == null)
                return false;

            if (!_validaciones.validarNumTarjeta(numTarjeta))
            {
                this.Message = "El numero de tarjeta no es valido";
                return false;           
            }

            if (!_validaciones.validarNombre(nombre))
            {
                this.Message = "El nombre no es valido";
                return false;
            }

            if (!_validaciones.validarApellido(apellido))
            {
                this.Message = "El apellido no es valido";
                return false;
            }
            
            if (!_validaciones.validarDNI(dni))
            {
                this.Message = "Error al validar el dni";
                return false;
            }

            if(!_validaciones.EsFechaVencimientoValida(fechaVenc))
            {
                this.Message = "La fecha de vencimiento no es valida";
                return false;
            }

            if (!_validaciones.validarCod(cod))
            {
                this.Message = "Error al validar el cod";
                return false;
            }

            var identificacionMercadoPago = _context.Billeteras
                    .Include(b => b.Usuario)
                    .FirstOrDefault(b => b.Tipo == "MercadoPago" && b.Usuario!.Dni == dni);
            if (identificacionMercadoPago == null)
            {
                this.Message = "No se encontró una billetera MercadoPago para el usuario o los datos personales no coinciden.";
                return false;
            }

            var identificarNameLastName = identificacionMercadoPago.Usuario.Nombre == nombre && identificacionMercadoPago.Usuario.Apellido == apellido;
            if (!identificarNameLastName)
            {
                this.Message = "El nombre o apellido no coinciden con los datos del usuario asociado a la billetera MercadoPago.";
                return false;
            }

            var tarjetaExistente = _context.Tarjetas
                    .FirstOrDefault(t => t.NumeroTarjeta == numTarjeta && t.IdBilletera == identificacionMercadoPago.IdBilletera);

            if (tarjetaExistente != null)
            {
                this.Message = "Ya existe una tarjeta con este número en la billetera de MercadoPago.";
                return false;
            }

            var tarjeta = new Tarjeta
            {
                NumeroTarjeta = numTarjeta,
                FechaVencimiento = fechaVenc,
                CodigoSeguridad = cod,
                Saldo = 10000,
                IdBilletera = identificacionMercadoPago.IdBilletera
            };
            _context.Tarjetas.Add(tarjeta);
            _context.SaveChanges();
            return true;
        }
    }
}