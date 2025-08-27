using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EjercicioInterfaces
{
    public class Tarjeta
    {
        public string nombreTitular;
        public string apellidoTitular;
        public string numeroTarjeta;
        public int dniTitular;
        public DateTime fechaVencimiento;
        public int codigoSeguridad;
        public decimal limiteSaldo = 10000.00m;

        public Tarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
            numeroTarjeta = numTarjeta;
            nombreTitular = nombre;
            apellidoTitular = apellido;
            dniTitular = dni;
            fechaVencimiento = fechaVenc;
            codigoSeguridad = cod;
        }

        public static bool validarTarjeta(string numTarjeta)
        {
            string expTarjeta = @"^\d{16}$";
            if (string.IsNullOrEmpty(numTarjeta))
            {
                Console.WriteLine($"Debe ingresar los numeros de la tarjeta. ");
                return false;
            }
            if (!Regex.IsMatch(numTarjeta, expTarjeta))
            {
                Console.WriteLine($"Error al registrar la tarjeta. ");
                return false;
            }
            return true;
        }

        //traigo el saldoMaximo en la tarjeta
        public decimal SaldoLimite()
        {
            return limiteSaldo;
        }
        public string devolverTarjeta()
        {
            return numeroTarjeta;
        }

    }
}
