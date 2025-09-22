
using BilleterasBack.Wallets.Collector.Cobrador;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Shared.Strategies.Mp;
using Microsoft.EntityFrameworkCore;
using System;
using BilleterasBack.Wallets.PayPal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using BilleterasBack.Wallets.Validaciones;
using BilleterasBack.Wallets.Data;

namespace BilleterasBack.Wallets.PayPal
{
    public class PayPalServicio
    {
        private readonly AppDbContext _context;
        private readonly Validador _validador = new Validador();

        public PayPalServicio(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Resultado<Billetera>> CrearCuentaPayPal(string email)
        {     
            if(!_validador.mailValidar(email))           
                    return Resultado<Billetera>.Failure("Correo electrónico no válido.");
            
            var usuario = await _context.Usuarios.Include(u=> u.Billeteras).FirstOrDefaultAsync(u => u.Email == email);
                if (usuario == null)
                    return Resultado<Billetera>.Failure("Email no encontrado.");
                      
                if (usuario.IdTipoUsuario == 1)
                    return Resultado<Billetera>.Failure("El usuario tiene una billetera tipo 'Cobrador' y no puede crear billetera PayPal.");

            bool existeBilletera = usuario.Billeteras.Any(b => b.Tipo == "PayPal");
                if (existeBilletera) 
                    return Resultado<Billetera>.Failure("El usuario ya esta registrado.");
                
                if(usuario.Email == null)
                    return Resultado<Billetera>.Failure("El usuario no tiene un correo electrónico asociado.");

                if (!_validador.mailValidar(usuario.Email))
                    return Resultado<Billetera>.Failure("Correo electrónico no válido.");
            
            try {
                var billetera = new Billetera
                {
                    IdUsuario = usuario.IdUsuario,
                    Tipo = "PayPal",
                    Cvu = usuario.Email,
                    Saldo = 0.0m
                };
                await _context.Billeteras.AddAsync(billetera);
                await _context.SaveChangesAsync();
                return Resultado<Billetera>.Success(billetera);
            }
            catch(Exception ex)
            {
                return Resultado<Billetera>.Failure($"Error al crear la billetera: {ex.Message}");
            }
        }
        
        public bool RealizarCobro(decimal cobro)
        {
            // Verificar que el cobro no sea negativo y que haya saldo suficiente
            if (cobro < 0)
            {
                return false; // Indica un error
            }

            //if (cobro > tarjeta?.limiteSaldo)
            //{
            //    Console.WriteLine("No hay suficiente saldo para realizar el cobro.");
            //    return true; // saldo insuficiente
            //}

            // restamos el cobro del saldo disponible
            //tarjeta?.limiteSaldo -= cobro; heck

            //Console.WriteLine($"Cobro realizado con éxito. Saldo restante: {tarjeta?.limiteSaldo}");
            return true;
        }

        public bool AgregarSaldoPaypal(decimal saldo)
        {
            //if (tarjeta == null)
            //{
            //    Console.WriteLine($"no es posible agregar saldo, no tiene tarjeta asociada. ");
            //    return false;
            //}

            //if (tarjeta.limiteSaldo == 0)
            //{
            //    Console.WriteLine($"Error el limite de su saldo es 0 USD. ");

            //    return false;
            //}

            if (saldo <= 0)
            {
                Console.WriteLine("El saldo a agregar debe ser mayor a cero USD.");
                return false;
            }

            //if (tarjeta.limiteSaldo < saldo)
            //{
            //    Console.WriteLine($"No posee suficiente saldo para esta operacion. ");
            //    return false;
            //}

            //tarjeta.limiteSaldo -= saldo;
            //saldoPayPal += saldo;
            //Console.WriteLine($"Saldo agregado correctamente. Nuevo saldo PayPal: ${saldoPayPal} USD");
            return true;
        }

    }
}
