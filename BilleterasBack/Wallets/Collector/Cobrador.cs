using BilleterasBack.Wallets.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BilleterasBack.Wallets.Collector.Cobrador
{   
        public class Cobrador
        {
            public string nombre;
            public string apellido;
            public int dni;
            public string mailCobrador;
            public decimal saldoCobrador = 0.00m;
            public string cbu;
            private readonly AppDbContext _context;

        public Cobrador(AppDbContext context)
        {    
            _context = context;
        }


        public async Task<Billetera?> CrearCuentaCobrador(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
            }

            if (usuario.IdTipoUsuario != 2)
                throw new Exception("Un usuario de tipo Cliente no puede crear una billetera Cobrador.");

            bool existeCobrador = usuario.Billeteras.Any(b => b.Tipo == "Cobrador");
            if (existeCobrador)
                throw new Exception("El usuario ya tiene una billetera de tipo Cobrador.");

            var billetera = new Billetera
            {
                IdUsuario = usuario.IdUsuario,
                Tipo = "Cobrador",
                Cvu = GenerarNumeroCbu(),
                Saldo = 0.0m
            };
            await _context.Billeteras.AddAsync(billetera);
            await _context.SaveChangesAsync();
            return billetera;
        }

        public decimal retornarSaldo()
            {
                return saldoCobrador;
            }

            public string retornarMail()
            {
                return mailCobrador;
            }

            public decimal cobrarMonto(decimal monto)
            {
                return saldoCobrador += monto;
            }

            public void mostrarSaldo()
            {
                Console.WriteLine($"El saldo de la persona que cobra es de: {saldoCobrador}");
            }

            public string retornarCbuCobrador()
            {
                return cbu;
            }
            public static bool validarMail(string mail)
            {
                string expMail = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

                if (string.IsNullOrEmpty(mail))
                {
                    return false;
                }

                if (!Regex.IsMatch(mail, expMail))
                {
                    return false;
                }

                return true;
            }

            public static bool validarNombre(string nombre)
            {
                string expNombre = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s'-]{2,50}$";

                if (string.IsNullOrEmpty(nombre))
                {
                    Console.WriteLine($"Debe ingresar un nombre. ");
                    return false;
                }

                if (!Regex.IsMatch(nombre, expNombre))
                {
                    Console.WriteLine($"El nombre '{nombre}' no es válido.");
                    return false;
                }
                return true;

            }

            public static bool validarApellido(string apellido)
            {
                string expNombre = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s'-]{2,50}$";
                if (string.IsNullOrEmpty(apellido))
                {
                    Console.WriteLine($"Debe ingresar el apellido. ");
                    return false;
                }

                if (!Regex.IsMatch(apellido, expNombre))
                {
                    Console.WriteLine($"El nombre '{apellido}' no es válido.");
                    return false;
                }
                return true;

            }

            public static bool validarDNI(int dni)
            {
                string dniString = dni.ToString();
                string expDNI = @"^\d{8}$";

                if (dni == 0)
                {
                    return false;
                }

                if (!Regex.IsMatch(dniString, expDNI))
                {
                    return false;
                }
                return true;
            }

            public static bool validarCbuCobrador(string cbu)
            {
                if (cbu == "")
                {
                    Console.WriteLine($"Error el cbu esta vacio");
                    return false;
                }
                string expCbu = @"^\d{22}$";

                if (!Regex.IsMatch(cbu, expCbu))
                {
                    Console.WriteLine($"Error con el cbu: {cbu}");

                    return false;
                }
                return true;
            }
            public string GenerarNumeroCbu()
            {
                Random random = new Random();
                string numero = "";

                for (int i = 0; i < 22; i++)
                {
                    numero += random.Next(0, 10).ToString();
                }

                return numero;
            }
        }
}
