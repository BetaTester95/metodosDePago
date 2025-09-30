using BilleterasBack.Wallets.Data;
using BilleterasBack.Wallets.Servicios;
using BilleterasBack.Wallets.Shared;
using BilleterasBack.Wallets.Shared.Strategies.Mp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes("estaEsUnaClaveSuperSecreta123333!!");

builder.Services.AddCors(); // <-- Agrega esta línea
// Agregar servicios de Controllers
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor(); // <--- esto es clave
builder.Services.AddScoped<MpAgregarTarjeta>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<BilleterasBack.Wallets.Validaciones.Validador>();

// Configurar DbContext con la cadena de conexión
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<ContextoPago>(serviceProvider =>
{
    // Define un tipo por defecto o lee de configuración
    var tipoDefault = TipoMetodoPago.MercadoPago; // o desde config
    return new ContextoPago(tipoDefault);
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


var app = builder.Build();

// Configurar pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(options =>
    options.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
