using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Interfaces;
using BilleterasBack.Wallets.Validaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BilleterasBack.Wallets.Shared.Strategies.CtaDni
{
    public class ctAgregarTarjeta : IAgregarCard
    {
        private readonly AppDbContext _context;
        private readonly Validador _valiciones;
        public string Message { get; private set; } = string.Empty;

        public ctAgregarTarjeta(AppDbContext context, Validador validador)
        {
            _context = context;
            _valiciones = validador;
        }
        public bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
                DateTime ahora = DateTime.Now;

            if (!_valiciones.validarNumTarjeta(numTarjeta))
            {
                Message = "Error al validar el numero de tarjeta. ";
                return false;
            }

            if (!_valiciones.validarNombre(nombre))
            {
                Message = "Error al validar el nombre. ";
                return false;
            }

            if (!_valiciones.validarApellido(apellido))
            {
                Message = "Error al validar el apellido. ";
                return false;
            }

            if (!_valiciones.validarDNI(dni))
            {
                Message = "Error al validar el dni. ";
                return false;
            }

            if (!_valiciones.EsFechaVencimientoValida(fechaVenc))
            {
                Message = "La fecha de vencimiento no es valida";
                return false;
            }

            if (!_valiciones.validarCod(cod))
            {
                Message = "Error al validar el codigo de la tarjeta. ";
                return false;
            }

            var billetera = _context.Billeteras
                    .Where(b => b.Tipo == "CuentaDni" && b.Usuario!.Dni == dni)
                    .FirstOrDefault();

                if (billetera == null)
                {
                    Message = "No se puede agregar una tarjeta si no tiene una billetera Cuenta Dni Asociada o El Dni No existe.";
                    return false;
                }

                if (billetera.Usuario?.Nombre != nombre)
                {
                    Message = "El nombre no coincide. ";
                    return false;
                }

                if (billetera.Usuario.Apellido != apellido)
                {
                    Message = "El apellido no coincide. ";
                    return false;
                }

                if (!numTarjeta.StartsWith("5195")) //con startwith comprobamos que la cadena empiece con el dato que pasamos por parametro.
                {
                    Message = "Las tarjeta debe ser del banco provincia debe iniciar con 5195";
                    return false;
                }

                var tarjeta = new Tarjeta
                {
                    NumeroTarjeta = numTarjeta,
                    FechaVencimiento = fechaVenc,
                    CodigoSeguridad = cod,
                    Saldo = 10000,
                    IdBilletera = billetera.IdBilletera
                };
                _context.Tarjetas.Add(tarjeta);
                _context.SaveChanges();
                return true;
            }     
    }
}

