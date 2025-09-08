
using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Strategies.Mp;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
namespace EjercicioInterfaces
{
    public class PayPal
    {
        private readonly AppDbContext _context;
       
        public PayPal(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Billetera> CrearCuentaPayPal(int dni)
        {
            if(!validarDNI(dni))
            {
                return new Billetera { Message = "DNI no valido." };
            }

            var usuario = await _context.Usuarios.Include(u=> u.Billeteras).FirstOrDefaultAsync(u => u.Dni == dni);
            if (usuario == null)
            {
                return new Billetera { Message = "Usuario no encontrado." };
            }

            bool tieneCobrador = usuario.Billeteras.Any(b => b.Tipo == "Cobrador");
            if (tieneCobrador)
            {
                return new Billetera { Message = "El usuario es un cobrador y no puede registrar PayPal." };
            }

            bool existeBilletera = usuario.Billeteras.Any(b => b.Tipo == "PayPal");
            if (existeBilletera) {
                return new Billetera { Message = "El usuario ya esta registrado." };
            }

            if (!mailValidar(usuario.Email))
            {
                return new Billetera { Message = "Correo electrónico no válido." };
            }

            var billetera = new Billetera
            {
                IdUsuario = usuario.IdUsuario,
                Tipo = "PayPal",
                Cvu = usuario.Email,
                Saldo = 0.0m
            };
            _context.Billeteras.Add(billetera);
            await _context.SaveChangesAsync();
            return billetera;
        }

        public static bool ValidarNumeroCelular(string numero)
        {
            if (string.IsNullOrEmpty(numero))
            {
                return false;
            }

            string regEspacios = @"[\s\-\(\)]";
            string regNumeros = @"\d{10,12}$";

            if (!Regex.IsMatch(numero, regEspacios))
            {
                return false;
            }
            if (Regex.IsMatch(numero, regNumeros))
            {
                return false;
            }
            return true;
        }

        public static bool mailValidar(string mail)
        {
            string regMail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (string.IsNullOrEmpty(mail))
            {
                return false;
            }

            if (!Regex.IsMatch(mail, regMail))
            {
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
                Console.WriteLine($"Error el dni no puede ser {dni}");
                return false;
            }

            if (!Regex.IsMatch(dniString, expDNI))
            {
                Console.WriteLine($"El DNI '{dni}' no es valido. Debe contener 8 digitos.");
                return false;
            }
            return true;
        }


        public bool RealizarCobro(decimal cobro)
        {
            // Verificar que el cobro no sea negativo y que haya saldo suficiente
            if (cobro < 0)
            {
                return false; // Indica un error
            }

            //if (cobro > tarjeta?.limiteSaldo)
            //{
            //    Console.WriteLine("No hay suficiente saldo para realizar el cobro.");
            //    return true; // saldo insuficiente
            //}

            // restamos el cobro del saldo disponible
            //tarjeta?.limiteSaldo -= cobro; heck

            //Console.WriteLine($"Cobro realizado con éxito. Saldo restante: {tarjeta?.limiteSaldo}");
            return true;
        }

        public static bool validarNombre(string nombre)
        {
            string expNombre = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s'-]{2,50}$";

            if (string.IsNullOrEmpty(nombre))
            {
                return false;
            }

            if (!Regex.IsMatch(nombre, expNombre))
            {
                return false;
            }
            return true;
        }

        public static bool validarApellido(string apellido)
        {
            if (apellido == null)
            {
                return false;
            }

            string expNombre = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s'-]{2,50}$";

            if (!Regex.IsMatch(apellido, expNombre))
            {
                Console.WriteLine($"El nombre '{apellido}' no es válido.");
                return false;
            }
            return true;

        }


        public bool AgregarSaldoPaypal(decimal saldo)
        {
            //if (tarjeta == null)
            //{
            //    Console.WriteLine($"no es posible agregar saldo, no tiene tarjeta asociada. ");
            //    return false;
            //}

            //if (tarjeta.limiteSaldo == 0)
            //{
            //    Console.WriteLine($"Error el limite de su saldo es 0 USD. ");

            //    return false;
            //}

            if (saldo <= 0)
            {
                Console.WriteLine("El saldo a agregar debe ser mayor a cero USD.");
                return false;
            }

            //if (tarjeta.limiteSaldo < saldo)
            //{
            //    Console.WriteLine($"No posee suficiente saldo para esta operacion. ");
            //    return false;
            //}

            //tarjeta.limiteSaldo -= saldo;
            //saldoPayPal += saldo;
            //Console.WriteLine($"Saldo agregado correctamente. Nuevo saldo PayPal: ${saldoPayPal} USD");
            return true;
        }

        public static bool validarNumTarjeta(string cbu)
        {
            if (string.IsNullOrEmpty(cbu))
            {
                Console.WriteLine($"Error debe ingresar el numero de la tarjeta");
                return false;
            }
            string expCbu = @"^\d{16}$";

            if (!Regex.IsMatch(cbu, expCbu))
            {
                Console.WriteLine($"Error al registrar la tarjeta: {cbu}");
                return false;
            }
            return true;
        }


    }
}
