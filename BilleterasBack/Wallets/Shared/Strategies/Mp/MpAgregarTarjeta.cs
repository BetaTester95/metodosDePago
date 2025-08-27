using EjercicioInterfaces.Pagos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EjercicioInterfaces;

namespace EjercicioInterfaces.Estrategias.MercadoPago
{
    public class MpAgregarTarjeta : IAgregarCard
    {
        public Tarjeta? tarjetaVariable {get;set;}
        public Cobrador cobrador;
        public AppMp mp { get; set; }

        public MpAgregarTarjeta(AppMp cuenta, Cobrador cobrador)
        {
            this.mp = cuenta;
            this.cobrador = cobrador;
        }

        public bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
            DateTime ahora = DateTime.Now;

            if (numTarjeta.Length > 16 || numTarjeta.Length < 16)
            {
                Console.WriteLine($"error con el numero de la tarjeta {numTarjeta} debe tener 16 digitos. ");
                return false;
            }

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido))
            {
                Console.WriteLine("el nombre o apellido son invalidos");
                return false;
            }

            if (dni < 8)
            {
                Console.WriteLine($"EL dni es menor a 8 digitos");
                return false;
            }

            if (fechaVenc < ahora)
            {
                Console.WriteLine("Error con la fecha");
                return false;
            }

            if (cod > 999 || cod < 000 || cod <= 99)
            {
                Console.WriteLine($"Error con el codigo de seguridad {cod}.");
                return false;
            }

            tarjetaVariable = new Tarjeta(numTarjeta, nombre, apellido, dni, fechaVenc, cod);
            mp.tarjeta = tarjetaVariable;


            Console.WriteLine($"==========TARJETA AGREGADA CORRECTAMENTE==========");
            Console.WriteLine($"\n");
            Console.WriteLine($"Nombre: {tarjetaVariable.nombreTitular} Apellido: {tarjetaVariable.apellidoTitular}");
            Console.WriteLine($"Tarjeta N°: {tarjetaVariable.devolverTarjeta()}");
            Console.WriteLine($"\n");
            Console.WriteLine($"==========TARJETA AGREGADA CORRECTAMENTE==========");
            return true;

        }

    }
}
