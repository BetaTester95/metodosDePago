using BilleterasBack.Wallets.Cards;
using BilleterasBack.Wallets.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioInterfaces.Estrategias.ctdEstrategias
{
    public class ctAgregarTarjeta : IAgregarCard
    {
        public CuentaDni ctdni;
        public Tarjeta? tarjeta;

        public ctAgregarTarjeta(CuentaDni ctaDni)
        {
            ctdni = ctaDni;
        }

        public bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
            DateTime ahora = DateTime.Now;
            if (!numTarjeta.StartsWith("5195")) //con startwith comprobamos que la cadena empiece con el dato que pasamos por parametro.
            {
                Console.WriteLine($"El {numTarjeta} no corresponde al banco ");
                return false;
            }

            if (numTarjeta.Length != 22)
            {
                Console.WriteLine($"Error con el num tarjeta debe tener un total de 22 digitos");
                return false;
            }

            // Verificar nombre y apellido
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido))
            {
                return false;
            }
            if (ctdni.nombre != nombre || ctdni.apellido != apellido || ctdni.dni != dni)
            {
                Console.WriteLine($"Error con el nombre, apellido o dni. ");
                return false;
            }
            // Validar fecha de vencimiento
            if (fechaVenc < ahora)
            {
                Console.WriteLine($"Error con la fecha de vencimiento: {fechaVenc}");
                return false;
            }

            // Validar codigo de seguridad 
            if (cod > 999 || cod < 000 || cod <= 99)
            {
                return false;
            }

            tarjeta = new Tarjeta(numTarjeta, nombre, apellido, dni, fechaVenc, cod);
            ctdni.tarjeta = tarjeta;
            Console.WriteLine($"Se agrego la tarjeta {ctdni.tarjeta.numeroTarjeta}");
            return true;
        }


    }
}
