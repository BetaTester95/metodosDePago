using Azure.Core;
using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Exceptions;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BilleterasBack.Wallets.Validaciones;

namespace BilleterasBack.Wallets.Shared.Strategies.Mp
{
    public class MpAgregarTarjeta : IAgregarCard
    {
        private readonly AppDbContext _context;
        private readonly Validador? _validaciones;

        public MpAgregarTarjeta(AppDbContext context, Validador validador)
        {
           _context = context;
           _validaciones = validador;
        }

        public bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
            DateTime ahora = DateTime.Now;

            if (_validaciones == null)
                throw new InvalidOperationException("El validador no ha sido inicializado.");

            if (!_validaciones.validarNumTarjeta(numTarjeta))
                throw new ArgumentException("El numeor de tarjeta no es valido");

            if (!_validaciones.validarNombre(nombre))
                throw new ArgumentException("El nombre no es valido");

            if (!_validaciones.validarApellido(apellido))
                throw new ArgumentException("El apellido no es valido");

            if (!_validaciones.validarDNI(dni))
                throw new ArgumentException("error al validar el dni");

            if (!_validaciones.validarCod(cod))
                throw new ArgumentException("Error al validar el cod");

            var identificacionMercadoPago = _context.Billeteras
                    .Include(b => b.Usuario)
                    .FirstOrDefault(b => b.Tipo == "MercadoPago" && b.Usuario!.Dni == dni);
            if (identificacionMercadoPago == null)
                throw new UsuarioExceptions("No se encontró una billetera PayPal para el usuario o los datos personales no coinciden.");

            var tarjetaExistente = _context.Tarjetas
                    .FirstOrDefault(t => t.NumeroTarjeta == numTarjeta && t.IdBilletera == identificacionMercadoPago.IdBilletera);

            if (tarjetaExistente != null)
                throw new BilleteraExceptions("Ya existe una tarjeta con este número en la billetera de PayPal.");

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