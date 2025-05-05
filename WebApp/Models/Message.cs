namespace WebApp.Models
{
    public class Message
    {
        public required string Name { get; set; }
        public string? Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
