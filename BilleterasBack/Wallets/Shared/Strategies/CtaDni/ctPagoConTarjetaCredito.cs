using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioInterfaces.Estrategias.ctdEstrategias
{
    public class ctPagoConTarjetaCredito : IpagoCardCred
    {
        private readonly AppDbContext _context;
        private string? _cvuCobradorSeleccionado;
        private int _idDni;
        public ctPagoConTarjetaCredito(AppDbContext context)
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
            string cvuCobrador = _cvuCobradorSeleccionado;
            int idDni = _idDni;

            var checkCobrador = _context.Billeteras.Include(b => b.Usuario).FirstOrDefault(b => b.Tipo == "Cobrador" && cvuCobrador == b.Cvu); //revisamos que exista el cobrador
            var checkTarjeta = _context.Tarjetas.Include(t => t.Billetera).ThenInclude(b => b.Usuario).FirstOrDefault(t => t.Billetera.Usuario.Dni == idDni); //revisamos que exista la tarjeta
            var checkSaldo = _context.Billeteras.Include(u => u.Usuario).FirstOrDefault(b => b.Usuario.Dni == idDni);
            decimal saldo = checkSaldo.Saldo;

            if (checkCobrador == null)
              return false;
           
            if(checkCobrador.Cvu == null)            
              return false;
           

            if (checkTarjeta.NumeroTarjeta == null)          
              return false;
            
            if (checkTarjeta.Saldo < montoPagar)
                return false;

            //if (accountDni.tarjetaV == null || accountDni.tarjetaV.numeroTarjetaVirtual == null)
            //{
            //    Console.WriteLine("No hay una tarjeta cargada para poder pagar.");
            //    return false;
            //}

            if(saldo < montoPagar)
            return false;

            if (saldo > montoPagar)
            {
                saldo -= montoPagar;
                checkSaldo.Saldo = saldo;
                checkCobrador.Saldo += montoPagar;
                _context.SaveChanges();
            }

            //decimal saldoTarjetaCredito = accountDni.tarjetaV.saldo();

            //if (saldoTarjetaCredito < montoPagar)
            //{
            //    Console.WriteLine("Saldo insuficiente en la tarjeta de credito virtual.");
            //    return false;
            //}

            //var tarjetaVirtual = tarjetaV!.limiteSaldo -= montoPagar;
            //accountDni.tarjetaV.limiteSaldo = saldoTarjetaCredito;

            //cobrador.cobrarMonto(montoPagar);
            //Console.WriteLine($"¡Se realizó el pago!");

            return true;
        }
    }
}
