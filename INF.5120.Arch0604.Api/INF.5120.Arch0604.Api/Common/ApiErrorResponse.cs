namespace INF._5120.Arch0604.Api.Common
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? Details { get; set; }

        public string TraceId { get; set; } = string.Empty;

        public DateTime TimestampUtc { get; set; }
    }
}