using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioInterfaces
{
    public class TarjetaVirtual
    {
        public string nombreTitular;
        public string apellidoTitular;
        public int dniTitular;
        public string numeroTarjetaVirtual;
        public decimal limiteSaldo = 10000m;
        public TarjetaVirtual(string nombre, string apellido, int dni, string numTarjetaVirtual)
        {
            nombreTitular = nombre;
            apellidoTitular = apellido;
            dniTitular = dni;
            numeroTarjetaVirtual = numTarjetaVirtual;
        }

        public decimal saldo()
        {
            return limiteSaldo;
        }

    }
}
