using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EjercicioInterfaces
{
    public class CuentaDni
    {

        private readonly AppDbContext _context;
        public CuentaDni(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Billetera> CrearCuentaDni(Usuario usuario)
        {
            if(usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
            }

            var billetera = new Billetera
            {
                IdUsuario = usuario.IdUsuario,
                Tipo = "CuentaDni",
                Cvu = GenerarNumeroCvu(),
                Saldo = 0.0m
            };

            _context.Billeteras.Add(billetera);
            await _context.SaveChangesAsync();
            return billetera;
        }
     
        public string GenerarNumeroTarjeta()
        {
            Random random = new Random();
            string numero = "";

            for (int i = 0; i < 16; i++)
            {
                numero += random.Next(0, 10).ToString();
            }

            return numero;
        }
        public string GenerarNumeroCvu()
        {
            Random random = new Random();
            string numero = "";

            for (int i = 0; i < 22; i++)
            {
                numero += random.Next(0, 10).ToString();
            }

            return numero;
        }

        public bool EsFechaVencimientoValida(DateTime fecha)
        {
            DateTime ahora = DateTime.Now;
            return fecha > ahora;
        }

        //public decimal agregarSaldo(decimal saldo)
        //{
        //    //tarjetaV!.limiteSaldo -= saldo;
        //    return this.saldoCuentaDni += saldo;
        //}

        public static bool validarNombre(string nombre)
        {
            string expNombre = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s'-]{2,50}$";

            if (nombre == null)
            {
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

            if (apellido == null)
            {

                return false;
            }

            if (!Regex.IsMatch(apellido, expNombre))
            {
                Console.WriteLine($"El nombre '{apellido}' no es válido.");
                return false;
            }
            return true;

        }

        public string CrearTarjetaVirtual()
        {
            string numeroGenerado = GenerarNumeroTarjeta();
            //tarjetaV = new TarjetaVirtual(this.nombre, this.apellido, this.dni, numeroGenerado);
            //Console.WriteLine($"Tarjeta virtual creada exitosamente: {tarjetaV.numeroTarjetaVirtual}");
            return numeroGenerado;
        }
        public static bool validarDNI(int dni)
        {

            string dniString = dni.ToString();
            string expDNI = @"^\d{8}$";



            if (!Regex.IsMatch(dniString, expDNI))
            {
                Console.WriteLine($"El DNI '{dni}' no es valido. Debe contener 8 digitos.");
                return false;
            }
            return true;
        }

        public static bool validarCod(int cod)
        {

            string codString = cod.ToString();
            string expCod = @"^\d{3}$";

            if (!Regex.IsMatch(codString, expCod))
            {
                Console.WriteLine($"El cod de seguridad: '{cod}' no es valido. Debe contener 3 digitos.");
                return false;
            }
            return true;
        }

        public static bool valudarNumero(string cbu)
        {
            string cbuString = cbu.ToString();
            string expCBU = @"^\d{22}$";

            if (String.IsNullOrEmpty(cbu))
            {
                return false;
            }

            if (!Regex.IsMatch(cbuString, expCBU))
            {
                Console.WriteLine($"El numero de tarjeta '{cbu}' no es valido.");

                return false;
            }
            return true;
        }
    }
}
