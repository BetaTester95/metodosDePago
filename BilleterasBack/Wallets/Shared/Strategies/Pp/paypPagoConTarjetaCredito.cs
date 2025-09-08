using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Shared.Interfaces;
using EjercicioInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilleterasBack.Wallets.Shared.Strategies.Pp
{
    public class paypPagoConTarjetaCredito : IpagoCardCred
    {
      
        private readonly AppDbContext _context;
        private decimal saldoTarjetaCredito;
        private string? _cvuCobradorSeleccionado;
        private int _idDni;

        public paypPagoConTarjetaCredito(AppDbContext context)
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

        public bool PagoConTarjetaCredito(decimal montoPagar, int cantCuotas)
        {
            var checkCobrador = _context.Billeteras.Include(b => b.Usuario).FirstOrDefault(b => b.Tipo == "Cobrador" && _cvuCobradorSeleccionado == b.Cvu); //revisamos que exista el cobrador
            var checkTarjeta = _context.Tarjetas.FirstOrDefault(t => t.IdTarjeta == _idDni); //revisamos que tengamos una tarjeta asociada

            var checkSaldo = _context.Billeteras.Include(u => u.Usuario).FirstOrDefault(b => b.Usuario.Dni == _idDni);
            saldoTarjetaCredito = checkTarjeta.Saldo;

            if (checkTarjeta.NumeroTarjeta == null)
            {
              return false;
            }
            
            if (checkCobrador == null)
            {
                return false;
            }
            if (checkSaldo == null)
            {
                return false;
            }
            
            if (montoPagar <= 0)
            {
                Console.WriteLine("Error el monto es 0");
                return false;
            }
            if (montoPagar > saldoTarjetaCredito)
            {
                Console.WriteLine("No posee saldo suficiente en la tarjeta");
                return false;
            }

            saldoTarjetaCredito -= montoPagar;
            checkCobrador.Saldo += montoPagar;
            _context.SaveChanges();
            return true;
        }
    }
}
