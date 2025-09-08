using BilleterasBack.Wallets.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;


public class AppMp
{
    private readonly AppDbContext _context;

    public AppMp(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> agregarDineroCuentaMp(int dni, decimal monto)
    {
        var tarjetaUsuario = await _context.Tarjetas
       .Include(t => t.Billetera)
           .ThenInclude(b => b.Usuario)
       .FirstOrDefaultAsync(t => t.Billetera.Usuario.Dni == dni && t.Billetera.Tipo == "MercadoPago");

        if (tarjetaUsuario == null)
            return false;

        if(tarjetaUsuario.Saldo < monto)
            return false;

        var billetera = await _context.Billeteras
        .Include(b => b.Usuario)
        .FirstOrDefaultAsync(b => b.Usuario.Dni == dni && b.Tipo == "MercadoPago");

        if (billetera == null) return false;

        tarjetaUsuario.Saldo -= monto;
        billetera.Saldo += monto;
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<Billetera> CrearCuentaMercadoPago(int dni)
    {
        //validar que sea hasta 8 digitos
        if (dni <= 0 || dni > 99999999)
        {
            throw new Exception("DNI debe ser mayor que cero y hasta 8 digitos");
        }

        var usuario = await _context.Usuarios.Include(u => u.Billeteras).FirstOrDefaultAsync(u => u.Dni == dni);
        
        if (usuario == null) throw new Exception("Usuario no encontrado");

        bool existeBilletera = usuario.Billeteras.Any(b => b.Tipo == "MercadoPago");
        if (existeBilletera)
        {
            throw new Exception("Ya existe una billetera de MercadoPago para este usuario.");
        }

        bool existeCobrador = usuario.Billeteras.Any(b => b.Tipo == "Cobrador");
        if (existeCobrador)
        {
            throw new Exception("No se puede crear una billetera de MercadoPago para un usuario que es Cobrador.");
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
