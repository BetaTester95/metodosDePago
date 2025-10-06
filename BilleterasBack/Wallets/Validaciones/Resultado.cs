namespace BilleterasBack.Wallets.Validaciones
{
    public class Resultado<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public static Resultado<T> Ok(T data) {
            return new Resultado<T> { Success  = true, Data = data };
        }
        public static Resultado<T> Failed(string error) {
            return new Resultado<T> { Success = false, Message = error }; 
        }
    }
}
