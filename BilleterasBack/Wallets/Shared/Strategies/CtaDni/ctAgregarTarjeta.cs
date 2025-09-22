using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Exceptions;
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
        public ctAgregarTarjeta(AppDbContext context, Validador validador)
        {
            _context = context;
            _valiciones = validador;
        }
        public bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
                DateTime ahora = DateTime.Now;

            if (!_valiciones.validarNumTarjeta(numTarjeta))
                throw new ArgumentException("Error al validar el numero de tarjeta. ");

            if (_valiciones.validarNombre(nombre))
                throw new ArgumentException("Error al validar el nombre. ");

            if (!_valiciones.validarApellido(apellido))
                throw new ArgumentException("Error al validar el apellido. ");

            if (!_valiciones.validarDNI(dni))
                throw new ArgumentException("Error al validar el dni. ");

            if (fechaVenc < ahora)
                throw new ArgumentException("La tarjeta esta vencida, su fecha expiro. ");
            
            if (!_valiciones.validarCod(cod))
                throw new ArgumentException("Error al validar el codigo de la tarjeta. ");

            var billetera = _context.Billeteras
                    .Where(b => b.Tipo == "CuentaDni" && b.Usuario!.Dni == dni)
                    .FirstOrDefault();

                if (billetera == null)
                        throw new ArgumentException("No se puede agregar una tarjeta si no tiene una billetera Cuenta Dni Asociada o El Dni No existe.");

                if (billetera.Usuario?.Nombre != nombre)
                        throw new ArgumentException("El nombre no coincide. ");
                
                if (billetera.Usuario.Apellido != apellido)
                        throw new ArgumentException("El apellido no coincide. ");

                if (!numTarjeta.StartsWith("5195")) //con startwith comprobamos que la cadena empiece con el dato que pasamos por parametro.
                    throw new ArgumentException("Las tarjeta debe ser del banco provincia debe iniciar con 5195");
                
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

