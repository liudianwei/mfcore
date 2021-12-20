namespace MF.FluentValidation
{
    public class HttpResult
    {
        public int Code { get; set; }

        public string Status { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}