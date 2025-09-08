using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Shared.Interfaces;
using EjercicioInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilleterasBack.Wallets.Shared.Strategies.Mp
{
    public class MpPagoConTransferencia : IPagoCardTransferencia
    {
        private readonly AppDbContext _context;
        private string? _cvuCobradorSeleccionado;
        private int _idDni;

        public MpPagoConTransferencia(AppDbContext context)
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
            var checkCobrador = _context.Billeteras.FirstOrDefault(b => b.Tipo == "Cobrador" && b.Cvu == cbu);
            var checkMercadoPago = _context.Billeteras.Include(b => b.Usuario).FirstOrDefault(b => b.Tipo == "MercadoPago" && b.Usuario.Dni == idDni);
            var checkSaldo = _context.Billeteras.Include(u => u.Usuario).FirstOrDefault(b => b.Usuario.Dni == idDni);

            if(checkCobrador == null || checkCobrador.Cvu == null)
            {                
               return false;
            }

            if(checkCobrador.Cvu == cbu)
            {
               decimal saldoMp = checkMercadoPago.Saldo;
               saldoMp -= montoPagar;
               checkCobrador.Saldo += montoPagar;
               _context.SaveChanges();
                return true;
            }
            else
            {
               return false;
            }
        }
    }
}