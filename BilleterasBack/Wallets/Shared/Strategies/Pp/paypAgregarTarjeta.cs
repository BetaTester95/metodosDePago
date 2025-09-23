using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Exceptions;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Interfaces;
using BilleterasBack.Wallets.Validaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilleterasBack.Wallets.Shared.Strategies.Pp
{
    public class paypAgregarTarjeta : IAgregarCard
    {
        private readonly AppDbContext _context;
        
        public paypAgregarTarjeta(AppDbContext context)
        {
            _context = context;           
        }

        public bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
              
                DateTime ahora = DateTime.Now;

                var identificacionPaypal = _context.Billeteras
                   .Include(b => b.Usuario)
                   .FirstOrDefault(b => b.Tipo == "PayPal" && b.Usuario.Dni == dni);

                if(identificacionPaypal == null)
                    throw new UsuarioExceptions("No se encontró una billetera PayPal para el usuario.");
                

                var tarjetaExistente = _context.Tarjetas
                    .FirstOrDefault(t => t.NumeroTarjeta == numTarjeta && t.IdBilletera == identificacionPaypal.IdBilletera);

                if (tarjetaExistente != null)              
                    throw new BilleteraExceptions("Ya existe una tarjeta con este número en la billetera de PayPal.");
                
                if (ahora > fechaVenc)   
                    return false;
                
                var tarjeta = new Tarjeta
                {
                    NumeroTarjeta = numTarjeta,
                    FechaVencimiento = fechaVenc,
                    CodigoSeguridad = cod,
                    Saldo = 10000,
                    IdBilletera = identificacionPaypal.IdBilletera
                };
                _context.Tarjetas.Add(tarjeta);
                _context.SaveChanges();

                return true;
                  
        }
    }
}
