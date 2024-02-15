using System.Linq;
using System;
using TheShortinator.Models;
using Azure;
using TheShortinator.Data;
using Microsoft.EntityFrameworkCore;

namespace TheShortinator.AppLogic
{
    /// <summary>
    /// static class to create shortened urls
    /// </summary>
    public static class Shortinator
    {
        /// <summary>
        /// Generate token to identify url
        /// </summary>
        /// <returns></returns>
        private static string GenerateToken()
        {
            string urlsafe = string.Empty;
            Enumerable.Range(48, 75).Where(i => i < 58 || i > 64 && i < 91 || i > 96).OrderBy(o => new Random().Next()).ToList().ForEach(i => urlsafe += Convert.ToChar(i));
            string token = urlsafe.Substring(new Random().Next(0, urlsafe.Length), new Random().Next(2, 6));
            return token;
        }

        /// <summary>
        /// Create and save url
        /// </summary>
        /// <param name="url">URL to shorten</param>
        /// <param name="baseURL">Base url of application</param>
        /// <returns>ShortinatorURL</returns>
        public static async Task<ShortinatorURL> CreateShortenedUrl(string url, string baseURL)
        {
            using (var context = new URLDBContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    string token = GenerateToken();
                    var urls = await context.ShortinatorURLs.ToListAsync();
                    while (urls.Exists(u => u.Token == token)) ;
                    ShortinatorURL shortenedUrl = new ShortinatorURL() { Token = token, URL = url, ShortenedURL = baseURL + token };
                    if (!urls.Exists(u => u.URL == url))
                        context.ShortinatorURLs.Add(shortenedUrl);
                    context.SaveChanges();

                    dbContextTransaction.Commit();

                    return shortenedUrl;
                }
            }
        }
    }
}
