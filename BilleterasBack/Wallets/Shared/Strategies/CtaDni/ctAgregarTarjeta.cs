using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilleterasBack.Wallets.Shared.Strategies.CtaDni
{
    public class ctAgregarTarjeta : IAgregarCard
    {
        private readonly AppDbContext _context;
        public ctAgregarTarjeta(AppDbContext context)
        {
            _context = context;
        }
        public bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
            try
            {
                DateTime ahora = DateTime.Now;
               

                var billetera = _context.Billeteras
                    .Where(b => b.Tipo == "CuentaDni" && b.Usuario.Dni == dni)
                    .FirstOrDefault();

                if (billetera == null)
                {
                    return false;
                }

                if (!numTarjeta.StartsWith("5195")) //con startwith comprobamos que la cadena empiece con el dato que pasamos por parametro.
                {
                    return false;
                }

                if (numTarjeta.Length != 22)
                {
                    return false;
                }

                // Verificar nombre y apellido
                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido))
                {
                    return false;
                }
                // Validar fecha de vencimiento
                if (fechaVenc < ahora)
                {
                    Console.WriteLine($"Error con la fecha de vencimiento: {fechaVenc}");
                    return false;
                }
                // Validar codigo de seguridad 
                if (cod > 999 || cod <= 99)
                {
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar la tarjeta: {ex.Message}");
                return false;
            }
        }
    }
}

