using EventBus;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineShop_Api.Messages;
using Shop.Web.Messages;
using Shop.Web.Models;
using System.Diagnostics;
using System.Text;

namespace Shop.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IEventBus eventBus;

    public HomeController(ILogger<HomeController> logger
        , IHttpClientFactory httpClientFactory
        , IEventBus eventBus)
    {
        _logger = logger;
        this.httpClientFactory = httpClientFactory;
        this.eventBus = eventBus;
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

        var response = await client.PostAsync("http://localhost:5005/login?useCookies=true", content);

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
        var stocks = new List<Stock>
        {
            new Stock { Id = 1, Name = "Apple Inc." },
            new Stock { Id = 2, Name = "Microsoft Corp." },
            new Stock { Id = 3, Name = "Amazon.com, Inc." },
            new Stock { Id = 4, Name = "Tesla, Inc." },
            new Stock { Id = 5, Name = "Facebook, Inc." },
            new Stock { Id = 6, Name = "Alphabet Inc." }, // Google'ýn ana þirketi
            new Stock { Id = 7, Name = "Berkshire Hathaway Inc." },
            new Stock { Id = 8, Name = "Johnson & Johnson" },
            new Stock { Id = 9, Name = "JPMorgan Chase & Co." },
            new Stock { Id = 10, Name = "Visa Inc." },
            new Stock { Id = 11, Name = "Procter & Gamble Co." },
            new Stock { Id = 12, Name = "UnitedHealth Group Incorporated" },
            // Diðer hisseler...
        };


        return View(stocks);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet("{controller}/{action}/{stock}")]
    public async Task<IActionResult> GetStock()
    {
        return View();
    }


    [HttpPost("{controller}/{action}")]
    public async Task<IActionResult> GetStock([FromBody] StockRequest request)
    {
        var testMessage = new TestMessage()
        {
            Content = request.Stock
        };

        await eventBus.PublishAsync(testMessage);

        return Ok(testMessage);
    }
}

public class StockRequest
{
    public string Stock { get; set; }
}