namespace TheShortinator.Models
{
    public class URLResponse
    {
        public required string URL { get; set; }
        public required string Status { get; set; }
        public string? Token { get; set; }
    }
}
