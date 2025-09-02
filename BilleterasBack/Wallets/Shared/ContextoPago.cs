using BilleterasBack.Wallets.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using EjercicioInterfaces.Estrategias.ctdEstrategias;
//using EjercicioInterfaces.Estrategias.ppEstrategias;
//using EjercicioInterfaces.Pagos;
namespace BilleterasBack.Wallets.Shared
{
    public enum TipoMetodoPago
    {
        MercadoPago,
        PayPal,
        CuentaDNI
    }
    public class ContextoPago
    {
        public IAgregarCard? _agregarCard { get; set; }
        public IPagoCardTransferencia? _pagoTransferenciaEstrategia { get;  set; }
        public IpagoCardCred? _pagoCardEstrategia { get;  set; }
        public TipoMetodoPago TipoSeleccionado { get;  set; }

        // Constructor con tipo de método de pago
        public ContextoPago(TipoMetodoPago tipoMetodo)
        {
            TipoSeleccionado = tipoMetodo;
        }

        // Constructor con estrategia inicial
        public ContextoPago(TipoMetodoPago tipoMetodo, IAgregarCard agregarCard) : this(tipoMetodo)
        {
            _agregarCard = agregarCard;
        }

        // Método para cambiar el tipo de método de pago
        public void CambiarTipoMetodo(TipoMetodoPago nuevoTipo)
        {
            TipoSeleccionado = nuevoTipo;
            // Limpiar estrategias al cambiar de tipo para evitar inconsistencias
            _agregarCard = null;
            _pagoTransferenciaEstrategia = null;
            _pagoCardEstrategia = null;
        }

        // Método unificado para cambiar estrategias
        public void CambiarEstrategia(object estrategia)
        {
            switch (estrategia)
            {
                case IAgregarCard agregar:
                    _agregarCard = agregar;
                    break;
                case IPagoCardTransferencia transferencia:
                    _pagoTransferenciaEstrategia = transferencia;
                    break;
                case IpagoCardCred tarjetaPago:
                    _pagoCardEstrategia = tarjetaPago;
                    break;
                default:
                    throw new ArgumentException($"Tipo de estrategia no soportada: {estrategia.GetType().Name}");
            }
        }

        // Metodo generico para procesar cualquier tipo de operación
        public bool Procesar<T>(T estrategia, params object[] args)
        {
            try
            {
                switch (estrategia)
                {
                    case IAgregarCard a:
                        return a.AgregarTarjeta(
                            (string)args[0],      // numTarjeta
                            (string)args[1],      // nombre
                            (string)args[2],      // apellido
                            (int)args[3],         // dni
                            (DateTime)args[4],    // fechaVenc
                            (int)args[5]          // cod
                        );

                    case IPagoCardTransferencia t:
                        t.PagoConTransferencia(
                            (decimal)args[0],     // monto
                            (string)args[1]       // cuentaDestino
                        );
                        return true;

                    case IpagoCardCred c:
                        c.PagoConTarjetaCredito(
                            (decimal)args[0],     // monto
                            (int)args[1]          // cuotas
                        );
                        return true;

                    default:
                        throw new ArgumentException($"Estrategia no soportada: {typeof(T).Name}");
                }
            }
            catch (Exception ex)
            {
                // Log del error específico del método de pago
                Console.WriteLine($"Error procesando {TipoSeleccionado}: {ex.Message}");
                return false;
            }
        }
        //// Método de conveniencia para agregar tarjeta
        public bool agregartarjeta(string numTarjeta, string nombre, string apellido, int dni, DateTime fechaVenc, int cod)
        {
            if (_agregarCard == null)
                throw new InvalidOperationException($"no hay estrategia de agregar tarjeta configurada para {TipoSeleccionado}");

            return Procesar(_agregarCard, numTarjeta, nombre, apellido, dni, fechaVenc, cod);
        }
        // método de conveniencia para pago con transferencia
        public bool pagocontransferencia(decimal monto, string cuentadestino)
        {
            if (_pagoTransferenciaEstrategia == null)
                throw new InvalidOperationException($"no hay estrategia de transferencia configurada para {TipoSeleccionado}");

            return Procesar(_pagoTransferenciaEstrategia, monto, cuentadestino);
        }

        // método de conveniencia para pago con tarjeta de crédito
        public bool pagocontarjetacredito(decimal monto, int cuotas)
        {
            if (_pagoCardEstrategia == null)
                throw new InvalidOperationException($"no hay estrategia de tarjeta de crédito configurada para {TipoSeleccionado}");

            return Procesar(_pagoCardEstrategia, monto, cuotas);
        }

        // Método para obtener información del contexto actual
        public string ObtenerInformacionContexto()
        {
            var estrategias = new List<string>();

            if (_agregarCard != null) estrategias.Add("Agregar Tarjeta");
            if (_pagoTransferenciaEstrategia != null) estrategias.Add("Transferencia");
            if (_pagoCardEstrategia != null) estrategias.Add("Tarjeta de Crédito");

            return $"Método: {TipoSeleccionado}, Estrategias disponibles: {string.Join(", ", estrategias)}";
        }
    }
    // Factory para crear contextos específicos (opcional, para mayor claridad)
    public static class ContextoPagoFactory
    {
        public static ContextoPago CrearMercadoPago()
        {
            return new ContextoPago(TipoMetodoPago.MercadoPago);
        }

        public static ContextoPago CrearPayPal()
        {
            return new ContextoPago(TipoMetodoPago.PayPal);
        }

        public static ContextoPago CrearCuentaDNI()
        {
            return new ContextoPago(TipoMetodoPago.CuentaDNI);
        }
    }
}
