namespace Application.Models
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; } = default;
        public string? Message { get; set; } = default;
        public IReadOnlyList<string>? Errors { get; set; } = default;

        public ApiResponse()
        {
        }

        private ApiResponse(T data, bool isSuccess = true, string? message = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
        }

        private ApiResponse(string[]? errors = null, bool isSuccess = false)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static ApiResponse<T> Success(T data) => new(data);
        public static ApiResponse<T> Success(T data, string? message = null) => new(data, true, message);
        public static ApiResponse<T> Failure(params string[]? errors) => new(errors, false);

        public TResult Match<TResult>(Func<ApiResponse<T>, TResult> onSuccess, Func<string[]?, TResult> onFailure)
        {
            return IsSuccess && Data is not null ? onSuccess(new(Data)) : onFailure(Errors?.ToArray());
        }

        public void Match(Action<T> onSuccess, Action<string[]?> onFailure)
        {
            if (IsSuccess && Data is not null)
            {
                onSuccess(Data);
            }
            else
            {
                onFailure(Errors?.ToArray());
            }
        }
    }
}
