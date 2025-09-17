using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Validaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;


public class AppMp
{
    private readonly AppDbContext _context;
    private readonly Validador _validador = new Validador();
    public AppMp(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> agregarDineroCuentaMp(int dni, decimal monto)
    {
        if(!_validador.validarDNI(dni))
            return false;
       
        if(!_validador.ValidarMonto(monto))
            return false;
        
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

    public async Task<Resultado<Billetera>> CrearCuentaMercadoPago(int dni)//ok
    {
        
            if (!_validador.validarDNI(dni)) 
                return Resultado<Billetera>.Failure("DNI debe ser mayor que cero y hasta 8 digitos");
 
            var usuario = await _context.Usuarios.Include(u => u.Billeteras).Include
            (u => u.TipoUsuario).FirstOrDefaultAsync(u => u.Dni == dni);

            if (usuario == null) 
                return Resultado<Billetera>.Failure("Usuario no encontrado.");

            if(usuario.IdTipoUsuario == 1)
                return Resultado<Billetera>.Failure("No se puede crear una billetera de MercadoPago para un usuario Cobrador.");

            bool existeBilletera = usuario.Billeteras.Any(b => b.Tipo == "MercadoPago");
                if (existeBilletera) 
                return Resultado<Billetera>.Failure("Ya existe una billetera de MercadoPago para este usuario.");

        try
        {
            var billetera = new Billetera
            {
                IdUsuario = usuario.IdUsuario,
                Tipo = "MercadoPago",
                Cvu = GenerarNumeroCbu(),
                Saldo = 0.0m
            };
            _context.Billeteras.Add(billetera);
            await _context.SaveChangesAsync();
            return Resultado<Billetera>.Success(billetera);
        }
        catch (Exception ex)
        {
            return Resultado<Billetera>.Failure($"Error en el servidor: {ex.Message}");
        }
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
