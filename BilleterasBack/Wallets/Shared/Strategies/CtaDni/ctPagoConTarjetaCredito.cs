using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilleterasBack.Wallets.Shared.Strategies.CtaDni
{
    public class ctPagoConTarjetaCredito : IpagoCardCred
    {
        private readonly AppDbContext _context;
        private string? _cvuCobradorSeleccionado;
        private int _idDni;
        private readonly ILogger<ctPagoConTarjetaCredito> _logger;
        public ctPagoConTarjetaCredito(AppDbContext context, ILogger<ctPagoConTarjetaCredito> logger)
        {
            _context = context;
            _logger = logger;
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

        public bool PagoConTarjetaCredito(decimal montoPagar, int cantCuotas = 0)
        {
            string cvuCobrador = "0046922191583351343977";
            int idDni = 12345678;

            var billeteraCobrador = _context.Billeteras.Include(b => b.Usuario)
                .FirstOrDefault(b => b.Tipo == "Cobrador" && b.Cvu == cvuCobrador);
            if (billeteraCobrador == null || billeteraCobrador.Tipo == null)
            {
                _logger.LogWarning("El cobrador con CVU: {cvuCobrador} no existe.", cvuCobrador);
                return false;
            }

            var tarjetaUsuario = _context.Tarjetas.Include(t => t.Billetera)
                .ThenInclude(b => b.Usuario)
                .FirstOrDefault(t => t.Billetera.Usuario.Dni == idDni && t.Billetera.Tipo == "CuentaDni");

            if (tarjetaUsuario == null)
            {
                _logger.LogWarning("La tarjeta asociada al DNI: {idDni} no existe.", idDni);
                return false;
            }

            var billeteraUsuario = _context.Billeteras.Include(u => u.Usuario)
                .FirstOrDefault(b => b.Usuario.Dni == idDni && b.Tipo == "CuentaDni");
            if (billeteraUsuario == null)
                return false;

            if (montoPagar > tarjetaUsuario.Saldo)
            {
                _logger.LogWarning("Saldo insuficiente en Cuenta DNI.");
                return false;
            }

            if (montoPagar <= 0)
                return false;

            tarjetaUsuario.Saldo -= montoPagar;
            billeteraUsuario.Saldo += montoPagar;
            _context.SaveChanges();
            return true;
        }
    }
}
