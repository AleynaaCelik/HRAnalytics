namespace HRAnalytics.API.Response
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public static ApiResponse<T> SuccessResult(T data, string message = null) =>
            new() { Data = data, Success = true, Message = message };

        public static ApiResponse<T> FailureResult(string message, IEnumerable<string> errors = null) =>
            new() { Success = false, Message = message, Errors = errors };
    }

}
