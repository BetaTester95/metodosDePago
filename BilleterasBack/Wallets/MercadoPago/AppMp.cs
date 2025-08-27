using EjercicioInterfaces;
using System;

public class AppMp
{

    public string nombre;
    public string apellido;
    public int dni;
    public string cvuMp;
    public decimal saldoCuentaMercadoPago = 0.00m;
    public Tarjeta? tarjeta;

    public AppMp(string nombre, string apellido, int dni)
    {
        this.nombre = nombre;
        this.apellido = apellido;
        this.dni = dni;
        cvuMp = GenerarNumeroCbu();
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
        saldoCuentaMercadoPago += saldoTarjetaCredito;
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

        if (montoTransferencia > saldoCuentaMercadoPago)
        {
            Console.WriteLine($"Saldo insuficiente para realizar esta operación. ");
            return false;
        }

        return true;
    }
}
