using BilleterasBack.Wallets.Models;
using System;


public class AppMp
{
    private readonly AppDbContext _context;

    public AppMp(AppDbContext context)
    {
        _context = context;
    }
    public bool agregarDineroCuentaMp(decimal dinero)
    {
        //if (tarjeta == null || tarjeta.limiteSaldo == 0)
        //{
        //    Console.WriteLine($"No tenes una tarjeta asociada para cargar saldo en MercadoPago.");
        //    return false;
        //}

        //if (dinero > tarjeta.limiteSaldo)
        //{
        //    Console.WriteLine($"No es posible transferir su saldo: {tarjeta.limiteSaldo} es menor al saldo que desea transferir; ${dinero} Pesos");
        //    return false;
        //}

        //decimal saldoTarjetaCredito = tarjeta.SaldoLimite();
        //saldoTarjetaCredito -= dinero;
        //saldo_cuenta_mercado_pago += saldoTarjetaCredito;
        return true;
    }

    public async Task<Mp> CrearCuenta(Usuario usuario)
    {
        var nuevaCuenta = new Mp
        {
            id_usuario = usuario.id_usuario,
            nombre = usuario.nombre,
            apellido = usuario.apellido,
            dni = usuario.dni,
            cvu_mp = GenerarNumeroCbu(),
            saldo_cuenta_mercado_pago = 0.0m
        };
        _context.Mp.Add(nuevaCuenta);
        await _context.SaveChangesAsync();
        return nuevaCuenta;

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

    public bool ValidarMonto(decimal montoTransferencia)
    {
        //    if (montoTransferencia <= 0)
        //    {
        //        Console.WriteLine($"El monto a transferir no puede ser $0 Pesos.");
        //        return false;
        //    }

        //    if (montoTransferencia > saldo_cuenta_mercado_pago)
        //    {
        //        Console.WriteLine($"Saldo insuficiente para realizar esta operación. ");
        //        return false;
        //    }

        return true;
        //}
    }
}
