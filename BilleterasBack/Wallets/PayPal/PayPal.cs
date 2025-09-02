
using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Models;
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


        public async Task<Billetera> CrearCuentaPayPal(Usuario usuario)
        {
            if(usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
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
                Console.WriteLine($"\n");
                Console.WriteLine($"Error al validar el numero de celular");
                return false;
            }

            string regEspacios = @"[\s\-\(\)]";
            string regNumeros = @"\d{10,12}$";


            if (!Regex.IsMatch(numero, regEspacios))
            {
                Console.WriteLine($"Error con el numero: {numero}");
                return false;
            }
            if (Regex.IsMatch(numero, regNumeros))
            {
                Console.WriteLine($"Error con el numero: {numero}");
                return false;
            }
            return true;
        }

        public static bool mailValidar(string mail)
        {
            string regMail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (string.IsNullOrEmpty(mail))
            {
                Console.WriteLine($"Debe ingresar un mail. ");
                return false;
            }

            if (!Regex.IsMatch(mail, regMail))
            {
                Console.WriteLine($"Error al validar el mail. ");
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
