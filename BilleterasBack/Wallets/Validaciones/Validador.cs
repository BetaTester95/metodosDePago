using BilleterasBack.Wallets.Shared;
using System.Text.RegularExpressions;

namespace BilleterasBack.Wallets.Validaciones
{
    public class Validador
    {
        public Validador() { }

        public bool validarNombre(string? nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return false;
            }
            string expNombre = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s'-]{2,50}$";

            if (!Regex.IsMatch(nombre, expNombre))
            {
                Console.WriteLine($"El nombre '{nombre}' no es válido.");
                return false;
            }
            return true;
        }

        public bool validarApellido(string? apellido)
        {
            string expNombre = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s'-]{2,50}$";

            if(string.IsNullOrEmpty(apellido))
            {
                return false;
            }   

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

        public bool validarDNI(int? dni)
        {
            if (dni == null)
            {
                return false;
            }

            string dniString = dni.Value.ToString();
            string expDNI = @"^\d{8}$";
            if (!Regex.IsMatch(dniString, expDNI))
            {
                Console.WriteLine($"El DNI '{dni}' no es valido. Debe contener 8 digitos.");
                return false;
            }
            return true;
        }


        public bool validarCod(int cod)
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


        public bool validarNumeroCbu(string cbu)
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
        public bool EsFechaVencimientoValida(DateTime fecha)
        {
            DateTime ahora = DateTime.Now.Date;
            return fecha > ahora;
        }

        public bool validarNumTarjeta(string cbu)
        {
            if (string.IsNullOrEmpty(cbu))
            {
                return false;
            }

            string expCbu = @"^\d{16,22}$";

            if (!Regex.IsMatch(cbu, expCbu))
            {
                return false;
            }
            return true;
        }

        public bool ValidarNumeroCelular(string numero)
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

        public bool mailValidar(string mail)
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

        public bool ValidarMonto(decimal monto)
        {
            // 1️⃣ Verificar que sea mayor a 0
            if (monto <= 0)
                return false;

            // 3️⃣ Opcional: evitar que sea un número "extraño" (muy grande)
            if (monto > 1000000) 
                return false;

            return true;
        }

        public bool ValidarTipoMetodoPago(string input)
        {
            // Intenta convertir el string al enum
            return Enum.TryParse<TipoMetodoPago>(input, ignoreCase: true, out TipoMetodoPago resultado)
                   && Enum.IsDefined(typeof(TipoMetodoPago), resultado);
        }


    }
}
