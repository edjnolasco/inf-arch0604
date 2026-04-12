namespace INF._5120.Arch0604.Application.Common
{
    public enum ServiceErrorType
    {
        None = 0,
        Validation = 1,
        NotFound = 2,
        Conflict = 3
    }

    public class ServiceResult<T>
    {
        public bool Success { get; private set; }

        public string Message { get; private set; } = string.Empty;

        public T Data { get; private set; } = default!;

        public ServiceErrorType ErrorType { get; private set; } = ServiceErrorType.None;

        private ServiceResult() { }

        // ✔ Éxito
        public static ServiceResult<T> Ok(T data, string message = "")
        {
            return new ServiceResult<T>
            {
                Success = true,
                Data = data,
                Message = message,
                ErrorType = ServiceErrorType.None
            };
        }

        // ✔ Fallo genérico
        public static ServiceResult<T> Fail(string message, ServiceErrorType errorType)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
                ErrorType = errorType,
                Data = default!
            };
        }

        // 🔥 Helpers (aquí mismo dentro de la clase)

        public static ServiceResult<T> Validation(string message)
            => Fail(message, ServiceErrorType.Validation);

        public static ServiceResult<T> NotFound(string message)
            => Fail(message, ServiceErrorType.NotFound);

        public static ServiceResult<T> Conflict(string message)
            => Fail(message, ServiceErrorType.Conflict);
    }
}