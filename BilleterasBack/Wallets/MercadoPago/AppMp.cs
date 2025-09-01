using BilleterasBack.Wallets.Cards;
using System;


public class AppMp
{
    public string nombre { get; set; }
    public string apellido { get; set; }
    public int dni { get ; set; }
    public string cvu_mp { get; set; }
    public decimal saldo_cuenta_mercado_pago { get; set; } = 0.00m;
    public Tarjeta? tarjeta;

    public AppMp(string nombre, string apellido, int dni)
    {
        this.nombre = nombre;
        this.apellido = apellido;
        this.dni = dni;
        this.saldo_cuenta_mercado_pago = 0.0m;
        this.cvu_mp = GenerarNumeroCbu();
    }
    public bool agregarDineroCuentaMp(decimal dinero)
    {
        if (tarjeta == null || tarjeta.limiteSaldo == 0)
        {
            Console.WriteLine($"No tenes una tarjeta asociada para cargar saldo en MercadoPago.");
            return false;
        }

        if (dinero > tarjeta.limiteSaldo)
        {
            Console.WriteLine($"No es posible transferir su saldo: {tarjeta.limiteSaldo} es menor al saldo que desea transferir; ${dinero} Pesos");
            return false;
        }

        decimal saldoTarjetaCredito = tarjeta.SaldoLimite();
        saldoTarjetaCredito -= dinero;
        saldo_cuenta_mercado_pago += saldoTarjetaCredito;
        return true;
    }

    public string GenerarNumeroCbu()
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
        if (montoTransferencia <= 0)
        {
            Console.WriteLine($"El monto a transferir no puede ser $0 Pesos.");
            return false;
        }

        if (montoTransferencia > saldo_cuenta_mercado_pago)
        {
            Console.WriteLine($"Saldo insuficiente para realizar esta operación. ");
            return false;
        }

        return true;
    }
}
