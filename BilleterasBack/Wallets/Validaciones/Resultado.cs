namespace BilleterasBack.Wallets.Validaciones
{
    public class Resultado<T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }
        public static Resultado<T> Success(T data) {
            return new Resultado<T> { IsSuccess = true, Data = data };
        }
        public static Resultado<T> Failed(string error) {
            return new Resultado<T> { IsSuccess = false, ErrorMessage = error }; 
        }
    }
}
