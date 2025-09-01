using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BilleterasBack.Wallets.Shared.Strategies.Mp
{
    public class MpAgregarTarjeta : IAgregarCard
    {
        private readonly AppDbContext _context;

        public MpAgregarTarjeta(AppDbContext context)
        {
            _context = context;
        }

        public bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
            DateTime ahora = DateTime.Now;

            // Validaciones
            //if (numTarjeta.Length != 16)
            //{
            //    Console.WriteLine($"Error: el número de tarjeta {numTarjeta} debe tener 16 dígitos.");
            //    return false;
            //}

            //if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido))
            //{
            //    Console.WriteLine("Error: nombre o apellido inválidos.");
            //    return false;
            //}

            //if (dni.ToString().Length < 8)
            //{
            //    Console.WriteLine("Error: el DNI debe tener al menos 8 dígitos.");
            //    return false;
            //}

            //if (fechaVenc < ahora)
            //{
            //    Console.WriteLine("Error: la fecha de vencimiento no puede ser menor a hoy.");
            //    return false;
            //}

            //if (cod < 100 || cod > 999)
            //{
            //    Console.WriteLine($"Error: el código de seguridad {cod} no es válido.");
            //    return false;
            //}

            // Crear entidad

            var cuentaMP = _context.Mp.FirstOrDefault(mp => mp.dni == dni);
            string tipoCuenta = cuentaMP != null ? "MercadoPago" : "Desconocido";

            var tarjeta = new TarjetaEntity
            {
                numeroTarjeta = numTarjeta,
                nombreTitular = nombre,
                apellidoTitular = apellido,
                dniTitular = dni,
                fechaVencimiento = fechaVenc,
                cod = cod,
                tipo_cuenta = tipoCuenta
            };

            try
            {
                _context.Tarjetas.Add(tarjeta);
                _context.SaveChanges();

                Console.WriteLine("========== TARJETA AGREGADA CORRECTAMENTE ==========");
                Console.WriteLine($"Nombre: {tarjeta.nombreTitular} Apellido: {tarjeta.apellidoTitular}");
                Console.WriteLine($"Tarjeta N°: {tarjeta.numeroTarjeta}");
                Console.WriteLine("===================================================");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar la tarjeta en la base de datos: {ex.Message}");
                return false;
            }
        }
    }

}
