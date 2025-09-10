using Azure.Core;
using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BilleterasBack.Wallets.Shared.Strategies.Mp
{
    public class MpAgregarTarjeta : IAgregarCard
    {
        private readonly AppDbContext _context;
        //agregar logger
        private readonly ILogger<MpAgregarTarjeta> _logger;
        public MpAgregarTarjeta(AppDbContext context, ILogger<MpAgregarTarjeta> logger)
        {
           _context = context;
           _logger = logger;
        }

        public bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
           try
           {  
                //buscar dni de usuario
                var usuario = _context.Usuarios.FirstOrDefault(u => u.Dni == dni); 
                var billetera = _context.Billeteras
                    .Include(b => b.Usuario)
                    .FirstOrDefault(b => b.Usuario.Dni == dni && b.Tipo == "MercadoPago");  
                if (billetera == null) {

                    _logger.LogWarning(message: "DEBUG: Billetera no encontrada para usuario:{dni} ", dni);
                    return false;
                }
                _logger.LogInformation("DEBUG: Billetera encontrada, creando tarjeta...");

                var tarjeta = new Tarjeta
                {
                    NumeroTarjeta = numTarjeta,
                    FechaVencimiento = fechaVenc,
                    CodigoSeguridad = cod,
                    Saldo = 10000,
                    IdBilletera = billetera.IdBilletera
                };
                _context.Tarjetas.Add(tarjeta);
                _context.SaveChanges();
               return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "ERROR: Falló al guardar la tarjeta");
                return false;
            }
        }
    }
}