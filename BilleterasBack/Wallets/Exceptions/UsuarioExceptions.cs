namespace BilleterasBack.Wallets.Exceptions
{
    public class UsuarioExceptions : Exception
    {

        public UsuarioExceptions(string mensaje) : base(mensaje)
        {
        }

    }


    public class BilleteraExceptions : Exception
    {
        public BilleteraExceptions(string mensaje) : base(mensaje) { }
    }
 
}
