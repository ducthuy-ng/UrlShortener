using Microsoft.AspNetCore.Mvc;
using UrlShortener.Web.Repository;

namespace UrlShortener.Web.Controllers;

public class ShortenerController(IConfiguration configuration, IUrlRepository urlRepository)
    : Controller
{
    private readonly Random random = new();

    [HttpGet("/")]
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost("/")]
    public ActionResult ShortenNewUrl([FromForm] string url)
    {
        if (!IsValidUrl(url))
        {
            ViewData["ErrorMsg"] = "Invalid URL";
            return View("Index");
        }

        string shortenedUrlId;
        do
        {
            shortenedUrlId = GenerateRandomShortenUrlId();
        } while (urlRepository.ContainsKey(shortenedUrlId));

        urlRepository.Add(shortenedUrlId, url);

        return Redirect($"preview/{shortenedUrlId}");
    }

    private string GenerateRandomShortenUrlId(int length = 5)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(
            Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray()
        );
    }

    private static bool IsValidUrl(string? url)
    {
        if (url is null)
        {
            return false;
        }

        return Uri.IsWellFormedUriString(url, UriKind.Absolute);
    }

    [HttpGet("preview/{id}")]
    public ActionResult PreviewShortenedUrl(string id)
    {
        ViewData["ShortenedUrl"] = $"{configuration["DomainName"]}/{id}";
        return View();
    }

    [HttpGet("/{id}")]
    public ActionResult VisitUrl(string id)
    {
        if (!urlRepository.ContainsKey(id))
        {
            return Redirect("/");
        }

        return Redirect(urlRepository.GetValue(id) ?? "/");
    }
}
