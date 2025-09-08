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
        private int _idDni;

        public MpPagoConTarjetaCredito(AppDbContext context)
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

           var checkCobrador = _context.Billeteras.Include(b=> b.Usuario).FirstOrDefault(b => b.Tipo =="Cobrador" && cvuCobrador == b.Cvu); //revisamos que exista el cobrador
           var checkTarjeta = _context.Tarjetas.FirstOrDefault(t => t.IdTarjeta == _idDni); //revisamos que tengamos una tarjeta asociada
           var checkSaldo = _context.Billeteras.Include(u => u.Usuario).FirstOrDefault(b=> b.IdBilletera == idDni);

            if (checkCobrador == null)
            {
                return false;
            }

            if(checkTarjeta == null)
            {
                 return false;
            }
            if(checkSaldo == null)
            {
                return false;
            }
            decimal resultado;

            if (montoPagar <= 0)
            {
                Console.WriteLine("Error el monto es 0");
                return false;
            }

            if (montoPagar > checkSaldo.Saldo)
            {
                Console.WriteLine($"El monto supera su saldo de la tarjeta de credito: ${checkSaldo.Saldo} Pesos");
                return false;
            }
            if (cantCuotas > 12)
            {
                Console.WriteLine($"La cantidad de cuotas que selecciono {cantCuotas} no estan permitidas. ");
                return false;
            }

            if (cantCuotas == 1)
            {
                checkSaldo.Saldo -= montoPagar;             
                checkCobrador.Saldo += montoPagar;
                return true;
            }
            if (cantCuotas == 3)
            {
                resultado = montoPagar + (0.15m * montoPagar);

                checkSaldo.Saldo -= resultado;
                checkCobrador.Saldo += resultado;
                //Console.WriteLine($"\n");
                //Console.WriteLine($"DATOS DEL COBRADOR: ");
                //Console.WriteLine($"Nombre Completo: {cobrador.nombre} {cobrador.apellido} ");
                //Console.WriteLine($"DNI: {cobrador.dni}");
                //Console.WriteLine($"Se realizo el pago total de: ${resultado}Pesos");
                //Console.WriteLine($"\n");

                return true;
            }

            if (cantCuotas == 6)
            {
                resultado = montoPagar + (0.20m * montoPagar);

                checkSaldo.Saldo -= resultado;
                checkCobrador.Saldo += resultado;
                //Console.WriteLine($"\n");
                //Console.WriteLine($"Su saldo de la tarjeta de credito actualmente: ${checkSaldo.Saldo} Pesos");
                //Console.WriteLine($"DATOS DEL COBRADOR: ");
                //Console.WriteLine($"Nombre Completo: {cobrador.nombre} {cobrador.apellido} ");
                //Console.WriteLine($"DNI: {cobrador.dni}");
                //Console.WriteLine($"Se realizo el pago total de: ${resultado}Pesos");
                //Console.WriteLine($"\n");
                return true;

            }
            if (cantCuotas == 12)
            {
                resultado = montoPagar + (0.25m * montoPagar);

                checkSaldo.Saldo -= resultado;
                checkSaldo.Saldo += resultado;
                //Console.WriteLine($"\n");
                //Console.WriteLine($"Su saldo de la tarjeta de credito actualmente: ${checkSaldo.Saldo} Pesos");
                //Console.WriteLine($"DATOS DEL COBRADOR: ");
                //Console.WriteLine($"Nombre Completo: {cobrador.nombre} {cobrador.apellido} ");
                //Console.WriteLine($"DNI: {cobrador.dni}");
                //Console.WriteLine($"Se realizo el pago total de: ${resultado}Pesos");
                //Console.WriteLine($"\n");
                return true;

            }
            Console.WriteLine($"Error no se pudo pagar. ");
            return false;
        }
        
    }
}
