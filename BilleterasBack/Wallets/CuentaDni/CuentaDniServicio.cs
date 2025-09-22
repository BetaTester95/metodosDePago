using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Strategies.Pp;
using BilleterasBack.Wallets.Validaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BilleterasBack.Wallets.CuentaDni
{

    public class CuentaDniServicio
    {
        private readonly AppDbContext _context;
        private readonly Validador _validador = new Validador();
        public CuentaDniServicio(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Resultado<Billetera>> CrearCuentaDni(int dni)
        {            
            if (!_validador.validarDNI(dni))
            return Resultado<Billetera>.Failure("DNI debe ser mayor que cero y hasta 8 digitos");

            var usuario = await _context.Usuarios.Include(u => u.Billeteras)
                                                 .FirstOrDefaultAsync(u => u.Dni == dni);
            if (usuario == null)
            {
                return Resultado<Billetera>.Failure("Usuario no encontrado.");
            }

            if (usuario.IdTipoUsuario == 2)
            {
                return Resultado<Billetera>.Failure("El usuario tiene una billetera tipo 'Cobrador' y no puede crear billetera CuentaDni.");
            }
            bool existeBilletera = usuario.Billeteras.Any(b => b.Tipo == "CuentaDni");
            if (existeBilletera)
            {
                return Resultado<Billetera>.Failure("El usuario ya tiene una billetera de tipo CuentaDni.");
            }

            try {
                var billetera = new Billetera
                {
                    IdUsuario = usuario.IdUsuario,
                    Tipo = "CuentaDni",
                    Cvu = GenerarNumeroCvu(),
                    Saldo = 0.0m
                };
                _context.Billeteras.Add(billetera);
                await _context.SaveChangesAsync();
                return Resultado<Billetera>.Success(billetera);
            }
            catch(Exception ex)
            {
                return Resultado<Billetera>.Failure($"Error al crear la billetera: {ex.Message}");
            }
        }
        private string GenerarNumeroCvu()
        {
            Random random = new Random();
            string numero = "";

            for (int i = 0; i < 22; i++)
            {
                numero += random.Next(0, 10).ToString();
            }
            return numero;
        }    
    }
}