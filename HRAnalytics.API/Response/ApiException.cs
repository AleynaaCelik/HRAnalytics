namespace HRAnalytics.API.Response
{
    public class ApiException
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public string TraceId { get; set; }
    }
}
