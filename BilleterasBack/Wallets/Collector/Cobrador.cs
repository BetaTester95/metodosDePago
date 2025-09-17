using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Validaciones;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BilleterasBack.Wallets.Collector.Cobrador
{   
        public class Cobrador
        {

        private readonly Validador _validador = new Validador();
        private readonly AppDbContext _context;

        public Cobrador(AppDbContext context)
        {    
            _context = context;
        }

        public async Task<Resultado<Billetera>> CrearCuentaCobrador(int dni)
        {
            if (_validador.validarDNI(dni))
                return Resultado<Billetera>.Failure("DNI debe ser mayor que cero y hasta 8 digitos");

            var usuarioCobrador = await _context.Usuarios.Include(u=> u.Billeteras).FirstOrDefaultAsync(u=> u.Dni == dni);

            if (usuarioCobrador == null)
            {
                return Resultado<Billetera>.Failure("Usuario no encontrado.");
            }

            if (usuarioCobrador.IdTipoUsuario != 2)
                return Resultado<Billetera>.Failure("El usuario no es de tipo 'Cobrador'.");

            bool existeCobrador = usuarioCobrador.Billeteras.Any(b => b.Tipo == "Cobrador");
            if (existeCobrador)
                return Resultado<Billetera>.Failure("El usuario ya tiene una billetera de tipo Cobrador.");

            try
            {
                var billetera = new Billetera
                {
                    IdUsuario = usuarioCobrador.IdUsuario,
                    Tipo = "Cobrador",
                    Cvu = GenerarNumeroCbu(),
                    Saldo = 0.0m
                };
                await _context.Billeteras.AddAsync(billetera);
                await _context.SaveChangesAsync();
                return Resultado<Billetera>.Success(billetera);
            }
            catch (Exception ex)
            {
                return Resultado<Billetera>.Failure("Error al crear la billetera: " + ex.Message);
            }      
        }

        public decimal retornarSaldo()
            {
                return 0;
            }

            public string retornarMail()
            {
                return "";
            }

            public decimal cobrarMonto(decimal monto)
            {
                return 0;
            }

            public string retornarCbuCobrador()
            {
                return "";
            }
           
            private string GenerarNumeroCbu()
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
