namespace STFMS.API.DTOs.Common
{
    public class ApiResponseDTO<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponseDTO<T> SuccessResponse(T data, string message = "Operation successful")
        {
            return new ApiResponseDTO<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponseDTO<T> ErrorResponse(string message, List<string>? errors = null)
        {
            return new ApiResponseDTO<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }
}
