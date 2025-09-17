namespace BilleterasBack.Wallets.Validaciones
{
    public class Resultado<T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }  // mensaje de error si falló

        public T? Data { get; set; }
        public static Resultado<T> Success(T data) => new Resultado<T> { IsSuccess = true, Data = data };
        public static Resultado<T> Failure(string error) => new Resultado<T> { IsSuccess = false, ErrorMessage = error };

    }
}
