using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;


namespace TheShortinator.Models
{
    public class ShortinatorURL
    {
        public Guid ID { get; set; }
        public required string URL { get; set; }
        public required string ShortenedURL { get; set; }
        public required string Token { get; set; }

        public ShortinatorURL()
        {
            ID = Guid.NewGuid();
        }
    }


}
