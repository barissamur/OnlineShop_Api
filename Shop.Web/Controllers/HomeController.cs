using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using System.Diagnostics;
using System.Net.Http;
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

        //var json = JsonConvert.SerializeObject(loginData);

        var json = JsonConvert.SerializeObject(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://localhost:44350/login?useCookies=true", content);

        if (response.IsSuccessStatusCode)
        {
            // Baþarýlý iþlem
            // Gerekli yönlendirme veya iþlemleri burada yapýn
        }
        else
        {
            // Hata durumu
            // Kullanýcýya hata mesajý göster
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
