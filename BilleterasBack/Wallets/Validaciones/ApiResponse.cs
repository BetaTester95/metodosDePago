using System.Diagnostics;

namespace BilleterasBack.Wallets.Validaciones
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }

        public static ApiResponse<T> Ok(T data, string message = "Operación exitosa")
        {
           return new ApiResponse<T> { Success = true, Data = data, Message = message };
        }

        public static ApiResponse<T> Fail(string message, string error = "")
        {
         return new ApiResponse<T> { Success = false, Message = message, Error = error };
        }
    }
}
