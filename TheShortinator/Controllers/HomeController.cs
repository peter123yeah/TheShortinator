using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using TheShortinator.AppLogic;
using TheShortinator.Data;
using TheShortinator.Models;

namespace TheShortinator.Controllers
{
    /// <summary>
    /// MVC Controller managing logic of default page of application
    /// </summary>
    /// <param name="logger"></param>
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;

        private string baseURL = string.Empty;

        /// <summary>
        /// Default page 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var uriBuilder = new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port ?? -1);
            if (uriBuilder.Uri.IsDefaultPort)
            {
                uriBuilder.Port = -1;
            }

            baseURL = uriBuilder.Uri.AbsoluteUri;
            return View();
        }

        /// <summary>
        /// Error VM
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Using token get full url and redirect webpage to full url
        /// </summary>
        /// <param name="token">token tp get full url</param>
        /// <returns></returns>
        [HttpGet, Route("/{token}")]
        public IActionResult TokenRedirect([FromRoute] string token)
        {
            try
            {
                using (var context = new URLDBContext())
                {
                    var urls = context.ShortinatorURLs.FirstOrDefault(s => s.Token == token);
                    if(urls != null)
                    {
                        return Redirect(urls.URL);
                    }
                    
                }

                return Json(new URLResponse() { URL = string.Empty, Status = "already shortened", Token = token });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// URL submitted to be shortened
        /// </summary>
        /// <param name="url">URL submitted to be shortened</param>
        /// <returns>Shortened url</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost, Route("/")]
        public IActionResult PostURL([FromBody] string url)
        {
            try
            {
                if (!url.Contains("http"))
                {
                    url = "http://" + url;
                }

                using (var context = new URLDBContext())
                {
                    if (context.ShortinatorURLs.Any(u => u.ShortenedURL == url))
                    {
                        Response.StatusCode = 405;
                        return Json(new URLResponse() { URL = url, Status = "already shortened", Token = null });
                    }
                }

                ShortinatorURL shortinatorURL = Shortinator.CreateShortenedUrl(url, baseURL);
                return Json(shortinatorURL.Token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
