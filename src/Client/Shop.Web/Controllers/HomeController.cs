using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web;
using Shop.Web.Models;
using System.Diagnostics;
using System.Security.Permissions;
using System.Text;

namespace Shop.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory httpClientFactory;

    public HomeController(ILogger<HomeController> logger
        , IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        this.httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Privacy(string username, string password, bool useCookies)
    {
        var client = httpClientFactory.CreateClient();

        var data = new Dictionary<string, string>
        {
            { "email", username },
            { "password", password }
        };

        var json = JsonConvert.SerializeObject(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://localhost:44350/login?useCookies=true", content);

        string cookie = response.Headers.NonValidated["Set-Cookie"].ToString();
        string[] sa = cookie.Split('=');

        SetCookie("Security", sa[1].Split(';')[0], 90);

        return View();
    }

    private void SetCookie(string key, string value, int? expireTime)
    {
        CookieOptions option = new CookieOptions();

        if (expireTime.HasValue)
            option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
        else
            option.Expires = DateTime.Now.AddMilliseconds(10); // Örnek olarak çok kýsa bir süre

        Response.Cookies.Append(key, value, option);
    }

    [Security]
    public IActionResult Test()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
