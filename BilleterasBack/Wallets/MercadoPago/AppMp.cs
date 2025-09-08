using BilleterasBack.Wallets.Models;
using System;


public class AppMp
{
    private readonly AppDbContext _context;

    public AppMp(AppDbContext context)
    {
        _context = context;
    }
    //public bool agregarDineroCuentaMp(decimal dinero)
    //{
    //    if (tarjeta == null || tarjeta.limiteSaldo == 0)
    //    {
    //        Console.WriteLine($"No tenes una tarjeta asociada para cargar saldo en MercadoPago.");
    //        return false;
    //    }

    //    if (dinero > tarjeta.limiteSaldo)
    //    {
    //       Console.WriteLine($"No es posible transferir su saldo: {tarjeta.limiteSaldo} es menor al saldo que desea transferir; ${dinero} Pesos");
    //        return false;
    //    }

    //    decimal saldoTarjetaCredito = tarjeta.SaldoLimite();
    //    saldoTarjetaCredito -= dinero;
    //    saldo_cuenta_mercado_pago += saldoTarjetaCredito;
    //    return true;
    //}

    public async Task<Billetera> CrearCuentaMercadoPago(Usuario usuario)
    {
        if(usuario == null)
        {
            throw new ArgumentNullException(nameof(usuario));
        }

        var billetera = new Billetera
        {
            IdUsuario = usuario.IdUsuario,
            Tipo = "MercadoPago",
            Cvu = GenerarNumeroCbu(),
            Saldo = 0.0m
        };
        _context.Billeteras.Add(billetera);
        await _context.SaveChangesAsync();
        return billetera;
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
