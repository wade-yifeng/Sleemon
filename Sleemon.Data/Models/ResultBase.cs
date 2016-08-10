namespace Sleemon.Data
{
    public class ResultBase
    {
        public bool IsSuccess { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}
