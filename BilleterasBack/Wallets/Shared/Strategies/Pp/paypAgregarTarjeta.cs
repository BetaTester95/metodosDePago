using BilleterasBack.Wallets.Cards;
using BilleterasBack.Wallets.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioInterfaces.Estrategias.ppEstrategias
{
    public class paypAgregarTarjeta : IAgregarCard
    {
        public PayPal ppal;
        public Tarjeta? tarjeta;


        public paypAgregarTarjeta(PayPal payPal)
        {
            ppal = payPal;
        }

        public bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
            DateTime ahora = DateTime.Now;

            if (numTarjeta.Length < 16 || numTarjeta.Length > 16)
            {
                Console.WriteLine($"Error no coincide el numero de tarjeta");
                return false;
            }

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido))
            {
                Console.WriteLine($"\n");
                Console.WriteLine($"No se puede pasar nombres o apellido vacios. ");
                return false;
            }
            if (ppal.nombre != nombre || ppal.apellido != apellido)
            {
                Console.WriteLine($"\n");
                Console.WriteLine($"Error los datos de la cuenta paypal no coincide con el titular de la cuenta. ");
                return false;
            }

            // Validar fecha de vencimiento
            if (ahora > fechaVenc)
            {
                Console.WriteLine($"\n");
                Console.WriteLine($"error con la fecha esta vencida");
                return false;
            }

            if (cod > 999 || cod < 000 || cod <= 99)
            {
                Console.WriteLine($"\n");
                Console.WriteLine($"Error con el codigo. ");
                return false;
            }
            tarjeta = new Tarjeta(numTarjeta, nombre, apellido, dni, fechaVenc, cod);
            ppal.tarjeta = tarjeta;

            bool cobroExitoso = ppal.RealizarCobro(1);

            if (!cobroExitoso)
            {
                return false;
            }
            Console.WriteLine($"\n");
            Console.WriteLine($"******************************");
            Console.WriteLine($"===== REGISTRO EXITOSO =====.");
            Console.WriteLine($"******************************");
            Console.WriteLine($"\n");
            Console.WriteLine($"Por politicas de PayPal se cobro $1.00 USD de su tarjeta por haberla registrado. {tarjeta.limiteSaldo}");
            Console.WriteLine($"Se registro la tarjeta con N°: {tarjeta.numeroTarjeta}");
            Console.WriteLine($"\n");
            return true;
        }


    }
}
