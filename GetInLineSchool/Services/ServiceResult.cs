namespace GetInLineSchool.Services
{
    public class Error
    {
        public string Key { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public List<Error>? Errors { get; set; }

        public static ServiceResult<T>Success(T data)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                Errors = null
            };
        }

        public static ServiceResult<T>Failure(T data, List<Error> errors)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Data = data,
                Errors = errors
            };
        }
    }
}
