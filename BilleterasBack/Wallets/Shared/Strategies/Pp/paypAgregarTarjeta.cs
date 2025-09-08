using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioInterfaces.Estrategias.ppEstrategias
{
    public class paypAgregarTarjeta : IAgregarCard
    {
        private readonly AppDbContext _context;
        public paypAgregarTarjeta(AppDbContext context)
        {
            _context = context;
        }

        public bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
            try
            {
                DateTime ahora = DateTime.Now;
                var identificacionPaypal = _context.Billeteras.FirstOrDefault(b => b.Usuario.Dni == dni && b.Tipo == "PayPal");
                if (identificacionPaypal == null)
                    return false;

                if (numTarjeta.Length < 16 || numTarjeta.Length > 16)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido))
                {
                    return false;
                }

                // Validar fecha de vencimiento
                if (ahora > fechaVenc)
                {
                    Console.WriteLine($"\n");
                    Console.WriteLine($"error con la fecha esta vencida");
                    return false;
                }

                if (cod > 999 || cod < 000 || cod <= 99)
                {
                    Console.WriteLine($"\n");
                    Console.WriteLine($"Error con el codigo. ");
                    return false;
                }
                var tarjeta = new Tarjeta
                {
                    NumeroTarjeta = numTarjeta,
                    FechaVencimiento = fechaVenc,
                    CodigoSeguridad = cod,
                    Saldo = 10000,
                    IdBilletera = identificacionPaypal.IdBilletera
                };
                _context.Tarjetas.Add(tarjeta);
                _context.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
