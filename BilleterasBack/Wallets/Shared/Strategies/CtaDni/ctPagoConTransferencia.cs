using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioInterfaces.Estrategias.ctdEstrategias
{
    public class ctPagoConTransferencia /*: IPagoCardTransferencia*/
    {
        private readonly AppDbContext _context;
        private string? _cvuCobradorSeleccionado;
        private int _idDni;
        private decimal descuentoTotal;
        private decimal descuentoLyM = 0.15m;
        private decimal descuentoS = 0.20m;
        public ctPagoConTransferencia(AppDbContext context)
        {
          _context = context;
        }

        public string CvuCobradorSeleccionado(string cvu)
        {
            _cvuCobradorSeleccionado = cvu;
            return _cvuCobradorSeleccionado;
        }

        public int identificarTarjeta(int dni)
        {
            _idDni = dni;
            return _idDni;
        }

        public bool PagoConTransferencia(decimal montoPagar, string cbu)
        {
            string? cvuCobrador = _cvuCobradorSeleccionado;
            int idDni = _idDni;
            var checkCobrador = _context.Billeteras.Include(b => b.Usuario).FirstOrDefault(b => b.Tipo == "Cobrador" && cvuCobrador == b.Cvu); //revisamos que exista el cobrador
            var checkTarjeta = _context.Tarjetas.Include(t => t.Billetera).ThenInclude(b => b.Usuario).FirstOrDefault(t => t.Billetera.Usuario.Dni == idDni); //revisamos que exista la tarjeta
            var checkSaldo = _context.Billeteras.Include(u => u.Usuario).FirstOrDefault(b => b.Usuario.Dni == idDni);
            decimal saldo = checkSaldo.Saldo;

            if (checkCobrador == null)
                return false;

            string? cbuCobrador = checkCobrador.Cvu;

            DateTime hoy = DateTime.Now;
            var region = new CultureInfo("es-ES");
            string diaSemana = hoy.ToString("dddd", region).ToLower();


            if (saldo < montoPagar)
            {
                Console.WriteLine("Saldo insuficiente en Cuenta DNI.");
                return false;
            }

            if (diaSemana == "lunes" || diaSemana == "miércoles")
            {
                descuentoTotal = montoPagar - (descuentoLyM * montoPagar);
                saldo -= descuentoTotal;
                checkSaldo.Saldo = saldo;
                checkCobrador.Saldo += descuentoTotal;
                _context.SaveChanges();              
                /*Console.WriteLine($"Resultado del monto a pagar con descuento aplicado: {descuentoTotal}");
                Console.WriteLine($"Le quedo un saldo de : {ctdni.saldoCuentaDni}");
                Console.WriteLine("Descuento del 15% aplicado por ser Lunes o Miércoles.");*/
                return true;
            }
            else if (diaSemana == "sábado")
            {
                descuentoTotal = montoPagar - (descuentoS * montoPagar);
                saldo -= descuentoTotal;
                checkSaldo.Saldo = saldo;
                checkCobrador.Saldo += descuentoTotal;
                _context.SaveChanges();
                /*Console.WriteLine($"Resultado del monto a pagar con descuento aplicado: {descuentoTotal}");
                Console.WriteLine($"Le quedo un saldo de : {ctdni.saldoCuentaDni}");
                Console.WriteLine("Descuento del 20% aplicado por ser sábado.");*/
                return true;
            }

            if (montoPagar >= saldo)
            {
                saldo -= montoPagar;
                checkSaldo.Saldo = saldo;
                checkCobrador.Saldo += montoPagar;
                _context.SaveChanges();
               return true;
           }
           return false;
        }
    }
}
