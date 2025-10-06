using BilleterasBack.Wallets.Data;
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
        private readonly Validador? _validaciones;
        public string Message { get; private set; } = string.Empty;
        public DateTime? fechaVenc { get; set; }


        public paypAgregarTarjeta(AppDbContext context, Validador validador)
        {
            _context = context;
            _validaciones = validador;
        }

        public bool AgregarTarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {

            if (!_validaciones.validarNumTarjeta(numTarjeta))
            {
                this.Message = "El numero de tarjeta no es valido";
                return false;
            }

            if (!_validaciones.validarNombre(nombre))
            {
                this.Message = "El nombre no es valido";
                return false;
            }

            if (!_validaciones.validarApellido(apellido))
            {
                this.Message = "El apellido no es valido";
                return false;
            }

            if (!_validaciones.validarDNI(dni))
            {
                this.Message = "Error al validar el dni";
                return false;
            }

            if (!_validaciones.EsFechaVencimientoValida(fechaVenc))
            {
                this.Message = "La fecha de vencimiento no es valida";
                return false;
            }

            if (!_validaciones.validarCod(cod))
            {
                this.Message = "Error al validar el codigo de la tarjeta";
                return false;
            }

                DateTime ahora = DateTime.Now;

                var identificacionPaypal = _context.Billeteras
                   .Include(b => b.Usuario)
                   .FirstOrDefault(b => b.Tipo == "PayPal" && b.Usuario!.Dni == dni);

                if(identificacionPaypal == null)
                    {
                        Message = "No se encontró una billetera PayPal para el usuario.";
                        return false;
                    }
                            
                var tarjetaExistente = _context.Tarjetas
                    .FirstOrDefault(t => t.NumeroTarjeta == numTarjeta && t.IdBilletera == identificacionPaypal.IdBilletera);

                if (tarjetaExistente != null)
                {
                    Message = "Ya existe una tarjeta con este número en la billetera de PayPal.";
                    return false;
                }

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
