using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BilleterasBack.Wallets.Shared.Strategies.Mp
{

    public class MpPagoConTarjetaCredito : IpagoCardCred
    {
        private readonly AppDbContext _context;
        private string? _cvuCobradorSeleccionado;
        private readonly ILogger<MpPagoConTarjetaCredito> _logger;
        private int _idDni;
        private decimal resultado;
        public MpPagoConTarjetaCredito(AppDbContext context, ILogger<MpPagoConTarjetaCredito> logger)
        {
            _context = context;
            _logger = logger;
        }
  

        public bool PagoConTarjetaCredito(decimal montoPagar, int cantCuotas)
        {
            _logger.LogInformation("=== INICIO DEBUG ===");
            _logger.LogInformation($"Monto: {montoPagar}, Cuotas: {cantCuotas}");

            string cvuCobrador = "0046922191583351343977";
            int idDni = 12345678; 

            var billeteraCobrador = _context.Billeteras.Include(b=> b.Usuario).FirstOrDefault(b => b.Tipo =="Cobrador" && cvuCobrador == b.Cvu); //revisamos que exista el cobrador
            var tarjetaUsuario = _context.Tarjetas.Include(t => t.Billetera).ThenInclude(t => t.Usuario).FirstOrDefault(t=>t.Billetera.Usuario.Dni == idDni && t.Billetera.Tipo == "MercadoPago");//revisamos que tengamos una tarjeta asociada
            var billeteraUsuario = _context.Billeteras.Include(u => u.Usuario).FirstOrDefault(b => b.Usuario.Dni == idDni && b.Tipo == "MercadoPago");  

            if (billeteraCobrador == null)
            {
                _logger.LogWarning("El cobrador con CVU: {cvuCobrador} no existe.", cvuCobrador);
                return false;
            }

            if (tarjetaUsuario == null)
            {
                _logger.LogWarning("La tarjeta asociada al DNI: {idDni} no existe.", idDni);
                return false;
            }
            if (billeteraUsuario == null)
            {
                _logger.LogWarning("La billetera asociada al DNI: {idDni} no existe.", idDni);
                return false;
            }

            if (montoPagar <= 0)
            {
                return false;
            }

            if (montoPagar > tarjetaUsuario.Saldo)
            {
                return false;
            }
            if (cantCuotas > 12)
            {
                return false;
            }

            if (cantCuotas == 1)
            {
                billeteraUsuario.Saldo -= montoPagar;             
                billeteraCobrador.Saldo += montoPagar;
                _context.SaveChanges();
                return true;
            }
            if (cantCuotas == 3)
            {
                resultado = montoPagar + (0.15m * montoPagar);
                billeteraUsuario.Saldo -= resultado;
                billeteraCobrador.Saldo += resultado;
                _context.SaveChanges();
                return true;
            }

            if (cantCuotas == 6)
            {
                resultado = montoPagar + (0.20m * montoPagar);

                billeteraUsuario.Saldo -= resultado;
                billeteraCobrador.Saldo += resultado;
                _context.SaveChanges();

                return true;
            }
            if (cantCuotas == 12)
            {
                resultado = montoPagar + (0.25m * montoPagar);
                billeteraUsuario.Saldo -= resultado;
                billeteraCobrador.Saldo += resultado;
                _context.SaveChanges();
                return true;

            }
            return false;
        }
        
    }
}
