using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilleterasBack.Wallets.Shared.Strategies.Pp
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
            //readonly logger
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<paypAgregarTarjeta>();

            if (numTarjeta.Length != 16)
            {
                logger.LogError("El numero de tarjeta debe tener 16 digitos.");
                return false;
            }

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido))
            {
                logger.LogError("El nombre y apellido no pueden estar vacios.");
                return false;
            }

            if (dni < 1000000 || dni > 99999999)
            {
                logger.LogError("El DNI debe tener entre 7 y 8 digitos.");
                return false;
            }

            try
            {
                DateTime ahora = DateTime.Now;

                var identificacionPaypal = _context.Billeteras.Where(b => b.Tipo == "PayPal" && b.Usuario.Dni == dni)
                    .FirstOrDefault();
                if (identificacionPaypal == null || identificacionPaypal.Tipo == null) {
                    logger.LogError("Billetera no encontrada para el usuario con DNI: {dni}", dni);
                    return false;
                }
      
                if (ahora > fechaVenc)   
                    return false;
                

                if (cod > 999 || cod < 100) 
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
