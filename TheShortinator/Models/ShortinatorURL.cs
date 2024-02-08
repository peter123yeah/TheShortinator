using System;

namespace TheShortinator.Models
{
    public class ShortinatorURL
    {
        public Guid ID { get; set; }
        public required string URL { get; set; }
        public required string ShortenedURL { get; set; }
        public required string Token { get; set; }

    }
}
