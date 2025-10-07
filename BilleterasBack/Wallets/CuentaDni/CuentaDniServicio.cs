using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Models;
using BilleterasBack.Wallets.Validaciones;
using Microsoft.EntityFrameworkCore;


namespace BilleterasBack.Wallets.CuentaDni
{

    public class CuentaDniServicio
    {
        private readonly AppDbContext _context;
        private readonly Validador _validador = new Validador();
        public string Message { get; set; } = string.Empty;

        public CuentaDniServicio(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Resultado<Billetera>> CrearCuentaDni(int dni) //crear billetera
        {
            if (!_validador.validarDNI(dni))
                return Resultado<Billetera>.Failed("DNI debe ser mayor que cero y hasta 8 digitos");

            var usuario = await _context.Usuarios.Include(u => u.Billeteras)
                                                 .FirstOrDefaultAsync(u => u.Dni == dni);
            if (usuario == null)
            {
                return Resultado<Billetera>.Failed("Usuario no encontrado.");
            }

            if (usuario.IdTipoUsuario == 2)
            {
                return Resultado<Billetera>.Failed("El usuario tiene una billetera tipo 'Cobrador' y no puede crear billetera CuentaDni.");
            }
            bool existeBilletera = usuario.Billeteras.Any(b => b.Tipo == "CuentaDni");
            if (existeBilletera)
            {
                return Resultado<Billetera>.Failed("El usuario ya tiene una billetera de tipo CuentaDni.");
            }

            try
            {
                var billetera = new Billetera
                {
                    IdUsuario = usuario.IdUsuario,
                    Tipo = "CuentaDni",
                    Cvu = GenerarNumeroCvu(),
                    Saldo = 0.0m
                };
                _context.Billeteras.Add(billetera);
                await _context.SaveChangesAsync();
                return Resultado<Billetera>.Ok(billetera);
            }
            catch (Exception ex)
            {
                return Resultado<Billetera>.Failed($"Error al crear la billetera: {ex.Message}");

            }
        }

        public async Task<bool> CargarSaldoCtaDni(int dni, decimal monto)
        {
            if (!_validador.validarDNI(dni))
            {
                Message = "DNI inválido.";
                return false;
            }

            if (!_validador.ValidarMonto(monto))
            {
                Message = "Monto inválido.";
                return false;
            }

            var usuario = await _context.Usuarios.Include(u => u.Billeteras)
                                                 .FirstOrDefaultAsync(u => u.Dni == dni);
            if (usuario == null)
            {
                Message = $"Usuario con DNI '{dni}' no encontrado.";
                return false;
            }

            var billetera = usuario.Billeteras.FirstOrDefault(b => b.Tipo == "CuentaDni");
            if (billetera == null)
            {
                Message = $"Billetera de tipo 'CuentaDni' no encontrada para el usuario con DNI '{dni}'.";
                return false;
            }

            billetera.Saldo += monto;
            await _context.SaveChangesAsync();
            return true;
        }

        private string GenerarNumeroCvu()
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