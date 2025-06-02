namespace foroLIS_backend.DTOs
{
    public class Message<T>
    {
        public required string message { get; set; }
        public required T data { get; set; }
    }
}
