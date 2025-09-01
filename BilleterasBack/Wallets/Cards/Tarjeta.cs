using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BilleterasBack.Wallets.Cards
{
    public class Tarjeta
    {
        public int id_tarjeta { get; set; }  // PK
        public int? id_usuario { get; set; } // FK

        public string nombreTitular { get; set; }
        public string apellidoTitular { get; set; }
        public string numeroTarjeta { get; set; }
        public int dniTitular { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public int cod { get; set; }
        public decimal limiteSaldo { get; set; } = 10000.00m;

        public bool activo { get; set; } = true;
        public DateTime fecha_creacion { get; set; } = DateTime.Now;

        public Tarjeta() { }

        public Tarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
            numeroTarjeta = numTarjeta;
            nombreTitular = nombre;
            apellidoTitular = apellido;
            dniTitular = dni;
            fechaVencimiento = fechaVenc;
            this.cod = cod;
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
