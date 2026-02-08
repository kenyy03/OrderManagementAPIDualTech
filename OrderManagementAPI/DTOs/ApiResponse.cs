using System.Net;

namespace OrderManagementAPI.DTOs
{
    public class ApiResponse<T>
    {
        private static readonly string[] errorsValue = ["Internal Server Error"];

        public ApiResponse()
        {
            Message = string.Empty;
            Errors = [];
        }

        public T? Data { get; set; } = default;
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string[] Errors { get; set; }
        public HttpStatusCode Status { get; set; }

        public static ApiResponse<T> Success(T data, string message = "")
        {
            return new ApiResponse<T>
            {
                Data = data,
                Message = message,
                Status = HttpStatusCode.OK,
                Errors = [],
                IsSuccess = true
            };
        }

        public static ApiResponse<T> Failure(T data = default!, string message = "", string[]? errors = null)
        {
            return new ApiResponse<T>
            {
                Data = default,
                Message = message,
                Status = HttpStatusCode.BadRequest,
                Errors = errors ?? errorsValue,
                IsSuccess = false
            };
        }

        public static ApiResponse<T> NotFound(T data = default!, string message = "", string[]? errors = null)
        {
            return new ApiResponse<T>
            {
                Data = default,
                Message = message,
                Status = HttpStatusCode.NotFound,
                Errors = errors ?? errorsValue,
                IsSuccess = false
            };
        }
    }
}
